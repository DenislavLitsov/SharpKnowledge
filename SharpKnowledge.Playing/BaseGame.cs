namespace SharpKnowledge.Playing
{
    public abstract class BaseGame
    {
        protected BaseGame()
        {
        }

        public abstract void Initialize();

        /// <summary>
        /// Returns if the game is over
        /// </summary>
        /// <param name="takenDecision"></param>
        /// <returns></returns>
        public abstract bool Update(float takenDecision);

        public abstract long GetScore();
    }
}
