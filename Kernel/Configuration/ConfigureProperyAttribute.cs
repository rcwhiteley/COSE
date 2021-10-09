using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Configuration
{
    class ConfigureProperyAttributeAttribute
    {
        public IEnumerable<string> PossibleValues { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
    }
}
