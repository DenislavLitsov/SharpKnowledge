using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using SharpKnowledge.Learning.BrainManagers;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Tester
{
    [TestFixture]
    public class BrainTester
    {
        [Test]
        public void BrainCalculationsTester()
        {
            int[] columnsWithRows = { 2, 3, 1 };
            var factory = new NullBrainFactory(columnsWithRows);
            Brain mainBrain = factory.GetBrain();
            mainBrain.biases.Set(0, 2, 1f);

            float[] inputs = { 0.25f, 0.5f };

            float[] outputs = mainBrain.CalculateOutputs(inputs);
            Assert.That(outputs[0] == 1f);

            mainBrain.biases.Set(0, 1, -1f);
            mainBrain.biases.Set(1, 1, 1f);
            mainBrain.biases.Set(2, 1, 1f);

            outputs = mainBrain.CalculateOutputs(inputs);
            Assert.That(outputs[0] == 1f);

            mainBrain.weights.Set(0, 1, 0, 0.5f);
            mainBrain.weights.Set(1, 1, 0, 0.1f);
            mainBrain.weights.Set(2, 1, 0, 0.1f);

            outputs = mainBrain.CalculateOutputs(inputs);
            Console.WriteLine(outputs[0]);
            Assert.That(outputs[0].ToString("0.00") == "0.70");

            mainBrain.weights.Set(0, 0, 0, 0.5f);
            mainBrain.weights.Set(1, 0, 0, 0.1f);

            mainBrain.weights.Set(0, 0, 1, 0.5f);
            mainBrain.weights.Set(1, 0, 1, 0.1f);

            mainBrain.weights.Set(0, 0, 2, 0.5f);
            mainBrain.weights.Set(1, 0, 2, 0.1f);

            outputs = mainBrain.CalculateOutputs(inputs);
            Console.WriteLine(outputs[0]);
            Assert.That(outputs[0].ToString("0.0000") == "0.8225");

            var outputs2 = mainBrain.CalculateOutputs(inputs);
            var outputs3 = mainBrain.CalculateOutputs(inputs);

            Assert.That(outputs[0] == outputs2[0]);
            Assert.That(outputs[0] == outputs3[0]);

            var clonedBrain = mainBrain.Clone();
            var outputsCloned = clonedBrain.CalculateOutputs(inputs);
            Assert.That(outputs[0] == outputsCloned[0]);
        }
    }
}
