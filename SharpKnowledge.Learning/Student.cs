using SharpKnowledge.Knowledge;
using SharpKnowledge.Playing;

namespace SharpKnowledge.Learning
{
    public class Student
    {
        private readonly Brain brain;
        private readonly BaseGame baseGame;

        public Brain Brain
        {
            get { return brain; }
        }

        public Student(Brain brain, BaseGame baseGame)
        {
            this.brain = brain;
            this.baseGame = baseGame;
        }

        public Thread StartPlayingUntilGameOver()
        {
            Thread thread = new Thread(new ThreadStart(PlayUntilGameOver));
            thread.Start();
            return thread;
        }

        protected void PlayUntilGameOver()
        {
            this.baseGame.Initialize();
            bool isGameOver = false;
            int totalUpdates = 0;
            while (!isGameOver)
            {
                float[] inputs = this.baseGame.GetBrainInputs();
                float[] outputs = this.brain.CalculateOutputs(inputs);
                float decision = outputs[0]; // Assuming single output for decision making
                isGameOver = !this.baseGame.Update(decision);
                totalUpdates++;
            }

            this.brain.BestScore = this.baseGame.GetScore();
            Console.WriteLine(totalUpdates);
        }
    }
}
