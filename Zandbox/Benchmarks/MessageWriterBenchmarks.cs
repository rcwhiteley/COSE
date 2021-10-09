using BenchmarkDotNet.Attributes;
using Kernel.Net.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class MessageWriterBenchmarks
    {
        private readonly byte[] buff1;
        private readonly byte[] buff2;
        private readonly Memory<byte> memory;
        BinaryWriter bw;
        MessageWriter mw;

        public MessageWriterBenchmarks()
        {
            buff1 = new byte[1024];
            buff2 = new byte[1024];
            memory = new Memory<byte>(buff2);
           
            
        }

        [Benchmark]
        public void WriteInt32BinaryWriter()
        {
            bw = new BinaryWriter(new MemoryStream(buff1));
            for (int i = 0; i < 50; i++)
            bw.Write(80000);
        }
        [Benchmark]
        public void WriteInt32MessageWriter()
        {
            mw = new MessageWriter(memory);
            for (int i = 0; i < 50; i++)
                mw.Write(80000);
        }

        [Benchmark]
        public void WriteStringBinaryWriter()
        {
            bw = new BinaryWriter(new MemoryStream(buff1));
            for (int i = 0; i < 20; i++)
                bw.Write("string test");
        }

        [Benchmark]
        public void WriteStringMessageWriter()
        {
            mw = new MessageWriter(memory);
            for (int i = 0; i < 20; i++)
                mw.Write("string test");
        }
    }
}
