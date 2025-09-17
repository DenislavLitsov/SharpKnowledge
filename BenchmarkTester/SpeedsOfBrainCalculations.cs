using SharpKnowledge.Knowledge.Factories;
using System.Diagnostics;

namespace BenchmarkTester
{

    [TestFixture]
    public class SpeedsOfBrainCalculations
    {
        [Test]
        public void SpeedTest()
        {
            int[] columnsWithRows = { 10_020, 1000, 100, 50, 4 };
            var factory = new RandomBrainFactory(columnsWithRows);
            var brain = factory.GetBrain();

            float[] input = new float[10_020];
            Random rnd = new Random();
            for (int i = 0; i < 10_020; i++)
            {
                input[i] = (float)rnd.NextDouble();
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var output = brain.CalculateOutputs(input);

            stopwatch.Stop();
            var singleRunTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Single run: {singleRunTime}ms");


            stopwatch.Reset();
            stopwatch.Start();

            for (int i = 0; i < 100; i++)
            {
                var output2 = brain.CalculateOutputs(input);
            }

            stopwatch.Stop();
            var multipleRuns = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"100 runs: {multipleRuns}ms");

        }
    }
}
