using Kernel.Workflow;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;

namespace Kernel.Threading
{
    //TODO: change class to use delegates instead of interfaces, maybe extend it to support both and keep legacy support?
    public class WorkerPool
    {
        private Channel<Action> channel = Channel.CreateUnbounded<Action>();

        public WorkerPool(int threadCount = 1)
        {
            for(int i = 0; i < threadCount; i++)
            {
                Thread worker = new Thread(Loop);
                worker.Start();
            }
        }

        private async void Loop(object obj)
        {
            while(true)
            {
                Action action = await channel.Reader.ReadAsync();
                action?.Invoke();
            }
        }

        public void Enqueue(Action action)
        {
            channel.Writer.TryWrite(action);
        }
    }
}
