using ILGPU.IR;
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
            CpuBrain brain = factory.GetCpuBrain();
            brain.Generation = 10;

            string gameName = "testerGame";
            var IO = new IO();
            IO.DeleteAllBrains(gameName);

            IO.Save(brain, 1, "Test", gameName);

            var res = new IO().LoadLatestCpuBrain("testerGame").cpuBrain;

            Assert.That(res.Generation == 10);

            Assert.That(res.nodes.Array[0].Length == 10);
            Assert.That(res.nodes.Array[1].Length == 50);
            Assert.That(res.nodes.Array[2].Length == 4);

            Assert.That(res.biases.Array[0].Length == 10);
            Assert.That(res.biases.Array[1].Length == 50);
            Assert.That(res.biases.Array[2].Length == 4);

            Assert.That(res.weights.Array[0].GetLength(1) == 50);
            Assert.That(res.weights.Array[1].GetLength(1) == 4);
        }

        [Test]
        public void SaveAndLoadBrainTestOutput()
        {
            string gameName = "testerGame";
            var IO = new IO();
            IO.DeleteAllBrains(gameName);

            int[] columnsWithRows = { 2, 3, 1 };
            var factory = new NullBrainFactory(columnsWithRows);
            CpuBrain mainBrain = factory.GetCpuBrain();
            mainBrain.biases.Set(0, 2, 1f);

            float[] inputs = { 0.25f, 0.5f };

            float[] outputs = mainBrain.CalculateOutputs(inputs);
            Assert.That(outputs[0] == QuickMaths.Sigmoid(1f));

            mainBrain.biases.Set(0, 1, -1f);
            mainBrain.biases.Set(1, 1, 1f);
            mainBrain.biases.Set(2, 1, 1f);

            outputs = mainBrain.CalculateOutputs(inputs);
            Assert.That(outputs[0] == 0.734707236f);

            mainBrain.weights.Set(0, 1, 0, 0.5f);
            mainBrain.weights.Set(1, 1, 0, 0.1f);
            mainBrain.weights.Set(2, 1, 0, 0.1f);

            outputs = mainBrain.CalculateOutputs(inputs);
            Console.WriteLine(outputs[0]);
            Assert.That(outputs[0] == 0.78198f);

            mainBrain.weights.Set(0, 0, 0, 0.5f);
            mainBrain.weights.Set(1, 0, 0, 0.1f);

            mainBrain.weights.Set(0, 0, 1, 0.5f);
            mainBrain.weights.Set(1, 0, 1, 0.1f);

            mainBrain.weights.Set(0, 0, 2, 0.5f);
            mainBrain.weights.Set(1, 0, 2, 0.1f);

            outputs = mainBrain.CalculateOutputs(inputs);
            Console.WriteLine(outputs[0]);
            Assert.That(outputs[0] == 0.784987032f);

            var outputs2 = mainBrain.CalculateOutputs(inputs);
            var outputs3 = mainBrain.CalculateOutputs(inputs);

            Assert.That(outputs[0] == outputs2[0]);
            Assert.That(outputs[0] == outputs3[0]);
            
            IO.Save(mainBrain, 1, "Test", gameName);
            var loadedBrain = new IO().LoadLatestCpuBrain("testerGame").cpuBrain;

            var outputsCloned = loadedBrain.CalculateOutputs(inputs);
            Assert.That(outputs[0] == outputsCloned[0]);
        }
    }
}
