using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Common
{
    public class ThreadedFunction<Result>
    {
        private Result result;
        private Thread thread;

        public ThreadedFunction()
        {
        }

        public void Run(Func<Result> func)
        {
            this.thread = new Thread(() => { this.result = func(); });
            this.thread.Start();
        }

        public Result WaitResult()
        {
            this.thread.Join();
            return this.result;
        }
    }
}
