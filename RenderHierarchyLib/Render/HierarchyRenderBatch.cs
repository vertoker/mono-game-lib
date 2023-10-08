﻿using RenderHierarchyLib;
using RenderHierarchyLib.Extensions;
using RenderHierarchyLib.Models;
using RenderHierarchyLib.Models.Transform;
using RenderHierarchyLib.Render.Sprite;
using System;
using static System.Formats.Asn1.AsnWriter;
using System.Drawing;
using RenderHierarchyLib.Models.Text;

namespace Microsoft.Xna.Framework.Graphics
{
    public class HierarchyRenderBatch : CustomGraphicsResource
    {
        private readonly HierarchySpriteBatcher _batcher;
        private readonly Camera _camera;

        private BlendState _blendState;
        private SamplerState _samplerState;
        private DepthStencilState _depthStencilState;
        private RasterizerState _rasterizerState;

        private readonly EffectPass _spritePass;
        private SpriteEffect _spriteEffect;

        private bool _beginCalled;
        private float _pixelScale;
        private Vector2 _posPixelScale;

        public HierarchyRenderBatch(GraphicsDevice graphicsDevice, Camera camera, int capacity = 256)
        {
            GraphicsDevice = graphicsDevice ?? 
                throw new ArgumentNullException("graphicsDevice", "The GraphicsDevice must not be null when creating new resources.");
            _camera = camera;

            _spriteEffect = new SpriteEffect(graphicsDevice);
            _spritePass = _spriteEffect.CurrentTechnique.Passes[0];
            _batcher = new HierarchySpriteBatcher(graphicsDevice, capacity);
        }

        public void Begin(BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null)
        {
            if (_beginCalled)
                throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");

            _blendState = blendState ?? BlendState.AlphaBlend;
            _samplerState = samplerState ?? SamplerState.LinearClamp;
            _depthStencilState = depthStencilState ?? DepthStencilState.None;
            _rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;

            _pixelScale = _camera.PixelScale;
            _posPixelScale = new(_pixelScale, -_pixelScale);
            _camera.UpdateAnchors(_pixelScale);

            _beginCalled = true;
        }
        public void End()
        {
            if (!_beginCalled)
                throw new InvalidOperationException("Begin must be called before calling End.");

            var device = GraphicsDevice;
            device.BlendState = _blendState;
            device.DepthStencilState = _depthStencilState;
            device.RasterizerState = _rasterizerState;
            device.SamplerStates[0] = _samplerState;
            _spritePass.Apply();

            _batcher.DrawBatch();
            _beginCalled = false;
        }

        private void CheckValid(Texture2D texture)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");

            if (!_beginCalled)
                throw new InvalidOperationException("Draw was called, but Begin has not yet been called. Begin must be called successfully before you can call Draw.");
        }
        private void CheckValid(CustomSpriteFont spriteFont, string text)
        {
            if (spriteFont == null)
            {
                throw new ArgumentNullException("spriteFont");
            }

            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (!_beginCalled)
            {
                throw new InvalidOperationException("DrawString was called, but Begin has not yet been called. Begin must be called successfully before you can call DrawString.");
            }
        }

        public HierarchySpriteBatchItem CreateBatchItem() => _batcher.CreateBatchItem();

        #region Tests
        public void RenderTextTest(CustomSpriteFont font, string text)
        {
            CheckValid(font, text);
            RenderTest(font.Texture);
        }

        public void RenderTest(Texture2D texture, float angle = 0)
        {
            CheckValid(texture);
            var spriteBatchItem = CreateBatchItem();
            spriteBatchItem.Texture = texture;
            spriteBatchItem.SortKey = 0;

            var rotation = MathHelper.ToRadians(angle);
            var sin = MathF.Sin(rotation);
            var cos = MathF.Cos(rotation);

            var vector = new Vector3(1, 1, 0);
            var scaleM1 = new Vector2(-100, -100);
            var scaleM2 = new Vector2(100, -100);
            var scaleM3 = new Vector2(-100, 100);
            var scaleM4 = new Vector2(100, 100);
            var scale1 = new Vector3(scaleM1.X * cos - scaleM1.Y * sin, scaleM1.X * sin + scaleM1.Y * cos, 0);
            var scale2 = new Vector3(scaleM2.X * cos - scaleM2.Y * sin, scaleM2.X * sin + scaleM2.Y * cos, 0);
            var scale3 = new Vector3(scaleM3.X * cos - scaleM3.Y * sin, scaleM3.X * sin + scaleM3.Y * cos, 0);
            var scale4 = new Vector3(scaleM4.X * cos - scaleM4.Y * sin, scaleM4.X * sin + scaleM4.Y * cos, 0);

            var vector2 = new Vector2(1, 1);
            var mark1 = new Vector2(0, 0);
            var mark2 = new Vector2(1, 0);
            var mark3 = new Vector2(0, 1);
            var mark4 = new Vector2(1, 1);

            var baseVector = new Vector3(200, 200, 0);

            // 1 2 3 4 - (1, 1)
            // 2 1 4 3 - (-1, 1)
            // 3 4 1 2 - (1, -1)
            // 4 3 2 1 - (-1, -1)

            spriteBatchItem.vertexTL = new VertexPositionColorTexture(baseVector + scale1 * vector, Color.White, mark1 * vector2);
            spriteBatchItem.vertexTR = new VertexPositionColorTexture(baseVector + scale2 * vector, Color.White, mark2 * vector2);
            spriteBatchItem.vertexBL = new VertexPositionColorTexture(baseVector + scale3 * vector, Color.White, mark3 * vector2);
            spriteBatchItem.vertexBR = new VertexPositionColorTexture(baseVector + scale4 * vector, Color.White, mark4 * vector2);
            //spriteBatchItem.Set(25, 0, 150, 200, Color.White, Vector2.Zero, Vector2.One, 0);
        }
        #endregion


        public unsafe void DrawString(CustomSpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            CheckValid(spriteFont, text);
            float sortKey = 0f;

            Vector2 zero = Vector2.Zero;
            bool flag = (effects & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically;
            bool flag2 = (effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally;
            if (flag || flag2)
            {
                spriteFont.MeasureString(ref text, out var size);
                if (flag2)
                {
                    origin.X *= -1f;
                    zero.X = 0f - size.X;
                }

                if (flag)
                {
                    origin.Y *= -1f;
                    zero.Y = (float)spriteFont.LineSpacing - size.Y;
                }
            }

            Matrix matrix = Matrix.Identity;
            float num = 0f;
            float num2 = 0f;
            if (rotation == 0f)
            {
                matrix.M11 = (flag2 ? (0f - scale.X) : scale.X);
                matrix.M22 = (flag ? (0f - scale.Y) : scale.Y);
                matrix.M41 = (zero.X - origin.X) * matrix.M11 + position.X;
                matrix.M42 = (zero.Y - origin.Y) * matrix.M22 + position.Y;
            }
            else
            {
                num = MathF.Cos(rotation);
                num2 = MathF.Sin(rotation);
                matrix.M11 = (flag2 ? (0f - scale.X) : scale.X) * num;
                matrix.M12 = (flag2 ? (0f - scale.X) : scale.X) * num2;
                matrix.M21 = (flag ? (0f - scale.Y) : scale.Y) * (0f - num2);
                matrix.M22 = (flag ? (0f - scale.Y) : scale.Y) * num;
                matrix.M41 = (zero.X - origin.X) * matrix.M11 + (zero.Y - origin.Y) * matrix.M21 + position.X;
                matrix.M42 = (zero.X - origin.X) * matrix.M12 + (zero.Y - origin.Y) * matrix.M22 + position.Y;
            }

            /*Vector2 zero2 = Vector2.Zero;
            bool flag3 = true;
            fixed (SpriteFont.Glyph* ptr = spriteFont.Glyphs)
            {
                foreach (char c in text)
                {
                    switch (c)
                    {
                        case '\n':
                            zero2.X = 0f;
                            zero2.Y += spriteFont.LineSpacing;
                            flag3 = true;
                            continue;
                        case '\r':
                            continue;
                    }

                    int glyphIndexOrDefault = spriteFont.GetGlyphIndexOrDefault(c);
                    SpriteFont.Glyph* ptr2 = ptr + glyphIndexOrDefault;
                    if (flag3)
                    {
                        zero2.X = Math.Max(ptr2->LeftSideBearing, 0f);
                        flag3 = false;
                    }
                    else
                    {
                        zero2.X += spriteFont.Spacing + ptr2->LeftSideBearing;
                    }

                    Vector2 position2 = zero2;
                    if (flag2)
                    {
                        position2.X += ptr2->BoundsInTexture.Width;
                    }

                    position2.X += ptr2->Cropping.X;
                    if (flag)
                    {
                        position2.Y += ptr2->BoundsInTexture.Height - spriteFont.LineSpacing;
                    }

                    position2.Y += ptr2->Cropping.Y;
                    Vector2.Transform(ref position2, ref matrix, out position2);
                    var spriteBatchItem = CreateBatchItem();
                    spriteBatchItem.Texture = spriteFont.Texture;
                    spriteBatchItem.SortKey = sortKey;
                    _texCoordTL.X = (float)ptr2->BoundsInTexture.X * spriteFont.Texture.TexelWidth;
                    _texCoordTL.Y = (float)ptr2->BoundsInTexture.Y * spriteFont.Texture.TexelHeight;
                    _texCoordBR.X = (float)(ptr2->BoundsInTexture.X + ptr2->BoundsInTexture.Width) * spriteFont.Texture.TexelWidth;
                    _texCoordBR.Y = (float)(ptr2->BoundsInTexture.Y + ptr2->BoundsInTexture.Height) * spriteFont.Texture.TexelHeight;
                    if ((effects & SpriteEffects.FlipVertically) != 0)
                    {
                        float y = _texCoordBR.Y;
                        _texCoordBR.Y = _texCoordTL.Y;
                        _texCoordTL.Y = y;
                    }

                    if ((effects & SpriteEffects.FlipHorizontally) != 0)
                    {
                        float x = _texCoordBR.X;
                        _texCoordBR.X = _texCoordTL.X;
                        _texCoordTL.X = x;
                    }

                    if (rotation == 0f)
                    {
                        spriteBatchItem.Set(position2.X, position2.Y, (float)ptr2->BoundsInTexture.Width * scale.X, (float)ptr2->BoundsInTexture.Height * scale.Y, color, _texCoordTL, _texCoordBR, layerDepth);
                    }
                    else
                    {
                        spriteBatchItem.Set(position2.X, position2.Y, 0f, 0f, (float)ptr2->BoundsInTexture.Width * scale.X, (float)ptr2->BoundsInTexture.Height * scale.Y, num2, num, color, _texCoordTL, _texCoordBR, layerDepth);
                    }

                    zero2.X += ptr2->Width + ptr2->RightSideBearing;
                }
            }

            FlushIfNeeded();*/
        }


        #region World Render Methods
        public void WorldRender(TextureView2D view, RenderObject transform) =>
            WorldRender(view.Texture, view.ViewStart, view.ViewEnd, view.ColorTL, view.ColorTR, view.ColorBL, view.ColorBR,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void WorldRender(TextureView2D view, TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(view.Texture, view.ViewStart, view.ViewEnd, view.ColorTL, view.ColorTR, view.ColorBL, view.ColorBR,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void WorldRender(TextureView2D view, Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(view.Texture, view.ViewStart, view.ViewEnd, view.ColorTL, view.ColorTR, view.ColorBL, view.ColorBR,
                pos, rot, sca, anchor, pivot, depth);

        public void WorldRender(Texture2D texture, Rectangle rectangle, Color color,
            RenderObject transform) =>
            WorldRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                color, color, color, color, transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void WorldRender(Texture2D texture, Rectangle rectangle, Color color,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                color, color, color, color, transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void WorldRender(Texture2D texture, Rectangle rectangle, Color color,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                color, color, color, color, pos, rot, sca, anchor, pivot, depth);

        public void WorldRender(Texture2D texture, Rectangle rectangle, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            RenderObject transform) =>
            WorldRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                colorTL, colorTR, colorBL, colorBR, transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void WorldRender(Texture2D texture, Rectangle rectangle, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                colorTL, colorTR, colorBL, colorBR, transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void WorldRender(Texture2D texture, Rectangle rectangle, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                colorTL, colorTR, colorBL, colorBR, pos, rot, sca, anchor, pivot, depth);

        public void WorldRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color color,
            RenderObject transform) =>
            WorldRender(texture, viewStart, viewEnd, color, color, color, color,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void WorldRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color color,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(texture, viewStart, viewEnd, color, color, color, color,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void WorldRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color color,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(texture, viewStart, viewEnd, color, color, color, color,
                pos, rot, sca, anchor, pivot, depth);

        public void WorldRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            RenderObject transform) =>
            WorldRender(texture, viewStart, viewEnd, colorTL, colorTR, colorBL, colorBR,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void WorldRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(texture, viewStart, viewEnd, colorTL, colorTR, colorBL, colorBR,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        #endregion

        #region Camera Render Methods
        public void CameraRender(TextureView2D view, RenderObject transform) =>
            CameraRender(view.Texture, view.ViewStart, view.ViewEnd, view.ColorTL, view.ColorTR, view.ColorBL, view.ColorBR,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void CameraRender(TextureView2D view, TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(view.Texture, view.ViewStart, view.ViewEnd, view.ColorTL, view.ColorTR, view.ColorBL, view.ColorBR,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void CameraRender(TextureView2D view, Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(view.Texture, view.ViewStart, view.ViewEnd, view.ColorTL, view.ColorTR, view.ColorBL, view.ColorBR,
                pos, rot, sca, anchor, pivot, depth);

        public void CameraRender(Texture2D texture, Rectangle rectangle, Color color,
            RenderObject transform) =>
            CameraRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                color, color, color, color, transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void CameraRender(Texture2D texture, Rectangle rectangle, Color color,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                color, color, color, color, transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void CameraRender(Texture2D texture, Rectangle rectangle, Color color,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                color, color, color, color, pos, rot, sca, anchor, pivot, depth);

        public void CameraRender(Texture2D texture, Rectangle rectangle, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            RenderObject transform) =>
            CameraRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                colorTL, colorTR, colorBL, colorBR, transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void CameraRender(Texture2D texture, Rectangle rectangle, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                colorTL, colorTR, colorBL, colorBR, transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void CameraRender(Texture2D texture, Rectangle rectangle, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                colorTL, colorTR, colorBL, colorBR, pos, rot, sca, anchor, pivot, depth);

        public void CameraRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color color,
            RenderObject transform) =>
            CameraRender(texture, viewStart, viewEnd, color, color, color, color,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void CameraRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color color,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(texture, viewStart, viewEnd, color, color, color, color,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void CameraRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color color,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(texture, viewStart, viewEnd, color, color, color, color,
                pos, rot, sca, anchor, pivot, depth);

        public void CameraRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            RenderObject transform) =>
            CameraRender(texture, viewStart, viewEnd, colorTL, colorTR, colorBL, colorBR,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void CameraRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(texture, viewStart, viewEnd, colorTL, colorTR, colorBL, colorBR,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        #endregion

        public void WorldRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth)
        {
            var spriteBatchItem = CreateBatchItem();
            spriteBatchItem.Texture = texture;
            spriteBatchItem.SortKey = depth;

            CalculateWorldRectangle(_camera.GetAnchorPosWorld(anchor), pos * _posPixelScale, rot, sca * _pixelScale, pivot,
                out var TL, out var TR, out var BL, out var BR);

            if (sca.X < 0) (viewStart.X, viewEnd.X) = (viewEnd.X, viewStart.X);
            if (sca.Y < 0) (viewStart.Y, viewEnd.Y) = (viewEnd.Y, viewStart.Y);

            spriteBatchItem.vertexTL = new VertexPositionColorTexture(new Vector3(TL.X, TL.Y, depth), colorTL, viewStart);
            spriteBatchItem.vertexTR = new VertexPositionColorTexture(new Vector3(TR.X, TR.Y, depth), colorTR, new Vector2(viewEnd.X, viewStart.Y));
            spriteBatchItem.vertexBL = new VertexPositionColorTexture(new Vector3(BL.X, BL.Y, depth), colorBL, new Vector2(viewStart.X, viewEnd.Y));
            spriteBatchItem.vertexBR = new VertexPositionColorTexture(new Vector3(BR.X, BR.Y, depth), colorBR, viewEnd);
        }

        public void CameraRender(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth)
        {
            var spriteBatchItem = CreateBatchItem();
            spriteBatchItem.Texture = texture;
            spriteBatchItem.SortKey = depth;

            CalculateCameraRectangle(_camera.GetAnchorPosCamera(anchor), pos * _posPixelScale, rot, sca * _pixelScale, pivot, 
                out var TL, out var TR, out var BL, out var BR);

            if (sca.X < 0) (viewStart.X, viewEnd.X) = (viewEnd.X, viewStart.X);
            if (sca.Y < 0) (viewStart.Y, viewEnd.Y) = (viewEnd.Y, viewStart.Y);

            spriteBatchItem.vertexTL = new VertexPositionColorTexture(new Vector3(TL.X, TL.Y, depth), colorTL, viewStart);
            spriteBatchItem.vertexTR = new VertexPositionColorTexture(new Vector3(TR.X, TR.Y, depth), colorTR, new Vector2(viewEnd.X, viewStart.Y));
            spriteBatchItem.vertexBL = new VertexPositionColorTexture(new Vector3(BL.X, BL.Y, depth), colorBL, new Vector2(viewStart.X, viewEnd.Y));
            spriteBatchItem.vertexBR = new VertexPositionColorTexture(new Vector3(BR.X, BR.Y, depth), colorBR, viewEnd);
        }

        private void CalculateWorldRectangle(Vector2 parentPos, Vector2 pos, float rot, Vector2 pixelSize, Vector2 pivot,
            out Vector2 TL, out Vector2 TR, out Vector2 BL, out Vector2 BR)
        {
            rot += _camera.Transform.Rot;
            var sin = MathF.Sin(rot * MathExtensions.Deg2Rad);
            var cos = MathF.Cos(rot * MathExtensions.Deg2Rad);

            pixelSize.GetBordersRectangleByPivot(pivot, out var TL2, out var BR2);

            if (pixelSize.X < 0) (TL2.X, BR2.X) = (BR2.X, TL2.X);
            if (pixelSize.Y < 0) (TL2.Y, BR2.Y) = (BR2.Y, TL2.Y);

            pos = parentPos + pos.RotateVector(sin, cos);
            TL = MathExtensions.RotateVector(TL2.X, TL2.Y, pos, sin, cos);
            TR = MathExtensions.RotateVector(BR2.X, TL2.Y, pos, sin, cos);
            BL = MathExtensions.RotateVector(TL2.X, BR2.Y, pos, sin, cos);
            BR = MathExtensions.RotateVector(BR2.X, BR2.Y, pos, sin, cos);
        }
        private void CalculateCameraRectangle(Vector2 parentPos, Vector2 pos, float rot, Vector2 pixelSize, Vector2 pivot, 
            out Vector2 TL, out Vector2 TR, out Vector2 BL, out Vector2 BR)
        {
            var sin = MathF.Sin(rot * MathExtensions.Deg2Rad);
            var cos = MathF.Cos(rot * MathExtensions.Deg2Rad);

            pixelSize.GetBordersRectangleByPivot(pivot, out var TL2, out var BR2);

            if (pixelSize.X < 0) (TL2.X, BR2.X) = (BR2.X, TL2.X);
            if (pixelSize.Y < 0) (TL2.Y, BR2.Y) = (BR2.Y, TL2.Y);

            pos = parentPos + pos;
            TL = MathExtensions.RotateVector(TL2.X, TL2.Y, pos, sin, cos);
            TR = MathExtensions.RotateVector(BR2.X, TL2.Y, pos, sin, cos);
            BL = MathExtensions.RotateVector(TL2.X, BR2.Y, pos, sin, cos);
            BR = MathExtensions.RotateVector(BR2.X, BR2.Y, pos, sin, cos);
        }
    }
}
