using Kernel.Net.Messages;
using System;
using System.Text;
using System.Buffers;
using Kernel.Net.Security;
using System.Runtime.InteropServices;

namespace AccountService.Net.Packets
{

    // message structure from https://gitlab.com/conquer-online/wiki/-/wikis/Packets/MsgAccount
    [Message(1051)]
    public unsafe class MsgAccount : IIncomingMessage<Client>
    {
        public Client Owner { get; set; }
        public ushort Length { get => msg.Length; set => msg.Length = value; }
        public ushort Type { get => msg.Type; set => msg.Type = value; }
        private MessageData msg;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct MessageData
        {
            public ushort Length;
            public ushort Type;
            public fixed byte Username[16];
            public fixed byte Password[16];
            public fixed byte ServerName[16];
        }

        public void Deserialize(ReadOnlySequence<byte> memory)
        {

        }

        public void Process()
        {
            //throw new NotImplementedException();
        }
    }
}
