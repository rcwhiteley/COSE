using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService
{
    public abstract class ServiceActor
    {
        public ServiceContext Context { get; set; }

        public ServiceActor(ServiceContext context)
        {
            Context = context;
        }
    }
}
