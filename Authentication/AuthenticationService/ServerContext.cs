using System;
using Kernel.Threading;
using Kernel.Net.Messages;
using AccountService.Net;
using WorldServiceContracts;

namespace AccountService
{
    public class ServiceContext
    {
        private static Serilog.ILogger log => Serilog.Log.ForContext<ServiceContext>();
        public MessageProvider<Client> MessageProvider { get; }
        public WorldService WorldServer { get; }
        public WorkerPool WorkerPool { get; }
        public string Name { get; set; }
        /// <summary>
        /// Creates a new instance of the <see cref="ServiceContext"/> class
        /// </summary>
        /// <param name="worldServer"></param>
        /// <param name="workerPool"></param>
        /// <param name="networkActions"></param>
        public ServiceContext(string name, WorldService worldServer, WorkerPool workerPool, MessageProvider<Client> messageProvider)
        {
            Name = name;
            MessageProvider = messageProvider;
            WorldServer = worldServer;
            WorkerPool = workerPool;
        }
    }
}