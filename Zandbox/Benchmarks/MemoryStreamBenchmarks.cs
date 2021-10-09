using BenchmarkDotNet.Attributes;
using Microsoft.IO;
using System;
using System.Buffers;
using System.IO;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class MemoryStreamBenchmarks
    {
        private static readonly byte[] buffer = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private static readonly RecyclableMemoryStreamManager rmsm = new RecyclableMemoryStreamManager();
        [Benchmark]
        public void CreateRecyclableMemoryStream()
        {
            for (int i = 0; i < 10; i++)
            {
                byte[] buf = ArrayPool<byte>.Shared.Rent(20000);
                var ms = new MemoryStream(buf,0, buf.Length, true, true);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                ArrayPool<byte>.Shared.Return(ms.GetBuffer());
                ms.Dispose();
            }
        }

        [Benchmark]
        public void CreateMemoryStream()
        {
            for (int i = 0; i < 10; i++)
            {
                MemoryStream ms = new MemoryStream(20000);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                //ms.Write(buffer);
                ms.Dispose();
            }
        }
    }
}
