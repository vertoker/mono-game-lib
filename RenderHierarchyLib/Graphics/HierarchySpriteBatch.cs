using Microsoft.Xna.Framework.Graphics;
using RenderHierarchyLib.Core;
using RenderHierarchyLib.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework.Graphics
{
    public class HierarchySpriteBatch : CustomGraphicsResource
    {
        //private SpriteBatch spriteBatch;

        private readonly HierarchySpriteBatcher _batcher;

        private SpriteSortMode _sortMode;

        private BlendState _blendState;

        private SamplerState _samplerState;

        private DepthStencilState _depthStencilState;

        private RasterizerState _rasterizerState;

        private Effect _effect;

        private bool _beginCalled;

        private SpriteEffect _spriteEffect;

        private readonly EffectPass _spritePass;

        private Rectangle _tempRect = new Rectangle(0, 0, 0, 0);

        private Vector2 _texCoordTL = new Vector2(0f, 0f);

        private Vector2 _texCoordBR = new Vector2(0f, 0f);

        public HierarchySpriteBatch(GraphicsDevice graphicsDevice)
            : this(graphicsDevice, 0)
        {
        }

        public HierarchySpriteBatch(GraphicsDevice graphicsDevice, int capacity)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice", "The GraphicsDevice must not be null when creating new resources.");
            }

            base.GraphicsDevice = graphicsDevice;
            _spriteEffect = new SpriteEffect(graphicsDevice);
            _spritePass = _spriteEffect.CurrentTechnique.Passes[0];
            _batcher = new HierarchySpriteBatcher(graphicsDevice, capacity);
            _beginCalled = false;
        }

        public void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            if (_beginCalled)
            {
                throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");
            }

            _sortMode = sortMode;
            _blendState = blendState ?? BlendState.AlphaBlend;
            _samplerState = samplerState ?? SamplerState.LinearClamp;
            _depthStencilState = depthStencilState ?? DepthStencilState.None;
            _rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;
            _effect = effect;
            _spriteEffect.TransformMatrix = transformMatrix;
            if (sortMode == SpriteSortMode.Immediate)
            {
                Setup();
            }

            _beginCalled = true;
        }

        public void End()
        {
            if (!_beginCalled)
            {
                throw new InvalidOperationException("Begin must be called before calling End.");
            }

            _beginCalled = false;
            if (_sortMode != SpriteSortMode.Immediate)
            {
                Setup();
            }

            _batcher.DrawBatch(_sortMode, _effect);
        }

        private void Setup()
        {
            GraphicsDevice obj = base.GraphicsDevice;
            obj.BlendState = _blendState;
            obj.DepthStencilState = _depthStencilState;
            obj.RasterizerState = _rasterizerState;
            obj.SamplerStates[0] = _samplerState;
            _spritePass.Apply();
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

        private void CheckValid(SpriteFont spriteFont, string text)
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

        private void CheckValid(SpriteFont spriteFont, StringBuilder text)
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

        public void Render(Texture2D texture)
        {
            CheckValid(texture);
            HierarchySpriteBatchItem spriteBatchItem = _batcher.CreateBatchItem();
            spriteBatchItem.Texture = texture;
            spriteBatchItem.SortKey = 0;
            spriteBatchItem.Set(0, 0, 200, 200, Color.White, Vector2.Zero, Vector2.One, 0);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            CheckValid(texture);
            HierarchySpriteBatchItem spriteBatchItem = _batcher.CreateBatchItem();
            spriteBatchItem.Texture = texture;
            switch (_sortMode)
            {
                case SpriteSortMode.Texture:
                    //spriteBatchItem.SortKey = texture.SortingKey;
                    break;
                case SpriteSortMode.FrontToBack:
                    spriteBatchItem.SortKey = layerDepth;
                    break;
                case SpriteSortMode.BackToFront:
                    spriteBatchItem.SortKey = 0f - layerDepth;
                    break;
            }

            origin *= scale;
            float w;
            float h;
            if (sourceRectangle.HasValue)
            {
                Rectangle valueOrDefault = sourceRectangle.GetValueOrDefault();
                w = (float)valueOrDefault.Width * scale.X;
                h = (float)valueOrDefault.Height * scale.Y;
                //_texCoordTL.X = (float)valueOrDefault.X * texture.TexelWidth;
                //_texCoordTL.Y = (float)valueOrDefault.Y * texture.TexelHeight;
                //_texCoordBR.X = (float)(valueOrDefault.X + valueOrDefault.Width) * texture.TexelWidth;
                //_texCoordBR.Y = (float)(valueOrDefault.Y + valueOrDefault.Height) * texture.TexelHeight;
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

            FlushIfNeeded();
        }

        internal void FlushIfNeeded()
        {
            if (_sortMode == SpriteSortMode.Immediate)
            {
                _batcher.DrawBatch(_sortMode, _effect);
            }
        }
    }
}
