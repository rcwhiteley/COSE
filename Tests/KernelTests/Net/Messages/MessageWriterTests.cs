using Kernel.Net.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Tests.Net.Messages.Tests
{
    [TestClass]
    public class MessageWriterTests
    {
        private readonly MessageWriter writer;
        private readonly BinaryReader reader;
        private readonly Memory<byte> memory;
        private readonly byte[] buffer;

        public MessageWriterTests()
        {
            buffer = new byte[1024];
            memory = new Memory<byte>(buffer);
            writer = new MessageWriter(memory);
            reader = new BinaryReader(new MemoryStream(buffer), Encoding.ASCII);
        }

        [TestMethod("WriteByteTest")]
        public void WriteByteTest()
        {
            byte val1 = 5;
            byte val2 = 10;
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(reader.ReadByte(), val1);
            Assert.AreEqual(reader.ReadByte(), val2);
        }

        [TestMethod("WriteShortTest")]
        public void WriteShortTest()
        {
            short val1 = 5;
            short val2 = 10;
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(reader.ReadInt16(), val1);
            Assert.AreEqual(reader.ReadInt16(), val2);
        }
        [TestMethod("WriteUInt32Test")]
        public void WriteUInt32Test()
        {
            UInt32 val1 = 5;
            UInt32 val2 = 10;
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(reader.ReadUInt32(), val1);
            Assert.AreEqual(reader.ReadUInt32(), val2);
        }

        [TestMethod("WriteUInt64Test")]
        public void WriteUInt64Test()
        {
            UInt64 val1 = 5;
            UInt64 val2 = 10;
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(reader.ReadUInt64(), val1);
            Assert.AreEqual(reader.ReadUInt64(), val2);
        }

        [TestMethod("WriteDoubleTest")]
        public void WriteDoubleTest()
        {
            double val1 = 5.3;
            double val2 = 10.7;
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(reader.ReadDouble(), val1);
            Assert.AreEqual(reader.ReadDouble(), val2);
        }

        [TestMethod("WriteCStringTest1")]
        [DataRow("a!@#re", 8)]
        public void WriteCStringTest1(string val, int len)
        {
           
            writer.Write(val, len);
            string read = Encoding.ASCII.GetString(reader.ReadBytes(len)).Trim().Trim('\0');
            Assert.AreEqual(val, read);

        }

        [TestMethod("WriteCStringTest2")]
        [DataRow("Hell#o", 3)]
        public void WriteCStringTest2(string val, int len)
        {

            writer.Write(val, len);
            string toCompare = val.Substring(0, len);
            string read = Encoding.ASCII.GetString(reader.ReadBytes(len));
            Assert.AreEqual(toCompare, read );

        }

        [DataRow("World")]
        [TestMethod("WriteStringTest")]
        public void WriteStringTest(string val)
        {
            writer.Write(val);
            Assert.AreEqual(val, reader.ReadString());
        }
        //[TestMethod]
        public void WriteStringListTest()
        {
            List<string> strings = new List<string>() { "Preparing", "Hello", "World" };
            List<string> results = new List<string>();
            writer.Write(strings);
            int count = reader.ReadByte();
            for (int i = 0; i < count; i++)
            {
                results.Add(reader.ReadString());
            }

            for (int i = 0; i < results.Count; i++)
            {
                Assert.AreEqual(strings[i], results[i]);
            }
        }
    }
}
