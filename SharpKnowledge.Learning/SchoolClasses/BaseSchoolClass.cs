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
    public abstract class BaseSchoolClass<BrainType> where BrainType : BaseBrain
    {
        protected readonly int learningThreads;
        protected readonly string className;
        protected readonly BaseTeacher teacher;
        protected readonly BrainEvolutioner brainEvolutioner;
        protected BaseBrain initialBrain;

        public bool StopToken = false;

        public BaseSchoolClass(string className, BaseTeacher teacher, BrainEvolutioner brainEvolutioner, BaseBrain initialBrain, int learningThreads)
        {
            this.className = className;
            this.teacher = teacher;
            this.brainEvolutioner = brainEvolutioner;
            this.initialBrain = initialBrain;
            this.learningThreads = learningThreads;
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
