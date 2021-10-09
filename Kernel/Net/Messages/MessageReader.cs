﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Net.Messages
{
    public class MessageReader
    {
        public int Offset { get; set; }
        public Memory<byte> Memory { get; set; }

        public MessageReader(Memory<byte> memory)
        {
            Memory = memory;
            Offset = 0;
        }

        public byte ReadByte()
        {
            return Memory.Span[Offset++];
        }

        public short ReadInt16()
        {
            var result = BitConverter.ToInt16(Memory.Span.Slice(Offset));
            Offset += sizeof(short);
            return result;
        }

        public ushort ReadUInt16()
        {
            var result = BitConverter.ToUInt16(Memory.Span.Slice(Offset));
            Offset += sizeof(ushort);
            return result;
        }

        public int ReadInt32()
        {
            var result = BitConverter.ToInt32(Memory.Span.Slice(Offset));
            Offset += sizeof(int);
            return result;
        }

        public uint ReadUInt32()
        {
            var result = BitConverter.ToUInt32(Memory.Span.Slice(Offset));
            Offset += sizeof(uint);
            return result;
        }

        public long ReadInt64()
        {
            var result = BitConverter.ToInt64(Memory.Span.Slice(Offset));
            Offset += sizeof(long);
            return result;
        }

        public ulong ReadUInt64()
        {
            var result = BitConverter.ToUInt64(Memory.Span.Slice(Offset));
            Offset += sizeof(ulong);
            return result;
        }

        public float ReadSingle()
        {
            var result = BitConverter.ToSingle(Memory.Span.Slice(Offset));
            Offset += sizeof(float);
            return result;
        }

        public double ReadDouble()
        {
            var result = BitConverter.ToDouble(Memory.Span.Slice(Offset));
            Offset += sizeof(double);
            return result;
        }

        public String ReadCString(int length)
        {
            string result = Encoding.ASCII.GetString(Memory.Span.Slice(Offset, length)).Trim('\0');
            Offset += length;
            return result;
        }

        public string ReadString()
        {
            byte len = ReadByte();
            return ReadCString(len);
        }

        public IList<string> ReadStringList()
        {
            byte count = ReadByte();
            List<string> result = new List<string>(count);
            while (count-- > 0)
            {
                result.Add(ReadString());
            }
            return result;
        }
    }
}
