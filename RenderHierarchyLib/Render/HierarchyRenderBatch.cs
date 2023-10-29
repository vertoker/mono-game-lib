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
using System.Net.Http.Headers;
using RenderHierarchyLib.Render;

namespace Microsoft.Xna.Framework.Graphics
{
    public class HierarchyRenderBatch : CustomGraphicsResource
    {
        public readonly HierarchySpriteBatcher Batcher;
        public readonly Camera Camera;

        private BlendState _blendState;
        private SamplerState _samplerState;
        private DepthStencilState _depthStencilState;
        private RasterizerState _rasterizerState;

        private readonly EffectPass _spritePass;
        private SpriteEffect _spriteEffect;

        private readonly UnsafeList<int> _glyphIndexes;
        private readonly UnsafeList<Vector2> _lineOrigins;

        private bool _beginCalled;
        private bool _autoReloadBatching = true;
        private int _batchCounter = 0;
        private int _batchMaxSize = 5000;
        private float _zPosition = 0f;

        private float _pixelScale;
        private Vector2 _posPixelScale;

        public HierarchyRenderBatch(Camera camera, HierarchyRenderBatchPreset preset = null)
        {
            GraphicsDevice = camera.GraphicsManager.GraphicsDevice ?? 
                throw new ArgumentNullException("graphicsDevice", "The GraphicsDevice must not be null when creating new resources.");
            preset ??= HierarchyRenderBatchPreset.Default;
            Camera = camera;

            _autoReloadBatching = preset.autoReloadBatching;
            _batchMaxSize = preset.batchMaxSize;
            _zPosition = preset.zPosition;

            _spriteEffect = new SpriteEffect(GraphicsDevice);
            _spritePass = _spriteEffect.CurrentTechnique.Passes[0];
            Batcher = new HierarchySpriteBatcher(GraphicsDevice, preset.spriteCapacity);
            _glyphIndexes = new UnsafeList<int>(preset.glyphIndexesCapacity);
            _lineOrigins = new UnsafeList<Vector2>(preset.lineOriginsCapacity);
        }

        public void Begin(BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null)
        {
            if (_beginCalled)
                throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");

            _blendState = blendState ?? BlendState.AlphaBlend;
            _samplerState = samplerState ?? SamplerState.LinearClamp;
            _depthStencilState = depthStencilState ?? DepthStencilState.None;
            _rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;

            _pixelScale = Camera.PixelScale;
            _posPixelScale = new(_pixelScale, -_pixelScale);
            Camera.UpdateAnchors(_pixelScale);

            _batchCounter = 0;
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

            Batcher.DrawBatch();
            _beginCalled = false;
        }

        public void Reload()
        {
            if (!_beginCalled)
                throw new InvalidOperationException("Reload must be called before calling End.");
            End();
            Begin();
        }

        public void TryReload()
        {
            if (!_autoReloadBatching)
                return;
            _batchCounter++;
            if (_batchCounter >= _batchMaxSize)
                Reload();
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

        public HierarchySpriteBatchItem CreateBatchItem() => Batcher.CreateBatchItem();

        #region World Rich Text Render Methods
        public void WorldRichTextRender(CustomSpriteFont font, RichTextParser richText, RenderText transform) =>
            WorldRichTextRender(font, richText,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth, transform.Alignment);
        public void WorldRichTextRender(CustomSpriteFont font, RichTextParser richText, TransformObject transform,
            Vector2 anchor, Vector2 pivot, int depth, TextAlignmentHorizontal alignment) =>
            WorldRichTextRender(font, richText,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth, alignment);
        #endregion

        #region World Text Render Methods
        public void WorldTextRender(CustomSpriteFont font, string text, Color color, RenderText transform) =>
            WorldTextRender(font, text, color,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth, transform.Alignment);
        public void WorldTextRender(CustomSpriteFont font, string text, Color color, TransformObject transform, 
            Vector2 anchor, Vector2 pivot, int depth, TextAlignmentHorizontal alignment) =>
            WorldTextRender(font, text, color,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth, alignment);
        #endregion

        #region Camera Rich Text Render Methods
        public void CameraRichTextRender(CustomSpriteFont font, RichTextParser richText, RenderText transform) =>
            CameraRichTextRender(font, richText,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth, transform.Alignment);
        public void CameraRichTextRender(CustomSpriteFont font, RichTextParser richText, TransformObject transform, 
            Vector2 anchor, Vector2 pivot, int depth, TextAlignmentHorizontal alignment) =>
            CameraRichTextRender(font, richText, transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth, alignment);
        #endregion

        #region Camera Text Render Methods
        public void CameraTextRender(CustomSpriteFont font, string text, Color color, RenderText transform) =>
            CameraTextRender(font, text, color,
                transform.Pos, transform.Rot, transform.Sca, transform.Anchor, transform.Pivot, transform.Depth, transform.Alignment);
        public void CameraTextRender(CustomSpriteFont font, string text, Color color, TransformObject transform, 
            Vector2 anchor, Vector2 pivot, int depth, TextAlignmentHorizontal alignment) =>
            CameraTextRender(font, text, color,
                transform.Pos, transform.Rot, transform.Sca, anchor, pivot, depth, alignment);
        #endregion

        public unsafe void WorldRichTextRender(CustomSpriteFont font, RichTextParser richText,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth, TextAlignmentHorizontal alignment)
        {
            if (CheckErrorText(font, richText.Text)) return;
            fixed (char* ptrText = richText.Text)
            {
                font.SetGlyphIndexes(ptrText, richText.Text.Length, _glyphIndexes, out var lines);

                _lineOrigins.EnsureCapacity(lines);

                rot -= Camera.Transform.Rot;
                var sin = -MathF.Sin(rot * MathExtensions.Deg2Rad);
                var cos = MathF.Cos(rot * MathExtensions.Deg2Rad);

                var flagNegativeX = sca.X < 0;
                var flagNegativeY = sca.Y < 0;

                fixed (int* ptrGlyphIndex = _glyphIndexes.Items)
                {
                    fixed (CustomSpriteFont.Glyph* ptrGlyph = font.Glyphs)
                    {
                        fixed (Vector2* ptrLineOrigin = _lineOrigins.Items)
                        {
                            CalculateTextVectors(font, _glyphIndexes.Size, lines,
                                ptrGlyphIndex, ptrGlyph, ptrText, ptrLineOrigin,
                                Camera.GetAnchorPosWorldInverse(anchor), pos * _posPixelScale, sin, cos, sca, pivot, alignment,
                                out var dirLeft, out var dirRight, out var dirUp, out var dirDown);

                            var flagLines = true;
                            var counterLines = 0;
                            var charOrigin = new Vector3(ptrLineOrigin[counterLines], _zPosition);
                            var currentColor = richText.DefaultColor;
                            for (int i = 0; i < _glyphIndexes.Size; i++)
                            {
                                if (ptrText[i] == '\n')
                                {
                                    flagLines = true;
                                    counterLines++;
                                    charOrigin = new Vector3(ptrLineOrigin[counterLines], _zPosition);
                                    continue;
                                }
                                else if (ptrText[i] == '\r')
                                {
                                    continue;
                                }

                                var glyph = ptrGlyph[ptrGlyphIndex[i]];
                                var item = CreateBatchItem();
                                var rect = glyph.BoundsInTexture;

                                var left = rect.Left * font.TextureTexel.X;
                                var right = rect.Right * font.TextureTexel.X;
                                var top = rect.Top * font.TextureTexel.Y;
                                var bottom = rect.Bottom * font.TextureTexel.Y;

                                item.Texture = font.Texture;
                                item.SortKey = depth;

                                if (flagLines) flagLines = false;
                                else charOrigin = charOrigin.Plus(dirRight * glyph.LeftBearing);

                                var upHeight = dirDown * (font.HeightSpacing - glyph.Cropping.Y);
                                var downHeight = dirUp * (glyph.BoundsInTexture.Height + glyph.Cropping.Y - font.HeightSpacing);

                                if (richText.Colors.TryGetValue(i, out var nextColor))
                                {
                                    currentColor = nextColor;
                                }
                                item.vertexTL.Setup(charOrigin.Plus(upHeight), currentColor, new Vector2(left, top));
                                item.vertexBL.Setup(charOrigin.Plus(downHeight), currentColor, new Vector2(left, bottom));

                                charOrigin = charOrigin.Plus(dirRight * rect.Width);
                                item.vertexTR.Setup(charOrigin.Plus(upHeight), currentColor, new Vector2(right, top));
                                item.vertexBR.Setup(charOrigin.Plus(downHeight), currentColor, new Vector2(right, bottom));

                                if (flagNegativeX ^ flagNegativeY)
                                {
                                    if (flagNegativeX)
                                    {
                                        (item.vertexTL, item.vertexTR) = (item.vertexTR, item.vertexTL);
                                        (item.vertexBL, item.vertexBR) = (item.vertexBR, item.vertexBL);
                                    }
                                    else
                                    {
                                        (item.vertexTL, item.vertexBL) = (item.vertexBL, item.vertexTL);
                                        (item.vertexTR, item.vertexBR) = (item.vertexBR, item.vertexTR);
                                    }
                                }

                                charOrigin = charOrigin.Plus(dirRight * (glyph.RightBearing + font.WidthSpacing));

                                TryReload();
                            }
                        }
                    }
                }
            }
        }

        public unsafe void WorldTextRender(CustomSpriteFont font, string text, Color color,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth, TextAlignmentHorizontal alignment)
        {
            if (CheckErrorText(font, text)) return;
            fixed (char* ptrText = text)
            {
                font.SetGlyphIndexes(ptrText, text.Length, _glyphIndexes, out var lines);
                _lineOrigins.EnsureCapacity(lines);

                rot -= Camera.Transform.Rot;
                var sin = -MathF.Sin(rot * MathExtensions.Deg2Rad);
                var cos = MathF.Cos(rot * MathExtensions.Deg2Rad);

                var flagNegativeX = sca.X < 0;
                var flagNegativeY = sca.Y < 0;

                fixed (int* ptrGlyphIndex = _glyphIndexes.Items)
                {
                    fixed (CustomSpriteFont.Glyph* ptrGlyph = font.Glyphs)
                    {
                        fixed (Vector2* ptrLineOrigin = _lineOrigins.Items)
                        {
                            CalculateTextVectors(font, _glyphIndexes.Size, lines,
                                ptrGlyphIndex, ptrGlyph, ptrText, ptrLineOrigin,
                                Camera.GetAnchorPosWorldInverse(anchor), pos * _posPixelScale, sin, cos, sca, pivot, alignment,
                                out var dirLeft, out var dirRight, out var dirUp, out var dirDown);

                            var flagLines = true;
                            var counterLines = 0;
                            var charOrigin = new Vector3(ptrLineOrigin[counterLines], _zPosition);
                            for (int i = 0; i < _glyphIndexes.Size; i++)
                            {
                                if (ptrText[i] == '\n')
                                {
                                    flagLines = true;
                                    counterLines++;
                                    charOrigin = new Vector3(ptrLineOrigin[counterLines], _zPosition);
                                    continue;
                                }
                                else if (ptrText[i] == '\r')
                                {
                                    continue;
                                }

                                var glyph = ptrGlyph[ptrGlyphIndex[i]];
                                var item = CreateBatchItem();
                                var rect = glyph.BoundsInTexture;

                                var left = rect.Left * font.TextureTexel.X;
                                var right = rect.Right * font.TextureTexel.X;
                                var top = rect.Top * font.TextureTexel.Y;
                                var bottom = rect.Bottom * font.TextureTexel.Y;

                                item.Texture = font.Texture;
                                item.SortKey = depth;

                                if (flagLines) flagLines = false;
                                else charOrigin = charOrigin.Plus(dirRight * glyph.LeftBearing);

                                var upHeight = dirDown * (font.HeightSpacing - glyph.Cropping.Y);
                                var downHeight = dirUp * (glyph.BoundsInTexture.Height + glyph.Cropping.Y - font.HeightSpacing);

                                item.vertexTL.Setup(charOrigin.Plus(upHeight), color, new Vector2(left, top));
                                item.vertexBL.Setup(charOrigin.Plus(downHeight), color, new Vector2(left, bottom));

                                charOrigin = charOrigin.Plus(dirRight * rect.Width);
                                item.vertexTR.Setup(charOrigin.Plus(upHeight), color, new Vector2(right, top));
                                item.vertexBR.Setup(charOrigin.Plus(downHeight), color, new Vector2(right, bottom));

                                if (flagNegativeX ^ flagNegativeY)
                                {
                                    if (flagNegativeX)
                                    {
                                        (item.vertexTL, item.vertexTR) = (item.vertexTR, item.vertexTL);
                                        (item.vertexBL, item.vertexBR) = (item.vertexBR, item.vertexBL);
                                    }
                                    else
                                    {
                                        (item.vertexTL, item.vertexBL) = (item.vertexBL, item.vertexTL);
                                        (item.vertexTR, item.vertexBR) = (item.vertexBR, item.vertexTR);
                                    }
                                }

                                charOrigin = charOrigin.Plus(dirRight * (glyph.RightBearing + font.WidthSpacing));

                                TryReload();
                            }
                        }
                    }
                }
            }
        }

        public unsafe void CameraRichTextRender(CustomSpriteFont font, RichTextParser richText,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth, TextAlignmentHorizontal alignment)
        {
            if (CheckErrorText(font, richText.Text)) return;
            fixed (char* ptrText = richText.Text)
            {
                font.SetGlyphIndexes(ptrText, richText.Text.Length, _glyphIndexes, out var lines);

                _lineOrigins.EnsureCapacity(lines);

                var sin = -MathF.Sin(rot * MathExtensions.Deg2Rad);
                var cos = MathF.Cos(rot * MathExtensions.Deg2Rad);

                var flagNegativeX = sca.X < 0;
                var flagNegativeY = sca.Y < 0;

                fixed (int* ptrGlyphIndex = _glyphIndexes.Items)
                {
                    fixed (CustomSpriteFont.Glyph* ptrGlyph = font.Glyphs)
                    {
                        fixed (Vector2* ptrLineOrigin = _lineOrigins.Items)
                        {
                            CalculateTextVectors(font, _glyphIndexes.Size, lines,
                                ptrGlyphIndex, ptrGlyph, ptrText, ptrLineOrigin,
                                Camera.GetAnchorPosCameraInverse(anchor), pos * _posPixelScale, sin, cos, sca, pivot, alignment,
                                out var dirLeft, out var dirRight, out var dirUp, out var dirDown);

                            var flagLines = true;
                            var counterLines = 0;
                            var charOrigin = new Vector3(ptrLineOrigin[counterLines], _zPosition);
                            var currentColor = richText.DefaultColor;
                            for (int i = 0; i < _glyphIndexes.Size; i++)
                            {
                                if (ptrText[i] == '\n')
                                {
                                    flagLines = true;
                                    counterLines++;
                                    charOrigin = new Vector3(ptrLineOrigin[counterLines], _zPosition);
                                    continue;
                                }
                                else if (ptrText[i] == '\r')
                                {
                                    continue;
                                }

                                var glyph = ptrGlyph[ptrGlyphIndex[i]];
                                var item = CreateBatchItem();
                                var rect = glyph.BoundsInTexture;

                                var left = rect.Left * font.TextureTexel.X;
                                var right = rect.Right * font.TextureTexel.X;
                                var top = rect.Top * font.TextureTexel.Y;
                                var bottom = rect.Bottom * font.TextureTexel.Y;

                                item.Texture = font.Texture;
                                item.SortKey = depth;

                                if (flagLines) flagLines = false;
                                else charOrigin = charOrigin.Plus(dirRight * glyph.LeftBearing);

                                var upHeight = dirDown * (font.HeightSpacing - glyph.Cropping.Y);
                                var downHeight = dirUp * (glyph.BoundsInTexture.Height + glyph.Cropping.Y - font.HeightSpacing);

                                if (richText.Colors.TryGetValue(i, out var nextColor)) 
                                { 
                                    currentColor = nextColor;
                                }
                                item.vertexTL.Setup(charOrigin.Plus(upHeight), currentColor, new Vector2(left, top));
                                item.vertexBL.Setup(charOrigin.Plus(downHeight), currentColor, new Vector2(left, bottom));

                                charOrigin = charOrigin.Plus(dirRight * rect.Width);
                                item.vertexTR.Setup(charOrigin.Plus(upHeight), currentColor, new Vector2(right, top));
                                item.vertexBR.Setup(charOrigin.Plus(downHeight), currentColor, new Vector2(right, bottom));

                                if (flagNegativeX ^ flagNegativeY)
                                {
                                    if (flagNegativeX)
                                    {
                                        (item.vertexTL, item.vertexTR) = (item.vertexTR, item.vertexTL);
                                        (item.vertexBL, item.vertexBR) = (item.vertexBR, item.vertexBL);
                                    }
                                    else
                                    {
                                        (item.vertexTL, item.vertexBL) = (item.vertexBL, item.vertexTL);
                                        (item.vertexTR, item.vertexBR) = (item.vertexBR, item.vertexTR);
                                    }
                                }

                                charOrigin = charOrigin.Plus(dirRight * (glyph.RightBearing + font.WidthSpacing));

                                TryReload();
                            }
                        }
                    }
                }
            }
        }

        public unsafe void CameraTextRender(CustomSpriteFont font, string text, Color color,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth, TextAlignmentHorizontal alignment)
        {
            if (CheckErrorText(font, text)) return;
            fixed (char* ptrText = text)
            {
                font.SetGlyphIndexes(ptrText, text.Length, _glyphIndexes, out var lines);
                _lineOrigins.EnsureCapacity(lines);

                var sin = -MathF.Sin(rot * MathExtensions.Deg2Rad);
                var cos = MathF.Cos(rot * MathExtensions.Deg2Rad);

                var flagNegativeX = sca.X < 0;
                var flagNegativeY = sca.Y < 0;

                fixed (int* ptrGlyphIndex = _glyphIndexes.Items)
                {
                    fixed (CustomSpriteFont.Glyph* ptrGlyph = font.Glyphs)
                    {
                        fixed (Vector2* ptrLineOrigin = _lineOrigins.Items)
                        {
                            CalculateTextVectors(font, _glyphIndexes.Size, lines,
                                ptrGlyphIndex, ptrGlyph, ptrText, ptrLineOrigin,
                                Camera.GetAnchorPosCameraInverse(anchor), pos * _posPixelScale, sin, cos, sca, pivot, alignment,
                                out var dirLeft, out var dirRight, out var dirUp, out var dirDown);

                            var flagLines = true;
                            var counterLines = 0;
                            var charOrigin = new Vector3(ptrLineOrigin[counterLines], _zPosition);
                            for (int i = 0; i < _glyphIndexes.Size; i++)
                            {
                                if (ptrText[i] == '\n')
                                {
                                    flagLines = true;
                                    counterLines++;
                                    charOrigin = new Vector3(ptrLineOrigin[counterLines], _zPosition);
                                    continue;
                                }
                                else if (ptrText[i] == '\r')
                                {
                                    continue;
                                }

                                var glyph = ptrGlyph[ptrGlyphIndex[i]];
                                var item = CreateBatchItem();
                                var rect = glyph.BoundsInTexture;

                                var left = rect.Left * font.TextureTexel.X;
                                var right = rect.Right * font.TextureTexel.X;
                                var top = rect.Top * font.TextureTexel.Y;
                                var bottom = rect.Bottom * font.TextureTexel.Y;

                                item.Texture = font.Texture;
                                item.SortKey = depth;

                                if (flagLines) flagLines = false;
                                else charOrigin = charOrigin.Plus(dirRight * glyph.LeftBearing);

                                var upHeight = dirDown * (font.HeightSpacing - glyph.Cropping.Y);
                                var downHeight = dirUp * (glyph.BoundsInTexture.Height + glyph.Cropping.Y - font.HeightSpacing);

                                item.vertexTL.Setup(charOrigin.Plus(upHeight), color, new Vector2(left, top));
                                item.vertexBL.Setup(charOrigin.Plus(downHeight), color, new Vector2(left, bottom));

                                charOrigin = charOrigin.Plus(dirRight * rect.Width);
                                item.vertexTR.Setup(charOrigin.Plus(upHeight), color, new Vector2(right, top));
                                item.vertexBR.Setup(charOrigin.Plus(downHeight), color, new Vector2(right, bottom));

                                if (flagNegativeX ^ flagNegativeY)
                                {
                                    if (flagNegativeX)
                                    {
                                        (item.vertexTL, item.vertexTR) = (item.vertexTR, item.vertexTL);
                                        (item.vertexBL, item.vertexBR) = (item.vertexBR, item.vertexBL);
                                    }
                                    else
                                    {
                                        (item.vertexTL, item.vertexBL) = (item.vertexBL, item.vertexTL);
                                        (item.vertexTR, item.vertexBR) = (item.vertexBR, item.vertexTR);
                                    }
                                }

                                charOrigin = charOrigin.Plus(dirRight * (glyph.RightBearing + font.WidthSpacing));

                                TryReload();
                            }
                        }
                    }
                }
            }
        }
         
        private static unsafe void CalculateTextVectors(CustomSpriteFont font, int textLength, int lineCount,
            int* ptrGlyphIndex, CustomSpriteFont.Glyph* ptrGlyph, char* ptrText, Vector2* ptrLineOrigin,
            Vector2 parentPos, Vector2 pos, float sin, float cos, Vector2 sca, Vector2 pivot, TextAlignmentHorizontal alignment,
            out Vector2 dirLeft, out Vector2 dirRight, out Vector2 dirUp, out Vector2 dirDown)
        {
            dirRight = (sca.X * Vector2.UnitX).RotateVector(sin, cos);
            dirUp = (sca.Y * Vector2.UnitY).RotateVector(sin, cos);
            dirLeft = -dirRight; dirDown = -dirUp;

            var flagLines = true;
            var textSize = Vector2.Zero;
            var counterLines = 0;
            ptrLineOrigin[0] = Vector2.Zero;

            for (int i = 0; i < textLength; i++)
            {
                switch (ptrText[i])
                {
                    case '\n':
                        flagLines = true;
                        //textSize.Y += ptrLineOrigin[counterLines].Y;
                        counterLines++;
                        ptrLineOrigin[counterLines] = Vector2.Zero;
                        continue;
                    case '\r':
                        continue;
                }

                var glyph = ptrGlyph[ptrGlyphIndex[i]];

                if (flagLines)
                {
                    flagLines = false;
                    ptrLineOrigin[counterLines].X += glyph.Width + glyph.RightBearing;
                }
                else
                {
                    ptrLineOrigin[counterLines].X += font.WidthSpacing + glyph.WidthIncludingBearings;
                }


                if (glyph.BoundsInTexture.Height > ptrLineOrigin[counterLines].Y)
                    ptrLineOrigin[counterLines].Y = glyph.BoundsInTexture.Height;
                if (ptrLineOrigin[counterLines].X > textSize.X)
                    textSize.X = ptrLineOrigin[counterLines].X;
            }
            //textSize.Y += ptrLineOrigin[counterLines].Y;
            textSize.Y = font.HeightSpacing * lineCount;

            var posCounter = ptrLineOrigin[0].Y * dirUp;
            var pivotOffset = dirDown * ((1f - pivot.Y) * 0.5f * textSize.Y) 
                + dirLeft * ((pivot.X + 1f) * 0.5f * textSize.X);

            for (counterLines = 0; counterLines < lineCount; counterLines++)
            {
                var alignmentOffset = alignment == TextAlignmentHorizontal.Left ? Vector2.Zero
                    : alignment == TextAlignmentHorizontal.Center
                    ? (textSize.X - ptrLineOrigin[counterLines].X) / 2f * dirRight
                    : (textSize.X - ptrLineOrigin[counterLines].X) * dirRight;
                ptrLineOrigin[counterLines] = parentPos + pos + posCounter + pivotOffset + alignmentOffset;
                posCounter += font.HeightSpacing * dirUp;
            }
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

            CalculateVertexes(Camera.GetAnchorPosWorldInverse(anchor), pos * _posPixelScale, rot + Camera.Transform.Rot,
                sca * _pixelScale, pivot, out var TL, out var TR, out var BL, out var BR);

            if (sca.X < 0) (viewStart.X, viewEnd.X) = (viewEnd.X, viewStart.X);
            if (sca.Y < 0) (viewStart.Y, viewEnd.Y) = (viewEnd.Y, viewStart.Y);

            spriteBatchItem.vertexTL.Setup(new Vector3(TL.X, TL.Y, _zPosition), color, viewStart);
            spriteBatchItem.vertexTR.Setup(new Vector3(TR.X, TR.Y, _zPosition), color, new Vector2(viewEnd.X, viewStart.Y));
            spriteBatchItem.vertexBL.Setup(new Vector3(BL.X, BL.Y, _zPosition), color, new Vector2(viewStart.X, viewEnd.Y));
            spriteBatchItem.vertexBR.Setup(new Vector3(BR.X, BR.Y, _zPosition), color, viewEnd);

            TryReload();
        }

        public void CameraRender(Texture2D texture, Color color, Vector2 viewStart, Vector2 viewEnd,
            Vector2 pos, float rot, Vector2 sca, Vector2 anchor, Vector2 pivot, int depth)
        {
            if (CheckErrorSprite(texture)) return;

            var spriteBatchItem = CreateBatchItem();
            spriteBatchItem.Texture = texture;
            spriteBatchItem.SortKey = depth;

            CalculateVertexes(Camera.GetAnchorPosCameraInverse(anchor), pos * _posPixelScale, rot,
                sca * _pixelScale, pivot, out var TL, out var TR, out var BL, out var BR);

            if (sca.X < 0) (viewStart.X, viewEnd.X) = (viewEnd.X, viewStart.X);
            if (sca.Y < 0) (viewStart.Y, viewEnd.Y) = (viewEnd.Y, viewStart.Y);

            spriteBatchItem.vertexTL.Setup(new Vector3(TL.X, TL.Y, _zPosition), color, viewStart);
            spriteBatchItem.vertexTR.Setup(new Vector3(TR.X, TR.Y, _zPosition), color, new Vector2(viewEnd.X, viewStart.Y));
            spriteBatchItem.vertexBL.Setup(new Vector3(BL.X, BL.Y, _zPosition), color, new Vector2(viewStart.X, viewEnd.Y));
            spriteBatchItem.vertexBR.Setup(new Vector3(BR.X, BR.Y, _zPosition), color, viewEnd);

            TryReload();
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

            var sin = -MathF.Sin(rot * MathExtensions.Deg2Rad);
            var cos = MathF.Cos(rot * MathExtensions.Deg2Rad);

            pos = parentPos + pos;
            TR = MathExtensions.RotateVector(BR.X, TL.Y, pos, sin, cos);
            BL = MathExtensions.RotateVector(TL.X, BR.Y, pos, sin, cos);
            TL = TL.RotateVector(pos, sin, cos);
            BR = BR.RotateVector(pos, sin, cos);
        }
    }
}
