using RenderHierarchyLib;
using RenderHierarchyLib.Core;
using RenderHierarchyLib.Extensions;
using RenderHierarchyLib.Graphics;
using RenderHierarchyLib.Models;
using RenderHierarchyLib.Models.Transform;
using System;

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
        private Vector2 _screenSize;

        private Vector2 _posPixelScale;
        private Vector2 _screenCenter;
        private Vector2 _screenAnchorOffset;

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
            _screenSize = new(_camera.ScreenX, _camera.ScreenY);
            _screenCenter = _screenSize / 2f;
            _screenAnchorOffset = new(_screenCenter.X, -_screenCenter.Y);

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

        public HierarchySpriteBatchItem CreateBatchItem() => _batcher.CreateBatchItem();

        public void RenderTest(Texture2D texture, float angle = 15)
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

            spriteBatchItem.vertexTL = new VertexPositionColorTexture(baseVector + scale1 * vector, Color.White, mark1 * vector2);
            spriteBatchItem.vertexTR = new VertexPositionColorTexture(baseVector + scale2 * vector, Color.White, mark2 * vector2);
            spriteBatchItem.vertexBL = new VertexPositionColorTexture(baseVector + scale3 * vector, Color.White, mark3 * vector2);
            spriteBatchItem.vertexBR = new VertexPositionColorTexture(baseVector + scale4 * vector, Color.White, mark4 * vector2);
            //spriteBatchItem.Set(25, 0, 150, 200, Color.White, Vector2.Zero, Vector2.One, 0);
        }

        #region Render Methods
        public void Render(TextureView2D view, RenderObject transform) => 
            Render(view.Texture, view.ViewStart, view.ViewEnd, view.ColorTL, view.ColorTR, view.ColorBL, view.ColorBR,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void Render(TextureView2D view, TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            Render(view.Texture, view.ViewStart, view.ViewEnd, view.ColorTL, view.ColorTR, view.ColorBL, view.ColorBR,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void Render(TextureView2D view, Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            Render(view.Texture, view.ViewStart, view.ViewEnd, view.ColorTL, view.ColorTR, view.ColorBL, view.ColorBR,
                pos, rot, sca, anchor, pivot, depth);

        public void Render(Texture2D texture, Rectangle rectangle, Color color,
            RenderObject transform) =>
            Render(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                color, color, color, color, transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void Render(Texture2D texture, Rectangle rectangle, Color color,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            Render(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                color, color, color, color, transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void Render(Texture2D texture, Rectangle rectangle, Color color,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            Render(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                color, color, color, color, pos, rot, sca, anchor, pivot, depth);

        public void Render(Texture2D texture, Rectangle rectangle, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            RenderObject transform) =>
            Render(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                colorTL, colorTR, colorBL, colorBR, transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void Render(Texture2D texture, Rectangle rectangle, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            Render(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                colorTL, colorTR, colorBL, colorBR, transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void Render(Texture2D texture, Rectangle rectangle, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            Render(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                colorTL, colorTR, colorBL, colorBR, pos, rot, sca, anchor, pivot, depth);

        public void Render(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color color,
            RenderObject transform) =>
            Render(texture, viewStart, viewEnd, color, color, color, color,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void Render(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color color,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            Render(texture, viewStart, viewEnd, color, color, color, color,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        public void Render(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color color,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth) =>
            Render(texture, viewStart, viewEnd, color, color, color, color,
                pos, rot, sca, anchor, pivot, depth);

        public void Render(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            RenderObject transform) =>
            Render(texture, viewStart, viewEnd, colorTL, colorTR, colorBL, colorBR,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth);
        public void Render(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            TransformObject transform, Vector2 anchor, Vector2 pivot, int depth) =>
            Render(texture, viewStart, viewEnd, colorTL, colorTR, colorBL, colorBR,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth);
        #endregion

        public void Render(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Color colorTL, Color colorTR, Color colorBL, Color colorBR,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth)
        {
            var spriteBatchItem = CreateBatchItem();
            spriteBatchItem.Texture = texture;
            spriteBatchItem.SortKey = depth;

            var anchorPos = _screenCenter + anchor * _screenAnchorOffset;
            var pixelsSca = sca * _pixelScale;
            CalculateRectangle(anchorPos + pos * _posPixelScale, rot, pixelsSca, pivot, 
                out var TL, out var TR, out var BL, out var BR);

            spriteBatchItem.vertexTL = new VertexPositionColorTexture(new Vector3(TL.X, TL.Y, depth), colorTL, viewStart);
            spriteBatchItem.vertexTR = new VertexPositionColorTexture(new Vector3(TR.X, TR.Y, depth), colorTR, new Vector2(viewEnd.X, viewStart.Y));
            spriteBatchItem.vertexBL = new VertexPositionColorTexture(new Vector3(BL.X, BL.Y, depth), colorBL, new Vector2(viewStart.X, viewEnd.Y));
            spriteBatchItem.vertexBR = new VertexPositionColorTexture(new Vector3(BR.X, BR.Y, depth), colorBR, viewEnd);
        }

        private void CalculateRectangle(Vector2 pos, float rot, Vector2 pixelSize, Vector2 pivot, out Vector2 TL, out Vector2 TR, out Vector2 BL, out Vector2 BR)
        {
            var sin = MathF.Sin(rot * MathExtensions.Deg2Rad);
            var cos = MathF.Cos(rot * MathExtensions.Deg2Rad);

            pixelSize.GetBordersRectangleByPivot(pivot, out var TL2, out var BR2);

            TL = MathExtensions.RotateVector(TL2.X, TL2.Y, pos, sin, cos);
            TR = MathExtensions.RotateVector(BR2.X, TL2.Y, pos, sin, cos);
            BL = MathExtensions.RotateVector(TL2.X, BR2.Y, pos, sin, cos);
            BR = MathExtensions.RotateVector(BR2.X, BR2.Y, pos, sin, cos);

            /*
            TL = new Vector2(pos.X + TL2.X * cos - TL2.Y * sin, pos.Y + TL2.X * sin + TL2.Y * cos);
            TR = new Vector2(pos.X + BR2.X * cos - TL2.Y * sin, pos.Y + BR2.X * sin + TL2.Y * cos);
            BL = new Vector2(pos.X + TL2.X * cos - BR2.Y * sin, pos.Y + TL2.X * sin + BR2.Y * cos);
            BR = new Vector2(pos.X + BR2.X * cos - BR2.Y * sin, pos.Y + BR2.X * sin + BR2.Y * cos);
            */
        }
    }
}
