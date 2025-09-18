using SharpKnowledge.Common;
using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using SharpKnowledge.Knowledge.IO;

namespace SharpKnowledge.Tester
{
    [TestFixture]
    public class IOTester
    {
        public IOTester()
        {
        }


        [Test]
        public void SaveAndLoadBrain()
        {
            int[] columnsWithRows = { 10, 50, 4 };
            var factory = new RandomBrainFactory(columnsWithRows);
            Brain brain = factory.GetBrain();
            brain.Generation = 10;

            new IO().Save(brain, "Test", StaticVariables.DataPath);

            var res = new IO().Load(10, StaticVariables.DataPath).Brain;

            Assert.That(res.Generation == 10);

            Assert.That(res.nodes.Array[0].Length == 10);
            Assert.That(res.nodes.Array[1].Length == 50);
            Assert.That(res.nodes.Array[2].Length == 4);
                        
            Assert.That(res.biases.Array[0].Length == 10);
            Assert.That(res.biases.Array[1].Length == 50);
            Assert.That(res.biases.Array[2].Length == 4);
                        
            Assert.That(res.weights.Array[0][0].Length == 50);
            Assert.That(res.weights.Array[1][0].Length == 4);
            Assert.That(res.weights.Array[2][0] == null);
        }
    }
}
