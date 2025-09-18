using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Games.Snake.Engine;
using SharpKnowledge.Playing;

namespace SharpKnowledge.Learning
{
    public class SnakeTeacher : BaseTeacher
    {
        private readonly RandomGeneratorFactory generatorFactory;

        public SnakeTeacher(int totalThreads, RandomGeneratorFactory generatorFactory) : base(totalThreads)
        {
            this.generatorFactory = generatorFactory;
        }

        protected override BaseGame InitializeNewGame()
        {
            return new SnakeGame(100, 100, this.generatorFactory.GetRandomGenerator());
        }
    }
}
