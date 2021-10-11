using AuthenticationServiceModel;
using Kernel.Net.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Net.Packets
{
    public unsafe class MsgConnect
    { 

        public ushort Length { get; set; }
        public ushort Type { get; set; }
        public uint Identity { get; set; }
        public AuthenticationResponse Response { get; set; }
        public string RedirectIP { get; set; }
        public uint RedirectPort { get; set; }

        public int Serialize(Memory<byte> memory)
        {
            throw new NotImplementedException();
        }
    }
}
