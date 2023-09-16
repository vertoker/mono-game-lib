using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneFramework.Interfaces.Kernel.Base
{
    public interface IServiceUpdateable
    {
        public void Update(GameTime time);
    }
}
