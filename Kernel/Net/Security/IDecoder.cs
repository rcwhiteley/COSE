using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Net.Security
{
    public interface IDecoder
    {
        public void Decode(Span<byte> data);
    }
}
