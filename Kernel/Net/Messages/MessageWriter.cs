using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Net.Messages
{
    //TODO: Sanity Checks
    public unsafe class MessageWriter
    {
        public Memory<byte> Memory { get; set; }
        private int offset;

        public MessageWriter(Memory<byte> memory)
        {
            offset = 0;
            Length = 0;
            Memory = memory;
        }

        public int Offset
        {
            get
            {
                return offset;
            }
            set
            {
                offset = value;
                Length = Math.Max(offset, Length);
            }
        }

        public int Length { get; private set; }

        public void Write(byte value)
        {
            Memory.Span[Offset++] = value;
        }

        public void Write(short value)
        {
            void* ptr = &value;
            new Span<byte>(ptr, sizeof(short)).CopyTo(Memory.Span.Slice(Offset));
            Offset += sizeof(short);
        }

        public void Write(ushort value)
        {
            void* ptr = &value;
            new Span<byte>(ptr, sizeof(ushort)).CopyTo(Memory.Span.Slice(Offset));
            Offset += sizeof(ushort);

        }

        public void Write(int value)
        {
            void* ptr = &value;
            new Span<byte>(ptr, sizeof(int)).CopyTo(Memory.Span.Slice(Offset));
            Offset += sizeof(int);
        }

        public void Write(uint value)
        {
            void* ptr = &value;
            new Span<byte>(ptr, sizeof(uint)).CopyTo(Memory.Span.Slice(Offset));
            Offset += sizeof(uint);

        }

        public void Write(long value)
        {
            void* ptr = &value;
            new Span<byte>(ptr, sizeof(long)).CopyTo(Memory.Span.Slice(Offset));
            Offset += sizeof(long);
        }

        public void Write(ulong value)
        {
            void* ptr = &value;
            new Span<byte>(ptr, sizeof(ulong)).CopyTo(Memory.Span.Slice(Offset));
            Offset += sizeof(ulong);

        }

        public void Write(float value)
        {
            void* ptr = &value;
            new Span<byte>(ptr, sizeof(float)).CopyTo(Memory.Span.Slice(Offset));
            Offset += sizeof(float);
        }

        public void Write(double value)
        {
            void* ptr = &value;
            new Span<byte>(ptr, sizeof(double)).CopyTo(Memory.Span.Slice(Offset));
            Offset += sizeof(double);

        }

        public void Write(string value)
        {
            Write((byte)value.Length);
            byte[] buffer = Encoding.ASCII.GetBytes(value);
            new Span<byte>(buffer).CopyTo(Memory.Span.Slice(Offset));
            Offset += value.Length;
        }


        public void Write(string value, int length)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(value);
            int copyLen = Math.Min(buffer.Length, length);
            new Span<byte>(buffer, 0, copyLen).CopyTo(Memory.Span.Slice(Offset));
            Offset += length;
        }
        
        public void Write(byte[] value)
        {
            new Span<byte>(value).CopyTo(Memory.Span.Slice(Offset));
            Offset += value.Length;
        }

        public void Write(byte[] value, int size)
        {
            new Span<byte>(value, 0, size).CopyTo(Memory.Span.Slice(Offset));
            Offset += size;
        }

        public void Write(IEnumerable<string> strings)
        {
            Write((byte)strings.Count());
            foreach(var str in strings)
            {
                Write(str);
            }
        }
    }
}
