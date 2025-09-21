using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using SharpKnowledge.Learning.BrainManagers;

namespace SharpKnowledge.Tester.Factories
{
    [TestFixture]
    public class BrainFactoryTests
    {
        public BrainFactoryTests()
        {
        }

        [Test]
        public void RandomBrainFactory_CreatesBrainWithCorrectDimensions()
        {
            int[] columnsWithRows = { 10, 50, 4 };
            var factory = new NullBrainFactory(columnsWithRows);
            CpuBrain brain = factory.GetCpuBrain();

            Assert.That(brain.nodes.Array[0].Length == 10);
            Assert.That(brain.nodes.Array[1].Length == 50);
            Assert.That(brain.nodes.Array[2].Length == 4);

            Assert.That(brain.biases.Array[0].Length == 10);
            Assert.That(brain.biases.Array[1].Length == 50);
            Assert.That(brain.biases.Array[2].Length == 4);

            Assert.That(brain.weights.Array[0].GetLength(1) == 50);
            Assert.That(brain.weights.Array[1].GetLength(1) == 4);
        }

        [Test]
        public void RandomBrainFactory_BiasesAreRandomized()
        {
            int[] columnsWithRows = { 4, 50, 2 };
            var factory = new NullBrainFactory(columnsWithRows);
            BaseBrain mainBrain = factory.GetCpuBrain();
            BaseBrain brain = new BrainEvolutioner().GetEvolvedBrain(mainBrain, 1, 0.5f);

            bool foundDifferentBias = false;
            float firstBias = brain.biases.Get(0, 0);

            for (int col = 0; col < brain.biases.Array.Length; col++)
            {
                for (int row = 0; row < brain.biases.Array[col].Length; row++)
                {
                    if (brain.biases.Get(row, col) != firstBias) foundDifferentBias = true;
                }
            }

            Assert.That(foundDifferentBias, "Biases should be randomized.");
        }

        [Test]
        public void RandomBrainFactory_WeightsAreRandomized()
        {
            int[] columnsWithRows = { 2, 2 };
            var factory = new NullBrainFactory(columnsWithRows);
            BaseBrain brain = factory.GetCpuBrain();

            BrainEvolutioner evolutioner = new BrainEvolutioner();
            brain = evolutioner.EvolveBrain(brain, 1, 1f, 1f)[0];

            var weightsArray = brain.weights.Array;
            float firstWeight = weightsArray[0][0,0];
            bool foundDifferentWeight = false;

            for (int col = 0; col < weightsArray.Length; col++)
            {
                for (int row = 0; row < weightsArray[col].GetLength(0); row++)
                {
                    for (int depth = 0; depth < weightsArray[col].GetLength(1); depth++)
                    {
                        if (weightsArray[col][row,depth] != firstWeight)
                        {
                            foundDifferentWeight = true;
                            break;
                        }
                    }
                    if (foundDifferentWeight) break;
                }
                if (foundDifferentWeight) break;
            }

            Assert.That(foundDifferentWeight, "Weights should be randomized.");
        }
    }
}
