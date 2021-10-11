namespace Kernel.Net.Messages
{
    public abstract class MessageProvider<T>
    {
        public abstract Message<T> Create(ushort messageType);
    }
}