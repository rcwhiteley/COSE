using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Kernel.Workflow
{
    public interface IPipelineStep<T>
    {
        T Execute(T input, ref bool stopPipeline);
    }
}
