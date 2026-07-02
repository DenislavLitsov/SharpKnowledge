using SharpKnowledge.Knowledge;
using SharpKnowledge.Learning.BrainManagers;
using SharpKnowledge.Learning.Pipeline;
using SharpKnowledge.Learning.Teachers;

namespace SharpKnowledge.Learning.SchoolClasses
{
    public interface ISchoolClass
    {
        Thread StartClass();
        void StopClass();
    }

    public abstract class BaseSchoolClass<BrainType> : ISchoolClass where BrainType : BaseBrain
    {
        protected readonly int learningThreads;
        protected readonly string className;
        protected readonly BaseTeacher teacher;
        protected readonly BrainEvolutioner brainEvolutioner;
        protected readonly BrainPipeline pipeline;
        protected readonly ConsumerPool consumerPool;
        protected BaseBrain initialBrain;

        public bool StopToken = false;

        public BaseSchoolClass(string className, BaseTeacher teacher, BrainEvolutioner brainEvolutioner, BaseBrain initialBrain, int learningThreads, int consumerCount = 32)
        {
            this.className = className;
            this.teacher = teacher;
            this.brainEvolutioner = brainEvolutioner;
            this.initialBrain = initialBrain;
            this.learningThreads = learningThreads;
            this.pipeline = new BrainPipeline();
            this.consumerPool = new ConsumerPool(pipeline, teacher, consumerCount);
        }

        public Thread StartClass()
        {
            consumerPool.Start();
            Thread thread = new Thread(new ThreadStart(this.MainRun));
            thread.Start();
            return thread;
        }

        public void StopClass()
        {
            StopToken = true;
            consumerPool.Stop();
        }

        protected abstract void MainRun();
    }
}
