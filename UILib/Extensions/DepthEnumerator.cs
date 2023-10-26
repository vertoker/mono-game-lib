using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UILib.Extensions
{
    public class DepthEnumerator : IEnumerator<int>
    {
        private readonly int _defaultDepth;
        private int _currentDepth;

        public DepthEnumerator(int defaultDepth)
        {
            _defaultDepth = defaultDepth;
            _currentDepth = defaultDepth;
        }

        int IEnumerator<int>.Current => _currentDepth;
        public object Current => _currentDepth;

        public void Dispose()
        {

        }
        public bool MoveNext()
        {
            _currentDepth++;
            return true;
        }
        public void Reset()
        {
            _currentDepth = _defaultDepth;
        }
    }
}
