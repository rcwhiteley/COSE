using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Net.Messages
{
    public interface IOutgoingMessage : IMessage
    {
        /// <summary>
        /// Serializes the message to the specified memory instance and returns the amount of bytes written
        /// </summary>
        /// <param name="memory"></param>
        /// <returns></returns>
        int Serialize(Memory<byte> memory);
    }
}
