using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Games.FlappyBird.Engine;
using SharpKnowledge.Games.Snake.Engine;
using SharpKnowledge.Playing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Learning.Teachers
{
    public class FlappyBirdTeacher : BaseTeacher
    {
        private readonly RandomGeneratorFactory generatorFactory;

        public FlappyBirdTeacher(RandomGeneratorFactory generatorFactory) : base()
        {
            this.generatorFactory = generatorFactory;
        }

        public override BaseGame InitializeNewGame()
        {
            return new FlappyBirdGame(this.generatorFactory.GetRandomGenerator());
        }
    }
}
