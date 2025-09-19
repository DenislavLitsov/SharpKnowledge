using SharpKnowledge.Knowledge;
using SharpKnowledge.Learning.BrainManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Learning.SchoolClasses
{
    public abstract class BaseSchoolClass
    {
        protected readonly string className;
        protected readonly BaseTeacher teacher;
        protected readonly BrainEvolutioner brainEvolutioner;
        protected readonly Brain initialBrain;

        public bool StopToken = false;

        public BaseSchoolClass(string className, BaseTeacher teacher, BrainEvolutioner brainEvolutioner, Brain initialBrain)
        {
            this.className = className;
            this.teacher = teacher;
            this.brainEvolutioner = brainEvolutioner;
            this.initialBrain = initialBrain;
        }

        public Thread StartClass()
        {
            Thread thread = new Thread(new ThreadStart(this.MainRun));
            thread.Start();
            return thread;
        }

        protected abstract void MainRun();
    }
}
