using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Net.Security
{
    public interface IEncoder
    {
        void Encode(Span<byte> bufferdata);
    }
}
