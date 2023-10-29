using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderHierarchyLib.Render
{
    public class HierarchyRenderBatchPreset
    {
        public int zPosition = 0;
        public int spriteCapacity = 256;
        public int glyphIndexesCapacity = 16;
        public int lineOriginsCapacity = 4;
        public bool autoReloadBatching = true;
        public int batchMaxSize = 5460 / 4;

        public static HierarchyRenderBatchPreset Default = new();
    }
}
