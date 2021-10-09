﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Kernel.Net.Messages
{
    public class MessageAttribute : Attribute
    {
        public ushort MessageType { get; }

        public MessageAttribute(ushort messageType)
        {
            MessageType = messageType;
        }
    }
    public class DefaultMessageProvider<T> : MessageProvider<T>
    {
        Func<ushort, IIncomingMessage<T>> generator;
        
        public DefaultMessageProvider(Assembly assembly)
        {
            CreateCtors(assembly);
        }

        public override IIncomingMessage<T> Create(ushort messageType)
        {
            return generator.Invoke(messageType);
        }

        private void CreateCtors(Assembly asm)
        {
            var messageTypeArg = Expression.Parameter(typeof(ushort), "messageType");
           // var @switch = Expression.Switch(messageTypeArg);
            var result = Expression.Variable(typeof(IIncomingMessage<T>), "result");
            List<SwitchCase> switchCases = new List<SwitchCase>();
            var def = Expression.Assign(result, Expression.Constant(null, typeof(IIncomingMessage<T>)));
            var types = asm.GetTypes().Where(t => t.GetInterfaces().Any(baseType => baseType == typeof(IIncomingMessage<T>)));
            foreach (var type in types)
            {
                //var createMethod = type.BaseType.GetMethod("Create", BindingFlags.Public | BindingFlags.Static);
                MessageAttribute attribute = type.GetCustomAttribute<MessageAttribute>(false);
                if(attribute != null)
                {
                    Console.WriteLine("Found one for type: "+ attribute.MessageType);
                    var body = Expression.Assign(result, Expression.New(type));
                    switchCases.Add(Expression.SwitchCase(body, Expression.Constant(attribute.MessageType)));
                    
                }
            }
            var switchExpression = Expression.Switch(messageTypeArg, def, switchCases.ToArray());
            var ret = Expression.Assign(result, result);
            var methodBody = Expression.Block(new[] { result }, switchExpression, ret);
            generator = Expression.Lambda<Func<ushort, IIncomingMessage<T>>>(methodBody, messageTypeArg).Compile();

        }
    }
}
