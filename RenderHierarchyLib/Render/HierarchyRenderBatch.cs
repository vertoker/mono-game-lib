using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib;
using RenderHierarchyLib.Core;
using RenderHierarchyLib.Graphics;
using RenderHierarchyLib.Models;
using RenderHierarchyLib.Models.Enum;
using RenderHierarchyLib.Models.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework.Graphics
{
    public class HierarchyRenderBatch : CustomGraphicsResource
    {
        private readonly HierarchySpriteBatcher _batcher;
        private readonly Camera _camera;

        private bool _beginCalled;

        private BlendState _blendState;
        private SamplerState _samplerState;
        private DepthStencilState _depthStencilState;
        private RasterizerState _rasterizerState;

        private readonly EffectPass _spritePass;
        private SpriteEffect _spriteEffect;

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

        public void RenderTest(Texture2D texture)
        {
            CheckValid(texture);
            HierarchySpriteBatchItem spriteBatchItem = _batcher.CreateBatchItem();
            spriteBatchItem.Texture = texture;
            spriteBatchItem.SortKey = 0;
            spriteBatchItem.vertexTL = new VertexPositionColorTexture(new Vector3(0, 0, 0), Color.White, new Vector2(0, 0));
            spriteBatchItem.vertexTR = new VertexPositionColorTexture(new Vector3(200, 0, 0), Color.White, new Vector2(1, 0));
            spriteBatchItem.vertexBL = new VertexPositionColorTexture(new Vector3(0, 200, 0), Color.White, new Vector2(0, 1));
            spriteBatchItem.vertexBR = new VertexPositionColorTexture(new Vector3(200, 200, 0), Color.White, new Vector2(1, 1));
            //spriteBatchItem.Set(25, 0, 150, 200, Color.White, Vector2.Zero, Vector2.One, 0);
        }
        
        public void Render(TextureView2D view, RenderTransformObject transform) => 
            Render(view.Texture, view.ViewStart, view.ViewEnd, transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Color);
        public void Render(TextureView2D view, TransformObject transform, Anchor anchor, Anchor pivot, Color color) =>
            Render(view.Texture, view.ViewStart, view.ViewEnd, transform.Pos, transform.Rot, transform.Sca, anchor, pivot, color);
        public void Render(TextureView2D view, Vector2 pos, float rot, Vector2 sca, Anchor anchor, Anchor pivot, Color color) =>
            Render(view.Texture, view.ViewStart, view.ViewEnd, pos, rot, sca, anchor, pivot, color);

        public void Render(Texture2D texture, Rectangle rectangle, RenderTransformObject transform) =>
            Render(texture, 
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width), 
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Color);
        public void Render(Texture2D texture, Rectangle rectangle, TransformObject transform, Anchor anchor, Anchor pivot, Color color) =>
            Render(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, color);
        public void Render(Texture2D texture, Rectangle rectangle, Vector2 pos, float rot, Vector2 sca, Anchor anchor, Anchor pivot, Color color) =>
            Render(texture,
                new Vector2(rectangle.Left / texture.Width, rectangle.Top / texture.Width),
                new Vector2(rectangle.Right / texture.Width, rectangle.Bottom / texture.Width),
                pos, rot, sca, anchor, pivot, color);

        public void Render(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, RenderTransformObject transform) =>
            Render(texture, viewStart, viewEnd, transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Color);
        public void Render(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, TransformObject transform, Anchor anchor, Anchor pivot, Color color) =>
            Render(texture, viewStart, viewEnd, transform.Pos, transform.Rot, transform.Sca, anchor, pivot, color);
        public void Render(Texture2D texture, Vector2 viewStart, Vector2 viewEnd, Vector2 pos, float rot, Vector2 sca, Anchor anchor, Anchor pivot, Color color)
        {

        }
    }
}
