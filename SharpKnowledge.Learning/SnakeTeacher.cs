using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Games.Snake.Engine;
using SharpKnowledge.Playing;

namespace SharpKnowledge.Learning
{
    public class SnakeTeacher : BaseTeacher
    {
        private readonly RandomGeneratorFactory generatorFactory;

        public SnakeTeacher(RandomGeneratorFactory generatorFactory) : base()
        {
            this.generatorFactory = generatorFactory;
        }

        protected override BaseGame InitializeNewGame()
        {
            return new SnakeGame(20, 20, this.generatorFactory.GetRandomGenerator());
        }
    }
}
