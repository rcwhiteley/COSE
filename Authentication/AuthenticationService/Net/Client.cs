using Kernel.Net.Security;
using Kernel.Net;
using System;
using System.IO.Pipelines;
using System.Net.Sockets;
using Kernel.Net.Messages;

namespace AccountService.Net
{
    public class Client : ServiceActor
    {
        private static Serilog.ILogger log => Serilog.Log.ForContext<Client>();

        public Guid Guid { get; }
        private IDecoder decoder;
        private IEncoder encoder;
        private TcpChannel channel;
        private MessageParser<Client> messageParser;

        public Client(ServiceContext context, TcpChannel channel) : base(context)
        {
            Guid = Guid.NewGuid();

            messageParser = new MessageParser<Client>(context.MessageProvider, 0);
            messageParser.MessageParsed += MessageParser_MessageParsed;
            messageParser.DataDiscarded += MessageParser_DataDiscarded;

            this.channel = channel;
            channel.Received += Client_Received;
            channel.PreparingSend += Client_PreparingSend;
            channel.Disconnected += Client_Disconnected;
            channel.UserToken = this;
            channel.BufferProvider = System.Buffers.MemoryPool<byte>.Shared;

            var cipher = new TqCipher();
            decoder = cipher;
            encoder = cipher;
            log.Debug("{serverName}: Accepting client with Guid: {guid}", context.Name, Guid);
            channel.ReceiveAsync();
        }

        private void MessageParser_DataDiscarded(object sender, System.Buffers.ReadOnlySequence<byte> e)
        {
            log.Debug("{serverName}: Client with Guid {guid} failed to find a message structure for message type {messageType}", Context.Name, Guid, BitConverter.ToUInt16(e.Slice(2).FirstSpan));
        }

        private void MessageParser_MessageParsed(object sender, IIncomingMessage<Client> e)
        {
            log.Debug("{serverName}: Client with Guid {guid} succesfully parsed a message with type {messageType}", Context.Name, Guid, e.Type);
            e.Owner = this;
            Context.WorkerPool.Enqueue(e);
        }

        private void Client_PreparingSend(object sender, SocketAsyncEventArgs e)
        {
            encoder.Encode(e.MemoryBuffer.Span);
        }

        private async void Client_Received(object sender, SocketAsyncEventArgs e)
        {
            decoder.Decode(e.MemoryBuffer.Span);
            await messageParser.ThrowBytes(e.MemoryBuffer.Slice(0, e.BytesTransferred));
            channel.ReceiveAsync();
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Client disconnected");
        }
    }
}
