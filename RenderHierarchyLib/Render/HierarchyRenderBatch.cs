using RenderHierarchyLib;
using RenderHierarchyLib.Extensions;
using RenderHierarchyLib.Models;
using RenderHierarchyLib.Models.Transform;
using RenderHierarchyLib.Render.Sprite;
using RenderHierarchyLib.Models.Text;
using System.Collections.Generic;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using RenderHierarchyLib.Extensions.MonoGame;
using System.Diagnostics;

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

        private readonly UnsafeList<int> _glyphIndexes;
        private readonly UnsafeList<Vector2> _lineOrigins;

        private bool _beginCalled;
        private float _pixelScale;
        private Vector2 _posPixelScale;

        public HierarchyRenderBatch(GraphicsDevice graphicsDevice, Camera camera, int spriteCapacity = 256, int glyphIndexesCapacity = 16, int lineOriginsCapacity = 4)
        {
            GraphicsDevice = graphicsDevice ?? 
                throw new ArgumentNullException("graphicsDevice", "The GraphicsDevice must not be null when creating new resources.");
            _camera = camera;

            _spriteEffect = new SpriteEffect(graphicsDevice);
            _spritePass = _spriteEffect.CurrentTechnique.Passes[0];
            _batcher = new HierarchySpriteBatcher(graphicsDevice, spriteCapacity);
            _glyphIndexes = new UnsafeList<int>(glyphIndexesCapacity);
            _lineOrigins = new UnsafeList<Vector2>(lineOriginsCapacity);
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

        private bool CheckErrorSprite(Texture2D texture)
        {
            if (texture == null) return true;
            if (!_beginCalled) return true;
            return false;
        }

        private bool CheckErrorText(CustomSpriteFont spriteFont, string text)
        {
            if (spriteFont == null) return true;
            if (text == null) return true;
            if (!_beginCalled) return true;
            return false;
        }

        public HierarchySpriteBatchItem CreateBatchItem() => _batcher.CreateBatchItem();

        #region Tests
        public void RenderTextTest(CustomSpriteFont font, string text)
        {
            if (CheckErrorText(font, text)) return;
            font.SetGlyphIndexes(ref text, _glyphIndexes, out var lines);
            //RenderTest(font.Texture);
        }

        public void RenderTest(Texture2D texture, float angle = 0)
        {
            if (CheckErrorSprite(texture)) return;

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


        private Vector2 _texCoordTL;
        private Vector2 _texCoordBR;
        public unsafe void DrawString(CustomSpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            if (CheckErrorText(spriteFont, text)) return;

            Vector2 zero = Vector2.Zero;
            bool flagVertical = (effects & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically;
            bool flagHorizontal = (effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally;
            /*
            if (flagVertical || flagHorizontal)
            {
                spriteFont.MeasureString(ref text, out var size);
                if (flagHorizontal)
                {
                    origin.X *= -1f;
                    zero.X = 0f - size.X;
                }

                if (flagVertical)
                {
                    origin.Y *= -1f;
                    zero.Y = (float)spriteFont.HeightSpacing - size.Y;
                }
            }
            */
            
            Matrix matrix = Matrix.Identity;
            float sin = 0f;
            float cos = 0f;
            if (rotation == 0f)
            {
                matrix.M11 = (flagHorizontal ? (0f - scale.X) : scale.X);
                matrix.M22 = (flagVertical ? (0f - scale.Y) : scale.Y);
                matrix.M41 = (zero.X - origin.X) * matrix.M11 + position.X;
                matrix.M42 = (zero.Y - origin.Y) * matrix.M22 + position.Y;
            }
            else
            {
                sin = MathF.Cos(rotation);
                cos = MathF.Sin(rotation);
                matrix.M11 = (flagHorizontal ? (0f - scale.X) : scale.X) * sin;
                matrix.M12 = (flagHorizontal ? (0f - scale.X) : scale.X) * cos;
                matrix.M21 = (flagVertical ? (0f - scale.Y) : scale.Y) * (0f - cos);
                matrix.M22 = (flagVertical ? (0f - scale.Y) : scale.Y) * sin;
                matrix.M41 = (zero.X - origin.X) * matrix.M11 + (zero.Y - origin.Y) * matrix.M21 + position.X;
                matrix.M42 = (zero.X - origin.X) * matrix.M12 + (zero.Y - origin.Y) * matrix.M22 + position.Y;
            }
            
            Vector2 zero2 = Vector2.Zero;
            bool flag3 = true;
            fixed (CustomSpriteFont.Glyph* ptr = spriteFont.Glyphs)
            {
                foreach (char c in text)
                {
                    switch (c)
                    {
                        case '\n':
                            zero2.X = 0f;
                            zero2.Y += spriteFont.HeightSpacing;
                            flag3 = true;
                            continue;
                        case '\r':
                            continue;
                    }

                    int glyphIndexOrDefault = spriteFont.GetGlyphIndexOrDefault(c);
                    var ptr2 = ptr + glyphIndexOrDefault;
                    if (flag3)
                    {
                        zero2.X = Math.Max(ptr2->LeftBearing, 0f);
                        flag3 = false;
                    }
                    else
                    {
                        zero2.X += spriteFont.WidthSpacing + ptr2->LeftBearing;
                    }

                    Vector2 position2 = zero2;
                    if (flagHorizontal)
                    {
                        position2.X += ptr2->BoundsInTexture.Width;
                    }

                    position2.X += ptr2->Cropping.X;
                    if (flagVertical)
                    {
                        position2.Y += ptr2->BoundsInTexture.Height - spriteFont.WidthSpacing;
                    }

                    position2.Y += ptr2->Cropping.Y;
                    Vector2.Transform(ref position2, ref matrix, out position2);
                    var spriteBatchItem = CreateBatchItem();
                    spriteBatchItem.Texture = spriteFont.Texture;
                    spriteBatchItem.SortKey = layerDepth;
                    _texCoordTL.X = (float)ptr2->BoundsInTexture.X * spriteFont.TextureTexel.X;
                    _texCoordTL.Y = (float)ptr2->BoundsInTexture.Y * spriteFont.TextureTexel.Y;
                    _texCoordBR.X = (float)(ptr2->BoundsInTexture.X + ptr2->BoundsInTexture.Width) * spriteFont.TextureTexel.X;
                    _texCoordBR.Y = (float)(ptr2->BoundsInTexture.Y + ptr2->BoundsInTexture.Height) * spriteFont.TextureTexel.Y;
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
                        spriteBatchItem.Set(position2.X, position2.Y, 0f, 0f, (float)ptr2->BoundsInTexture.Width * scale.X, (float)ptr2->BoundsInTexture.Height * scale.Y, cos, sin, color, _texCoordTL, _texCoordBR, layerDepth);
                    }

                    zero2.X += ptr2->Width + ptr2->RightBearing;
                }
            }
            
        }

        public unsafe void CameraTextRender(CustomSpriteFont font, string text, Color defaultColor, List<TextColorIndexes> colors,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth, TextAlignmentHorizontal alignment)
        {
            if (CheckErrorText(font, text)) return;
            font.SetGlyphIndexes(ref text, _glyphIndexes, out var lines);
            _lineOrigins.EnsureCapacity(lines);

            var sin = MathF.Sin(rot * MathExtensions.Deg2Rad);
            var cos = MathF.Cos(rot * MathExtensions.Deg2Rad);

            fixed (int* ptrGlyphIndex = _glyphIndexes.Items)
            {
                fixed (CustomSpriteFont.Glyph* ptrGlyph = font.Glyphs)
                {
                    fixed (Vector2* ptrLineOrigin = _lineOrigins.Items)
                    {
                        fixed (char* ptrText = text)
                        {
                            CalculateTextVectors(font, _glyphIndexes.Size, ptrGlyphIndex, ptrGlyph, ptrText, ptrLineOrigin,
                                _camera.GetAnchorPosCamera(anchor), pos * _posPixelScale, sin, cos, sca, pivot,
                                out var dirRight, out var dirDown);

                            var flagLines = true;
                            var counterLines = 0;
                            var charOrigin = new Vector3(ptrLineOrigin[counterLines], depth);
                            for (int i = 0; i < _glyphIndexes.Size; i++)
                            {
                                if (ptrText[i] == '\n')
                                {
                                    flagLines = true;
                                    counterLines++;
                                    charOrigin = new Vector3(ptrLineOrigin[counterLines], depth);
                                    continue;
                                }
                                else if (ptrText[i] == '\r')
                                {
                                    continue;
                                }

                                var glyph = ptrGlyph[ptrGlyphIndex[i]];
                                var item = CreateBatchItem();
                                var rect = glyph.BoundsInTexture;

                                item.Texture = font.Texture;
                                item.SortKey = depth;

                                var downHeight = dirDown * glyph.Cropping.Height;
                                charOrigin = charOrigin.Plus(dirRight * glyph.LeftBearing);

                                item.vertexTL.Setup(charOrigin, defaultColor, rect.TL() * font.TextureTexel);
                                item.vertexBL.Setup(charOrigin.Plus(downHeight), defaultColor, rect.BL() * font.TextureTexel);

                                charOrigin = charOrigin.Plus(dirRight * glyph.Width);
                                item.vertexTR.Setup(charOrigin, defaultColor, rect.TR() * font.TextureTexel);
                                item.vertexBR.Setup(charOrigin.Plus(downHeight), defaultColor, rect.BR() * font.TextureTexel);

                                Debug.WriteLine(item.vertexTL);
                                Debug.WriteLine(item.vertexBL);
                                Debug.WriteLine(item.vertexTR);
                                Debug.WriteLine(item.vertexBR);

                                charOrigin = charOrigin.Plus(dirRight * (glyph.RightBearing + font.WidthSpacing));
                            }
                        }
                    }
                }
            }
        }
         
        private static unsafe void CalculateTextVectors(CustomSpriteFont font, int textLength,// int lineCount,
            int* ptrGlyphIndex, CustomSpriteFont.Glyph* ptrGlyph, char* ptrText, Vector2* ptrLineOrigin,
            Vector2 parentPos, Vector2 pos, float sin, float cos, Vector2 sca, Vector2 pivot, out Vector2 dirRight, out Vector2 dirUp)
        {
            dirRight = (sca.X < 0 ? -Vector2.UnitX : Vector2.UnitX).RotateVector(sin, cos);
            dirUp = (sca.Y < 0 ? -Vector2.UnitY : Vector2.UnitY).RotateVector(sin, cos);
            var dirDown = -dirUp;

            var flagLines = true;
            var textSize = Vector2.Zero;
            var counterLines = 0;
            ptrLineOrigin[0] = Vector2.Zero;

            for (int i = 0; i < textLength; i++)
            {
                switch (ptrText[i])
                {
                    case '\n':
                        counterLines++;
                        flagLines = true;
                        textSize.Y += font.HeightSpacing;
                        ptrLineOrigin[counterLines] = textSize;
                        continue;
                    case '\r':
                        continue;
                }

                if (flagLines) flagLines = false;
                else ptrLineOrigin[counterLines].X += font.WidthSpacing;

                var glyph = ptrGlyph[ptrGlyphIndex[i]];
                ptrLineOrigin[counterLines].X += glyph.WidthIncludingBearings;

                if (glyph.Cropping.Height > ptrLineOrigin[counterLines].Y)
                    ptrLineOrigin[counterLines].Y = glyph.Cropping.Height;
                if (ptrLineOrigin[counterLines].X > textSize.X)
                    textSize.X = ptrLineOrigin[counterLines].X;
            }

            // convert size to points
            // later with angle and scale

            //size.X = sizeX;
            //size.Y = zero.Y + num2;
        }


        #region World Render Methods
        public void WorldRender(TextureView view, RenderObject transform) =>
            WorldRender(view.Texture, view.Color, view.ViewStart, view.ViewEnd,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void WorldRender(TextureView view, TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(view.Texture, view.Color, view.ViewStart, view.ViewEnd,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void WorldRender(TextureView view, Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(view.Texture, view.Color, view.ViewStart, view.ViewEnd,
                pos, rot, sca, anchor, pivot, depth);

        public void WorldRender(Texture2D texture, Rectangle rectangle, Color color,
            RenderObject transform) =>
            WorldRender(texture, color,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void WorldRender(Texture2D texture, Rectangle rectangle, Color color,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(texture, color,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void WorldRender(Texture2D texture, Rectangle rectangle, Color color,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(texture, color,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                pos, rot, sca, anchor, pivot, depth);

        public void WorldRender(Texture2D texture, Color color, Vector2 viewStart, Vector2 viewEnd,
            RenderObject transform) =>
            WorldRender(texture, color, viewStart, viewEnd,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void WorldRender(Texture2D texture, Color color, Vector2 viewStart, Vector2 viewEnd,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            WorldRender(texture, color, viewStart, viewEnd,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        #endregion

        #region Camera Render Methods
        public void CameraRender(TextureView view, RenderObject transform) =>
            CameraRender(view.Texture, view.Color, view.ViewStart, view.ViewEnd,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void CameraRender(TextureView view, TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(view.Texture, view.Color, view.ViewStart, view.ViewEnd,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void CameraRender(TextureView view, Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(view.Texture, view.Color, view.ViewStart, view.ViewEnd,
                pos, rot, sca, anchor, pivot, depth);

        public void CameraRender(Texture2D texture, Color color, Rectangle rectangle,
            RenderObject transform) =>
            CameraRender(texture, color,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void CameraRender(Texture2D texture, Color color, Rectangle rectangle,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(texture, color,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void CameraRender(Texture2D texture, Color color, Rectangle rectangle,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(texture, color,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                pos, rot, sca, anchor, pivot, depth);

        public void CameraRender(Texture2D texture, Color color, Vector2 viewStart, Vector2 viewEnd,
            RenderObject transform) =>
            CameraRender(texture, color, viewStart, viewEnd,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void CameraRender(Texture2D texture, Color color, Vector2 viewStart, Vector2 viewEnd,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            CameraRender(texture, color, viewStart, viewEnd,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        #endregion

        public void WorldRender(Texture2D texture, Color color, Vector2 viewStart, Vector2 viewEnd,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth)
        {
            if (CheckErrorSprite(texture)) return;

            var spriteBatchItem = CreateBatchItem();
            spriteBatchItem.Texture = texture;
            spriteBatchItem.SortKey = depth;

            CalculateVertexes(_camera.GetAnchorPosWorld(anchor), pos * _posPixelScale, rot + _camera.Transform.Rot,
                sca * _pixelScale, pivot, out var TL, out var TR, out var BL, out var BR);

            if (sca.X < 0) (viewStart.X, viewEnd.X) = (viewEnd.X, viewStart.X);
            if (sca.Y < 0) (viewStart.Y, viewEnd.Y) = (viewEnd.Y, viewStart.Y);

            spriteBatchItem.vertexTL.Setup(new Vector3(TL.X, TL.Y, depth), color, viewStart);
            spriteBatchItem.vertexTR.Setup(new Vector3(TR.X, TR.Y, depth), color, new Vector2(viewEnd.X, viewStart.Y));
            spriteBatchItem.vertexBL.Setup(new Vector3(BL.X, BL.Y, depth), color, new Vector2(viewStart.X, viewEnd.Y));
            spriteBatchItem.vertexBR.Setup(new Vector3(BR.X, BR.Y, depth), color, viewEnd);
        }

        public void CameraRender(Texture2D texture, Color color, Vector2 viewStart, Vector2 viewEnd,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth)
        {
            if (CheckErrorSprite(texture)) return;

            var spriteBatchItem = CreateBatchItem();
            spriteBatchItem.Texture = texture;
            spriteBatchItem.SortKey = depth;

            CalculateVertexes(_camera.GetAnchorPosCamera(anchor), pos * _posPixelScale, rot,
                sca * _pixelScale, pivot, out var TL, out var TR, out var BL, out var BR);

            if (sca.X < 0) (viewStart.X, viewEnd.X) = (viewEnd.X, viewStart.X);
            if (sca.Y < 0) (viewStart.Y, viewEnd.Y) = (viewEnd.Y, viewStart.Y);

            spriteBatchItem.vertexTL.Setup(new Vector3(TL.X, TL.Y, depth), color, viewStart);
            spriteBatchItem.vertexTR.Setup(new Vector3(TR.X, TR.Y, depth), color, new Vector2(viewEnd.X, viewStart.Y));
            spriteBatchItem.vertexBL.Setup(new Vector3(BL.X, BL.Y, depth), color, new Vector2(viewStart.X, viewEnd.Y));
            spriteBatchItem.vertexBR.Setup(new Vector3(BR.X, BR.Y, depth), color, viewEnd);
        }

        private static void CalculateVertexes(Vector2 parentPos, Vector2 pos, float rot, Vector2 pixelSize, Vector2 pivot,
            out Vector2 TL, out Vector2 TR, out Vector2 BL, out Vector2 BR)
        {
            pixelSize.GetBordersRectangleByPivot(pivot, out TL, out BR);

            if (pixelSize.X < 0) (TL.X, BR.X) = (BR.X, TL.X);
            if (pixelSize.Y < 0) (TL.Y, BR.Y) = (BR.Y, TL.Y);

            if (rot == 0)
            {
                TR.X = BR.X; TR.Y = TL.Y;
                BL.X = TL.X; BL.Y = BR.Y;
                return;
            }

            var sin = MathF.Sin(rot * MathExtensions.Deg2Rad);
            var cos = MathF.Cos(rot * MathExtensions.Deg2Rad);

            pos = parentPos + pos;
            TR = MathExtensions.RotateVector(BR.X, TL.Y, pos, sin, cos);
            BL = MathExtensions.RotateVector(TL.X, BR.Y, pos, sin, cos);
            TL = TL.RotateVector(pos, sin, cos);
            BR = BR.RotateVector(pos, sin, cos);
        }

        /*private static void CalculateTextVertexes(Vector2 parentPos, Vector2 pos, float rot, Vector2 pixelSize, Vector2 pivot,
            out Vector2 origin, out Vector2 dirRight, out Vector2 dirDown)
        {
            pixelSize.GetBordersRectangleByPivot(pivot, out TL, out BR);

            if (pixelSize.X < 0) (TL.X, BR.X) = (BR.X, TL.X);
            if (pixelSize.Y < 0) (TL.Y, BR.Y) = (BR.Y, TL.Y);

            if (rot == 0)
            {
                TR.X = BR.X; TR.Y = TL.Y;
                BL.X = TL.X; BL.Y = BR.Y;
                return;
            }

            var sin = MathF.Sin(rot * MathExtensions.Deg2Rad);
            var cos = MathF.Cos(rot * MathExtensions.Deg2Rad);

            pos = parentPos + pos;
            TR = MathExtensions.RotateVector(BR.X, TL.Y, pos, sin, cos);
            BL = MathExtensions.RotateVector(TL.X, BR.Y, pos, sin, cos);
            TL = TL.RotateVector(pos, sin, cos);
            BR = BR.RotateVector(pos, sin, cos);
        }*/
    }
}
