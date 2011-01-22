using System;
using System.Collections.Generic;
using System.Threading;

namespace FunnelWeb.Tasks
{
    public class MyTask : ITask
    {
        public IEnumerable<TaskStep> Execute(Dictionary<string, object> properties)
        {
            for (var i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                if (i == 34)
                    throw new DivideByZeroException();
                yield return new TaskStep(i, "Step {0}", i);
            }
        }
    }
}