using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UILib.Core;

namespace UILib.Interfaces.Core
{
    public interface IUIModule : IUIDraw, IUIUpdate, IDisposable
    {
        public void Initialize(UI ui);
    }
}
