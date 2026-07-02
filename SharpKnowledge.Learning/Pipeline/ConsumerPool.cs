using SharpKnowledge.Knowledge;
using SharpKnowledge.Learning.Teachers;

namespace SharpKnowledge.Learning.Pipeline
{
    public class ConsumerPool
    {
        private readonly Thread[] _threads;
        private readonly BrainPipeline _pipeline;
        private readonly BaseTeacher _teacher;
        private volatile bool _running;

        public ConsumerPool(BrainPipeline pipeline, BaseTeacher teacher, int threadCount = 32)
        {
            _pipeline = pipeline;
            _teacher = teacher;
            _threads = new Thread[threadCount];
        }

        public void Start()
        {
            _running = true;
            for (int i = 0; i < _threads.Length; i++)
            {
                _threads[i] = new Thread(ConsumerLoop);
                _threads[i].IsBackground = true;
                _threads[i].Name = $"Consumer-{i}";
                _threads[i].Start();
            }
        }

        public void Stop()
        {
            _running = false;
            for (int i = 0; i < _threads.Length; i++)
            {
                _threads[i].Join();
            }
        }

        private void ConsumerLoop()
        {
            while (_running)
            {
                BaseBrain? brain = _pipeline.TryDequeueWork();
                if (brain != null)
                {
                    var game = _teacher.InitializeNewGame();
                    var student = new Student(brain, game);
                    student.PlayUntilGameOver();
                    _pipeline.SubmitResult(brain);
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}
