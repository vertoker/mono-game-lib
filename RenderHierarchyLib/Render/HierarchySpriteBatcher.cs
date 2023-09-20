using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Core
{
    public class HierarchySpriteBatcher
    {
        private const int InitialBatchSize = 256;

        private const int MaxBatchSize = 5461;

        private const int InitialVertexArraySize = 256;

        private HierarchySpriteBatchItem[] _batchItemList;

        private int _batchItemCount;

        private readonly GraphicsDevice _device;

        private short[] _index;

        private VertexPositionColorTexture[] _vertexArray;

        public HierarchySpriteBatcher(GraphicsDevice device, int capacity = 256)
        {
            _device = device;
            capacity = (capacity > 0) ? ((capacity + 63) & -64) : 256;
            _batchItemList = new HierarchySpriteBatchItem[capacity];
            _batchItemCount = 0;
            for (int i = 0; i < capacity; i++)
                _batchItemList[i] = new HierarchySpriteBatchItem();

            EnsureArrayCapacity(capacity);
        }

        public HierarchySpriteBatchItem CreateBatchItem()
        {
            if (_batchItemCount >= _batchItemList.Length)
            {
                int num = _batchItemList.Length;
                int num2 = num + num / 2;
                num2 = (num2 + 63) & -64;
                Array.Resize(ref _batchItemList, num2);
                for (int i = num; i < num2; i++)
                {
                    _batchItemList[i] = new HierarchySpriteBatchItem();
                }

                EnsureArrayCapacity(Math.Min(num2, 5461));
            }

            return _batchItemList[_batchItemCount++];
        }

        private unsafe void EnsureArrayCapacity(int numBatchItems)
        {
            int num = 6 * numBatchItems;
            if (_index != null && num <= _index.Length)
            {
                return;
            }

            short[] array = new short[6 * numBatchItems];
            int num2 = 0;
            if (_index != null)
            {
                _index.CopyTo(array, 0);
                num2 = _index.Length / 6;
            }

            fixed (short* ptr = array)
            {
                short* ptr2 = ptr + num2 * 6;
                int num3 = num2;
                while (num3 < numBatchItems)
                {
                    *ptr2 = (short)(num3 * 4);
                    ptr2[1] = (short)(num3 * 4 + 1);
                    ptr2[2] = (short)(num3 * 4 + 2);
                    ptr2[3] = (short)(num3 * 4 + 1);
                    ptr2[4] = (short)(num3 * 4 + 3);
                    ptr2[5] = (short)(num3 * 4 + 2);
                    num3++;
                    ptr2 += 6;
                }
            }

            _index = array;
            _vertexArray = new VertexPositionColorTexture[4 * numBatchItems];
        }

        public unsafe void DrawBatch()
        {
            if (_batchItemCount == 0) return;

            int num = 0;
            int num2 = _batchItemCount;
            while (num2 > 0)
            {
                int start = 0;
                int num3 = 0;
                Texture2D texture2D = null;
                int num4 = num2;
                if (num4 > 5461)
                {
                    num4 = 5461;
                }

                fixed (VertexPositionColorTexture* ptr = _vertexArray)
                {
                    VertexPositionColorTexture* ptr2 = ptr;
                    int num5 = 0;
                    while (num5 < num4)
                    {
                        var spriteBatchItem = _batchItemList[num];
                        if (spriteBatchItem.Texture != texture2D)
                        {
                            FlushVertexArray(start, num3);
                            texture2D = spriteBatchItem.Texture;
                            start = (num3 = 0);
                            ptr2 = ptr;
                            _device.Textures[0] = texture2D;
                        }

                        *ptr2 = spriteBatchItem.vertexTL;
                        ptr2[1] = spriteBatchItem.vertexTR;
                        ptr2[2] = spriteBatchItem.vertexBL;
                        ptr2[3] = spriteBatchItem.vertexBR;
                        spriteBatchItem.Texture = null;
                        num5++;
                        num++;
                        num3 += 4;
                        ptr2 += 4;
                    }
                }

                FlushVertexArray(start, num3);
                num2 -= num4;
            }

            _batchItemCount = 0;
        }

        private void FlushVertexArray(int start, int end)
        {
            if (start == end) return;
            int num = end - start;

            _device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertexArray, 0, num, _index, 0, num / 4 * 2, VertexPositionColorTexture.VertexDeclaration);
        }
    }
}
