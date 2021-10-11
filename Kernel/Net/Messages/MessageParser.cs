using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Kernel.Net.Messages
{
    public class MessageParser<T>
    {
        private MessageProvider<T> messageProvider;
        private Pipe dataPipe;
        private PipeWriter dataWriter;
        private PipeReader dataReader;
        public event EventHandler<Message<T>> MessageParsed;
        public event EventHandler<ReadOnlySequence<byte>> DataDiscarded;
        public int SealLength { get; set; } // TQServer seal
        public MessageParser(MessageProvider<T> messageProvider, int sealLength)
        {
            this.SealLength = sealLength;
            this.messageProvider = messageProvider;
            dataPipe = new Pipe();
            dataWriter = dataPipe.Writer;
            dataReader = dataPipe.Reader;
        }

        public async Task ThrowBytes(Memory<byte> data)
        {
            var memory = dataWriter.GetMemory(data.Length);
            data.CopyTo(memory);
            dataWriter.Advance(data.Length);
            await dataWriter.FlushAsync();
            TryParseMessages();
        }

        private void TryParseMessages()
        {

            if (dataReader.TryRead(out ReadResult result))
            {
                ushort len = BitConverter.ToUInt16(result.Buffer.FirstSpan);
                if (result.Buffer.Length >= len)
                {
                    ushort type = BitConverter.ToUInt16(result.Buffer.Slice(2).FirstSpan);
                    var message = messageProvider.Create(type);
                    var messageData = result.Buffer.Slice(0, len);
                    if (message != null)
                    {
                        message.Deserialize(new MessageReader(messageData));
                        OnMessageParsed(message);
                    }
                    else
                    {
                        OnDataDiscarded(messageData);
                    }

                    dataReader.AdvanceTo(messageData.End);
                    TryParseMessages();
                }
            }
        }

        protected virtual void OnDataDiscarded(ReadOnlySequence<byte> messageData)
        {
            var handler = DataDiscarded;
            handler?.Invoke(this, messageData);
        }

        protected virtual void OnMessageParsed(Message<T> message)
        {
            var handler = MessageParsed;
            handler?.Invoke(this, message);
        }
    }
}
