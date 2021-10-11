using Kernel.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Net.Messages
{
    public abstract class Message<TOwner>
    {
        public abstract ushort Type { get; set; }
        public abstract ushort Length { get; set; }
        public virtual void Process(TOwner owner) { }
        public abstract void Serialize(MessageWriter writer);
        public abstract void Deserialize(MessageReader reader);

    }
    public abstract class Message <TMessage, TOwner> : Message<TOwner> where TMessage : Message<TMessage, TOwner>
    {
        private static MessageSerializer<TMessage> serializer;

        private static MessageSerializer<TMessage> Serializer
        {
            get 
            { 
                if (serializer == null) serializer = new MessageSerializer<TMessage>();
                return serializer;
            }
        }
        

        public override void Serialize( MessageWriter writer)
        {
            Serializer.Serialize(writer, (TMessage)this);
        }

        public override void Deserialize(MessageReader reader)
        {
            Serializer.Deserialize(reader, (TMessage)this);
        }
    }
}
