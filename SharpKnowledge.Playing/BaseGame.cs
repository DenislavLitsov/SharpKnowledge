namespace SharpKnowledge.Playing
{
    public abstract class BaseGame
    {
        protected BaseGame()
        {
        }

        public abstract void Initialize();

        /// <summary>
        /// Returns false if game is over
        /// </summary>
        /// <param name="takenDecision"></param>
        /// <returns></returns>
        public abstract bool Update(float[] takenDecisions);

        public abstract float GetScore();

        public abstract float[] GetBrainInputs();
    }
}
