using SharpKnowledge.Knowledge;
using SharpKnowledge.Playing;
using System.Diagnostics;

namespace SharpKnowledge.Learning
{
    public class Student
    {
        private readonly BaseBrain brain;
        private readonly BaseGame baseGame;

        public BaseBrain Brain
        {
            get { return brain; }
        }

        public Student(BaseBrain brain, BaseGame baseGame)
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
            GameResult isGameOver = GameResult.Continue;
            int totalUpdates = 0;
            while (isGameOver == GameResult.Continue)
            {
                float[] inputs = this.baseGame.GetBrainInputs();
                float[] outputs = this.brain.CalculateOutputs(inputs);
                isGameOver = this.baseGame.Update(outputs);
                totalUpdates++;
            }

            this.brain.BestScore = this.baseGame.GetScore();
        }
    }
}
