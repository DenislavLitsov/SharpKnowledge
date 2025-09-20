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
            var factory = new NullBrainFactory(columnsWithRows);
            Brain brain = factory.GetBrain();
            brain.Generation = 10;

            new IO().Save(brain, 1, "Test", StaticVariables.DataPath, "testGame");

            var res = new IO().Load(10, StaticVariables.DataPath, "testGame").Brain;

            Assert.That(res.Generation == 10);

            Assert.That(res.nodes.Array[0].Length == 10);
            Assert.That(res.nodes.Array[1].Length == 50);
            Assert.That(res.nodes.Array[2].Length == 4);
                        
            Assert.That(res.biases.Array[0].Length == 10);
            Assert.That(res.biases.Array[1].Length == 50);
            Assert.That(res.biases.Array[2].Length == 4);
                        
            Assert.That(res.weights.Array[0].GetLength(1) == 50);
            Assert.That(res.weights.Array[1].GetLength(1) == 4);
            Assert.That(res.weights.Array[2] == null);
        }
    }
}
