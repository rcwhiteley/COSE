using Kernel.Workflow;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.Net
{
    //TODO: Better restart support
    public class TcpServer
    {
        private Socket listener { get; set; }

        public event EventHandler<SocketAsyncEventArgs> Accepted;
        public event EventHandler Closing;
        public event EventHandler Closed;
        private InterlockedBoolean active = new InterlockedBoolean(false);

        public TcpServer()
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Bind(IPEndPoint endPoint)
        {
            listener.Bind(endPoint);
        }

        public void Listen(int backlog)
        {
            active.Exchange(true);
            listener.Listen(backlog);
        }

        public void StartAccept(SocketAsyncEventArgs e)
        {
            if (e == null)
                e = new SocketAsyncEventArgs();
            e.AcceptSocket = null;
            e.Completed -= EndAccept;
            e.Completed += EndAccept;
            if (active.State)
            {
                if (!listener.AcceptAsync(e))
                    EndAccept(null, e);
            }
        }

        private void EndAccept(object sender, SocketAsyncEventArgs e)
        {
            if (active.State && e.SocketError == SocketError.Success)
            {
                OnSocketConnected(e);
            }
        }

        public void Close()
        {
            if(active.CompareExchange(false, true))
            {
                OnClosing(EventArgs.Empty);
                listener.Close();
                OnClosed(EventArgs.Empty);
            }
        }

        protected virtual void OnSocketConnected(SocketAsyncEventArgs args)
        {
            EventHandler<SocketAsyncEventArgs> handler = Accepted;
            Accepted?.Invoke(this, args);
        }

        private void OnClosed(EventArgs args)
        {
            EventHandler handler = Closed;
            handler?.Invoke(this, args);
        }

        private void OnClosing(EventArgs args)
        {
            EventHandler handler = Closing;
            handler?.Invoke(this, args);
        }
    }
}
