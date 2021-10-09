using Microsoft.Extensions.ObjectPool;
using System;
using System.Buffers;
using System.Net.Sockets;

namespace Kernel.Net
{
    // TODO-LIST:
    // Recycle SocketAsyncEventArgs
    public class TcpChannel
    {
        public ObjectPool<SocketAsyncEventArgs> SocketArgsPool { get; set; }
        public MemoryPool<byte> BufferProvider { get; set; }

        public int MaxPacketSize { get; set; } = 2048;
        Socket socket;
        public event EventHandler<SocketAsyncEventArgs> PreparingSend;
        public event EventHandler<SocketAsyncEventArgs> PreparingReceive;
        public event EventHandler<SocketAsyncEventArgs> Received;
        public event EventHandler<SocketAsyncEventArgs> Sent;
        public event EventHandler<SocketAsyncEventArgs> Connecting;
        public event EventHandler<SocketAsyncEventArgs> Connected;
        public event EventHandler Disconnecting;
        public event EventHandler Disconnected;
        private InterlockedBoolean socketIsActive;

        public object UserToken { get; set; }

        public TcpChannel(Socket connectedSocket)
        {
            socket = connectedSocket;
            socketIsActive = new InterlockedBoolean(true);
        }

        public TcpChannel()
        {
            socketIsActive = new InterlockedBoolean(false);
        }

        private SocketAsyncEventArgs AcquireSocketEventArgs()
        {
            if (SocketArgsPool != null)
                return SocketArgsPool.Get();
            return new SocketAsyncEventArgs();
        }

        private void ReturnSocketEventArgs(SocketAsyncEventArgs e)
        {
            SocketArgsPool?.Get();
        }
        
        private void AcquireBuffer(SocketAsyncEventArgs e, int requiredLength)
        {
            IMemoryOwner<byte> memoryOwner = BufferProvider.Rent(requiredLength);
            e.SetBuffer(memoryOwner.Memory.Slice(0, requiredLength));
            e.UserToken = memoryOwner;
        }

        private void ReturnBuffer(SocketAsyncEventArgs e)
        {
            IMemoryOwner<byte> memoryOwner = e.UserToken as IMemoryOwner<byte>;
            if (memoryOwner == null)
                throw new InvalidCastException("SocketAsyncEventArgs.UserToken is not a IMemoryOwner instance");
            e.SetBuffer(null);
            e.UserToken = null;
            memoryOwner.Dispose();
        }


        public void SendAsync(ReadOnlyMemory<byte> buffer)
        {
            SocketAsyncEventArgs e = AcquireSocketEventArgs();
            AcquireBuffer(e, buffer.Length);
            buffer.CopyTo(e.MemoryBuffer);  
            OnPreparingSend(e);

            if (!socket.SendAsync(e))
                CompleteSend(e);
        }

        public void ReceiveAsync()
        {
            SocketAsyncEventArgs e = AcquireSocketEventArgs();
            AcquireBuffer(e, MaxPacketSize);
            OnPreparingReceive(e);

            if (!socket.ReceiveAsync(e))
                CompleteReceive(e);
        }


        private void Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Send:
                    CompleteSend(e);
                    break;
                case SocketAsyncOperation.Receive:
                    CompleteReceive(e);
                    break;
            }
            ReturnBuffer(e);
            ReturnSocketEventArgs(e);
        }

        private void CompleteReceive(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success && socketIsActive.State && socket.Connected)
            {
                OnReceived(e);
            }
            else
            {
                CloseAsync();
            }
        }

        private void CompleteSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success 
                && socketIsActive.State 
                && socket.Connected
                && e.BytesTransferred > 0)
            {
                OnSent(e);
            }
            else
            {
                CloseAsync();
            }
        }

        private void CloseAsync()
        {
            Console.WriteLine("Closing");
            if (socketIsActive.CompareExchange(false, true) == false)
                return;
                
            try
            {
                socket.Shutdown(SocketShutdown.Both);

            }
            finally 
            {
                OnDisconnected(AcquireSocketEventArgs());
                socket?.Close();
            }
        }

        protected virtual void OnDisconnected(SocketAsyncEventArgs e)
        {
            var handler = Disconnected;
            handler?.Invoke(this,e);
        }

        protected virtual void OnPreparingSend(SocketAsyncEventArgs e)
        {
            var handler = PreparingSend;
            handler?.Invoke(this, e);
        }

        protected virtual void OnPreparingReceive(SocketAsyncEventArgs e)
        {
            var handler = PreparingReceive;
            handler?.Invoke(this, e);
        }

        protected virtual void OnSent(SocketAsyncEventArgs e)
        {
            var handler = Sent;
            handler?.Invoke(this, e);
        }

        protected virtual void OnReceived(SocketAsyncEventArgs e)
        {
            var handler = Received;
            handler?.Invoke(this, e);
        }
    }
}
