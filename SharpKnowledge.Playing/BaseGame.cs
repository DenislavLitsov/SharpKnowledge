namespace SharpKnowledge.Playing
{
    public abstract class BaseGame
    {
        protected BaseGame()
        {
        }

        public abstract void Initialize();

        /// <summary>
        /// </summary>
        /// <param name="takenDecision"></param>
        /// <returns></returns>
        public abstract GameResult Update(float[] takenDecisions);

        public abstract float GetScore();

        public abstract float[] GetBrainInputs();
    }
}
