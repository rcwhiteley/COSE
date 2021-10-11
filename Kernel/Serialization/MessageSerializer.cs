using Kernel.Net.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Serialization
{
    public class SerializeAttribute : Attribute
    {
        public int FixedLength = 0;
        public int Order { get; set; }

        public SerializeAttribute(int order)
        {
            Order = order;
        }
    }
    public class MessageSerializer<T>
    {
        public Action<MessageWriter, T> Serialize { get; private set; }
        public Action<MessageReader, T> Deserialize { get; private set; }
        public Expression Dexpression;
        public MessageSerializer()
        {
            CreateSerializer();
            CreateDeserializer();
        }

        private IEnumerable<PropertyInfo> GetSortedProperties()
        {
            return typeof(T).GetProperties().Where(prop => GetSerializeAttribute(prop) != null).OrderBy(prop => GetSerializeAttribute(prop).Order);
        }

        private SerializeAttribute GetSerializeAttribute(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<SerializeAttribute>();
        }

        private void CreateSerializer()
        {
            var properties = GetSortedProperties();
            var writer = Expression.Parameter(typeof(MessageWriter), "writer");
            var msg = Expression.Parameter(typeof(T), "instance");

            List<Expression> expressions = new List<Expression>();
            foreach (var prop in properties)
            {
                var attr = GetSerializeAttribute(prop);
                if (prop.PropertyType == typeof(string) && attr.FixedLength != 0) // cstring
                {
                    MethodInfo method = typeof(MessageWriter).GetMethod("Write", new[] { typeof(string), typeof(int) });
                    var call = Expression.Call(writer, method, Expression.Property(msg, prop), Expression.Constant(attr.FixedLength));
                    expressions.Add(call);
                }
                else if (prop.PropertyType == typeof(byte[]) && attr.FixedLength != 0) // these ifs can be merged
                {
                    MethodInfo method = typeof(MessageWriter).GetMethod("Write", new[] { typeof(byte[]), typeof(int) });
                    var call = Expression.Call(writer, method, Expression.Property(msg, prop), Expression.Constant(attr.FixedLength));
                    expressions.Add(call);
                }
                else
                {
                    MethodInfo method;
                    Expression arg;
                    if (prop.PropertyType.IsEnum) // the propery is enum
                    {
                        Type underlyingType = Enum.GetUnderlyingType(prop.PropertyType);
                        arg = Expression.Convert(Expression.Property(msg, prop), underlyingType);
                        method = typeof(MessageWriter).GetMethod("Write", new[] { underlyingType });

                    }
                    else if (prop.PropertyType.GetInterface(typeof(IEnumerable<string>).Name, true) != null) // the propery is a string enumerable
                    {
                        arg = Expression.Property(msg, prop);
                        method = typeof(MessageWriter).GetMethod("Write", new[] { typeof(IEnumerable<string>) });
                    }
                    else // tries to find a writer method for the type
                    {
                        arg = Expression.Property(msg, prop);
                        method = typeof(MessageWriter).GetMethod("Write", new[] { prop.PropertyType });
                    }

                    if (method == null) throw new Exception("No Write method supports the type " + prop.PropertyType);

                    var call = Expression.Call(writer, method, arg);
                    expressions.Add(call);
                }


            }
            var body = Expression.Block(expressions);
            Serialize = Expression.Lambda<Action<MessageWriter, T>>(body, writer, msg).Compile();
        }

        private void CreateDeserializer()
        {
            var properties = GetSortedProperties();
            var reader = Expression.Parameter(typeof(MessageReader), "reader");
            var msg = Expression.Parameter(typeof(T), "instance");

            List<Expression> expressions = new List<Expression>();
            foreach (var prop in properties)
            {
                var attr = GetSerializeAttribute(prop);

                MethodInfo method;
                Expression propAccess = Expression.Property(msg, prop);
                if (prop.PropertyType.IsEnum) // the propery is enum
                {
                    Type underlyingType = Enum.GetUnderlyingType(prop.PropertyType);
                    method = typeof(MessageReader).GetMethods().First(method => method.ReturnType == underlyingType);
                    if (method == null) throw new Exception();
                    var readCall = Expression.Call(reader, method);
                    var conversion = Expression.Convert(readCall, underlyingType);
                    var assignation = Expression.Assign(propAccess, conversion);
                    expressions.Add(assignation);

                }
                else if (prop.PropertyType == typeof(string)) // the propery is a string
                {
                    Expression call;
                    if (attr.FixedLength == 0)
                    {
                        method = typeof(MessageReader).GetMethod("ReadString", Type.EmptyTypes);
                        if (method == null) throw new Exception();
                        call = Expression.Call(reader, method);
                    }
                    else
                    {
                        method = typeof(MessageReader).GetMethod("ReadString", new[] { typeof(int) });
                        if (method == null) throw new Exception();
                        call = Expression.Call(reader, method, Expression.Constant(attr.FixedLength));
                    }
                    var assignation = Expression.Assign(propAccess, call);
                    expressions.Add(assignation);
                }
                else if (prop.PropertyType == typeof(byte[])) // the propery is a string
                {
                    Expression call;
                    if (attr.FixedLength == 0)
                    {
                        method = typeof(MessageReader).GetMethod("ReadBytes", Type.EmptyTypes);
                        if (method == null) throw new Exception();
                        call = Expression.Call(reader, method);
                    }
                    else
                    {
                        method = typeof(MessageReader).GetMethod("ReadBytes", new[] { typeof(int) });
                        if (method == null) throw new Exception();
                        call = Expression.Call(reader, method, Expression.Constant(attr.FixedLength));
                    }
                    var assignation = Expression.Assign(propAccess, call);
                    expressions.Add(assignation);
                }
                else // tries to find a read method for the type
                {
                    method = typeof(MessageReader).GetMethods().First(method => method.ReturnType == prop.PropertyType);
                    if (method == null) throw new Exception();
                    var call = Expression.Call(reader, method);
                    var assignation = Expression.Assign(propAccess, call);
                    expressions.Add(assignation);
                }

                if (method == null) throw new Exception("No read method supports the type " + prop.PropertyType);
            }

            var body = Expression.Block(expressions);
            Deserialize = Expression.Lambda<Action<MessageReader, T>>(body, reader, msg).Compile();
        }
    }
}