using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using System.Diagnostics;

namespace BenchmarkTester
{

    [TestFixture]
    public class SpeedsOfBrainCalculations
    {
        [Test]
        public void CPUSpeedTest()
        {
            int[] columnsWithRows = { 400, 500, 100, 50, 4 };
            var factory = new NullBrainFactory(columnsWithRows);
            var brain = factory.GetBrain();

            float[] input = new float[columnsWithRows[0]];
            Random rnd = new Random();
            for (int i = 0; i < columnsWithRows[0]; i++)
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
        [Test]
        public void GPUSpeedTest()
        {
            Stopwatch gpuTester = new Stopwatch();
            gpuTester.Start();
            //GpuBrain.InitializeGpu();
            gpuTester.Stop();
            Console.WriteLine($"GPU Initializa: {gpuTester.ElapsedMilliseconds}");

            int[] columnsWithRows = { 400, 500, 100, 50, 4 };
            var factory = new NullBrainFactory(columnsWithRows);
            GpuBrain brain = factory.GetGpuBrain();

            float[] input = new float[columnsWithRows[0]];
            Random rnd = new Random();
            for (int i = 0; i < columnsWithRows[0]; i++)
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
