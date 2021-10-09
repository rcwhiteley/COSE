using AccountService;
using AccountService.Net;
using Kernel.Net;
using Kernel.Net.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHost
{
    public class AuthenticationServiceHost
    {
        private ServiceContext context;

        public TcpServer server;
        public AuthenticationServiceHost()
        {
            Memory<byte> mem;
            server = new TcpServer();
            server.Bind(new System.Net.IPEndPoint(IPAddress.Any, 9958));
            server.Listen(100);
            server.Accepted += Server_Accepted;
            context = new ServiceContext("Account Server", null, new Kernel.Threading.WorkerPool(1), new DefaultMessageProvider<Client>(Assembly.GetAssembly(typeof(ServiceContext))));
            server.StartAccept(null);

        }

        private void Server_Accepted(object sender, System.Net.Sockets.SocketAsyncEventArgs e)
        {
            Console.WriteLine("Accepted");
            Client client = new Client(context, new TcpChannel(e.AcceptSocket));

        }
    }
}
