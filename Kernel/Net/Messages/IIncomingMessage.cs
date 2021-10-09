using Kernel.Workflow;
using System.Buffers;

namespace Kernel.Net.Messages
{
    public interface IIncomingMessage<T> : IProcessable, IMessage
    {
        T Owner { get; set; }
        void Deserialize(ReadOnlySequence<byte> memory);
    }
}
