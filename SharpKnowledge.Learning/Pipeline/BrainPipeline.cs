using SharpKnowledge.Knowledge;

namespace SharpKnowledge.Learning.Pipeline
{
    public class BrainPipeline
    {
        private readonly Queue<BaseBrain> _workQueue = new();
        private readonly object _workLock = new();

        private readonly List<BaseBrain> _results = new();
        private readonly object _resultLock = new();
        private int _expectedResults;

        public void SetExpectedResults(int count)
        {
            lock (_resultLock)
            {
                _expectedResults = count;
                _results.Clear();
            }
        }

        public void EnqueueWork(BaseBrain brain)
        {
            lock (_workLock)
            {
                _workQueue.Enqueue(brain);
            }
        }

        public BaseBrain? TryDequeueWork()
        {
            lock (_workLock)
            {
                if (_workQueue.Count > 0)
                {
                    return _workQueue.Dequeue();
                }
                return null;
            }
        }

        public void SubmitResult(BaseBrain brain)
        {
            lock (_resultLock)
            {
                _results.Add(brain);
                if (_results.Count >= _expectedResults)
                {
                    Monitor.PulseAll(_resultLock);
                }
            }
        }

        public BaseBrain[] WaitForResults()
        {
            lock (_resultLock)
            {
                while (_results.Count < _expectedResults)
                {
                    Monitor.Wait(_resultLock);
                }
                return _results.ToArray();
            }
        }
    }
}
