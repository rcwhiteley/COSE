using Kernel.Net.Messages;
using System;
using System.Text;
using System.Buffers;
using Kernel.Net.Security;
using System.Runtime.InteropServices;
using Kernel.Serialization;

namespace AccountService.Net.Packets
{
    [Message(1051)]
    public unsafe class MsgAccount : Message<MsgAccount, Client>
    {
        [Serialize(0)]
        public override ushort Length { get; set; }

        [Serialize(1)]
        public override ushort Type { get; set; }

        [Serialize(2, FixedLength = 16)]
        public string Username { get; set; }

        [Serialize(3, FixedLength = 16)]
        public byte[] Password { get; set; }

        [Serialize(4, FixedLength = 16)] 
        public string ServerName { get; set; }

        public override void Process(Client owner)
        {
            new RivestCipher5().Decode(Password);

            Console.WriteLine($"{Username} {Encoding.ASCII.GetString(Password)} {ServerName}");
        }

    }
}