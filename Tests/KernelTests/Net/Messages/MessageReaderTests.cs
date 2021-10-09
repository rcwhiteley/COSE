using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kernel.Net.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kernel.Net.Messages.Tests
{
    [TestClass()]
    public class MessageReaderTests
    {
        private readonly MessageReader reader;
        private readonly BinaryWriter writer;
        private readonly Memory<byte> memory;
        private readonly byte[] buffer;

        public MessageReaderTests()
        {
            buffer = new byte[1024];
            memory = new Memory<byte>(buffer);
            reader = new MessageReader(memory);
            writer = new BinaryWriter(new MemoryStream(buffer), Encoding.ASCII);
        }
        [TestMethod()]
        [DataRow((byte)5, (byte)10)]
        public void ReadByte(byte val1, byte val2)
        {
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(val1, reader.ReadByte());
            Assert.AreEqual(val2, reader.ReadByte());
        }

        [DataRow((Int16)5, (Int16)10)]
        [TestMethod()]
        public void ReadInt16Test(Int16 val1, Int16 val2)
        {
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(val1, reader.ReadInt16());
            Assert.AreEqual(val2, reader.ReadInt16());
        }

        [DataRow((UInt16)5, (UInt16)10)]
        [TestMethod()]
        public void ReadUInt16Test(UInt16 val1, UInt16 val2)
        {
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(val1, reader.ReadUInt16());
            Assert.AreEqual(val2, reader.ReadUInt16());
        }

        [DataRow(5, 10)]
        [TestMethod()]
        public void ReadInt32Test(Int32 val1, Int32 val2)
        {
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(val1, reader.ReadInt32());
            Assert.AreEqual(val2, reader.ReadInt32());
        }

        [DataRow((UInt32)5, (UInt32)10)]
        [TestMethod()]
        public void ReadUInt32Test(UInt32 val1, UInt32 val2)
        {
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(val1, reader.ReadUInt32());
            Assert.AreEqual(val2, reader.ReadUInt32());
        }

        [DataRow(5, 10)]
        [TestMethod()]
        public void ReadInt64Test(Int64 val1, Int64 val2)
        {
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(val1, reader.ReadInt64());
            Assert.AreEqual(val2, reader.ReadInt64());
        }

        [DataRow((UInt64)5, (UInt64)10)]
        [TestMethod()]
        public void ReadUInt64Test(UInt64 val1, UInt64 val2)
        {
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(val1, reader.ReadUInt64());
            Assert.AreEqual(val2, reader.ReadUInt64());
        }

        [DataRow(5, 10)]
        [TestMethod()]
        public void ReadSingleTest(Single val1, Single val2)
        {
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(val1, reader.ReadSingle());
            Assert.AreEqual(val2, reader.ReadSingle());
        }

        [DataRow(5, 10)]
        [TestMethod()]
        public void ReadDoubleTest(Double val1, Double val2)
        {
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(val1, reader.ReadDouble());
            Assert.AreEqual(val2, reader.ReadDouble());
        }

        [DataRow("Hello", "World")]
        [TestMethod()]
        public void ReadCStringTest(string val1, string val2)
        {
            writer.Write(Encoding.ASCII.GetBytes(val1));
            writer.Write(Encoding.ASCII.GetBytes(val2) );
            Assert.AreEqual(val1, reader.ReadCString(val1.Length));
            Assert.AreEqual(val2, reader.ReadCString(val2.Length));
        }

        [DataRow("Hello", "World")]
        [TestMethod()]
        public void ReadStringTest(string val1, string val2)
        {
            writer.Write(val1);
            writer.Write(val2);
            Assert.AreEqual(val1, reader.ReadString());
            Assert.AreEqual(val2, reader.ReadString());
        }

        [DataRow("Hello", "new", "World")]
        [TestMethod()]
        public void ReadStringListTest(string val1, string val2, string val3)
        {
            
            writer.Write((byte)3);
            writer.Write(val1);
            writer.Write(val2);
            writer.Write(val3);
            var list = reader.ReadStringList();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(val1, list[0]);
            Assert.AreEqual(val2, list[1]);
            Assert.AreEqual(val3, list[2]);
        }
    }
}