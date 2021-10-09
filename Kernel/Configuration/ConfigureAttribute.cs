using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Configuration
{
    public class ConfigureAttribute : Attribute
    {
        public string FileName { get; set; }
    }
}
