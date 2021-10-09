namespace Kernel.Net.Messages
{
    public abstract class MessageProvider<T>
    {
        public abstract IIncomingMessage<T> Create(ushort messageType);
    }
}