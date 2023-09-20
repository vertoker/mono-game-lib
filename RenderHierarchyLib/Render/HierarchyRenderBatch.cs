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

        private bool _beginCalled = false;

        private Vector2 _texCoordTL = new Vector2(0f, 0f);
        private Vector2 _texCoordBR = new Vector2(0f, 0f);

        public HierarchyRenderBatch(GraphicsDevice graphicsDevice, Camera camera, int capacity = 0)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("graphicsDevice", "The GraphicsDevice must not be null when creating new resources.");

            GraphicsDevice = graphicsDevice;

            _batcher = new HierarchySpriteBatcher(graphicsDevice, capacity);
            _camera = camera;

            _beginCalled = false;
        }

        public void Begin(BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null)
        {
            if (_beginCalled)
                throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");
            
            var device = GraphicsDevice;
            device.BlendState = blendState ?? BlendState.AlphaBlend;
            device.SamplerStates[0] = samplerState ?? SamplerState.LinearClamp;
            device.DepthStencilState = depthStencilState ?? DepthStencilState.None;
            device.RasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
            
            _beginCalled = true;
        }
        public void End()
        {
            if (!_beginCalled)
                throw new InvalidOperationException("Begin must be called before calling End.");

            _batcher.DrawBatch(SpriteSortMode.Texture, null);
            _beginCalled = false;
        }

        private void CheckValid(Texture2D texture)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }

            if (!_beginCalled)
            {
                throw new InvalidOperationException("Draw was called, but Begin has not yet been called. Begin must be called successfully before you can call Draw.");
            }
        }

        public HierarchySpriteBatchItem CreateBatchItem()
        {
            return _batcher.CreateBatchItem();
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
            spriteBatchItem.vertexBR = new VertexPositionColorTexture(new Vector3(200, 200, 0), Color.Black, new Vector2(1, 1));
            //spriteBatchItem.Set(25, 0, 150, 200, Color.White, Vector2.Zero, Vector2.One, 0);
        }

        
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            CheckValid(texture);
            var spriteBatchItem = _batcher.CreateBatchItem();
            spriteBatchItem.Texture = texture;
            spriteBatchItem.SortKey = 0;

            origin *= scale;
            float w;
            float h;
            if (sourceRectangle.HasValue)
            {
                Rectangle valueOrDefault = sourceRectangle.GetValueOrDefault();
                w = (float)valueOrDefault.Width * scale.X;
                h = (float)valueOrDefault.Height * scale.Y;
                /*_texCoordTL.X = (float)valueOrDefault.X * texture.TexelWidth;
                _texCoordTL.Y = (float)valueOrDefault.Y * texture.TexelHeight;
                _texCoordBR.X = (float)(valueOrDefault.X + valueOrDefault.Width) * texture.TexelWidth;
                _texCoordBR.Y = (float)(valueOrDefault.Y + valueOrDefault.Height) * texture.TexelHeight;*/
            }
            else
            {
                w = (float)texture.Width * scale.X;
                h = (float)texture.Height * scale.Y;
                _texCoordTL = Vector2.Zero;
                _texCoordBR = Vector2.One;
            }

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
                spriteBatchItem.Set(position.X - origin.X, position.Y - origin.Y, w, h, color, _texCoordTL, _texCoordBR, layerDepth);
            }
            else
            {
                spriteBatchItem.Set(position.X, position.Y, 0f - origin.X, 0f - origin.Y, w, h, MathF.Sin(rotation), MathF.Cos(rotation), color, _texCoordTL, _texCoordBR, layerDepth);
            }
        }

        /*public void Render(TextureView2D view, RenderTransformObject transform) => 
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

        }*/
    }
}
