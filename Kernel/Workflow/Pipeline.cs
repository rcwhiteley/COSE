using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kernel.Workflow
{
    public class Pipeline<T>
    {
        private List<IPipelineStep<T>> steps = new List<IPipelineStep<T>>();

        public Pipeline<T> Register(IPipelineStep<T> step)
        {
            steps.Add(step);
            return this;
        }

        public T Process(T input)
        {
            bool stopPipeline = false;
            foreach (var step in steps)
            {
                if (stopPipeline)
                    break;
                input = step.Execute(input, ref stopPipeline);
            }
            return input;
        }

        public async Task<T> ProcessAsync(T input)
        {
            return await Task.Run<T>(() => Process(input));
        }
    }
}
