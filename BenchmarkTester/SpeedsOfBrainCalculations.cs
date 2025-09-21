using ILGPU;
using ILGPU.Runtime.Cuda;
using ILGPU.Runtime.OpenCL;
using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using System.Diagnostics;

namespace BenchmarkTester
{

    [TestFixture]
    public class SpeedsOfBrainCalculations
    {
        int[] columnsWithRows = { 4000, 1000, 50, 4 };
        int totalRuns = 100;

        [Test]
        public void CPUSpeedTest()
        {
            Console.WriteLine("Normal CPU");
            var factory = new NullBrainFactory(columnsWithRows);
            var brain = factory.GetCpuBrain();

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

            for (int i = 0; i < totalRuns; i++)
            {
                var output2 = brain.CalculateOutputs(input);
            }

            stopwatch.Stop();
            var multipleRuns = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"{totalRuns} runs: {multipleRuns}ms. Per run: {multipleRuns/ totalRuns}ms");

        }

        [Test]
        public void GPUSpeedTest()
        {
            Stopwatch gpuTester = new Stopwatch();
            gpuTester.Start();

            var factory = new NullBrainFactory(columnsWithRows);
            GpuBrain brain = factory.GetGpuBrain();

            var context = Context.CreateDefault();
            var accelerator = context.CreateCudaAccelerator(0);
            Console.WriteLine(accelerator);
            brain.cudaContext = context;
            brain.cudaAccelerator = accelerator;

            gpuTester.Stop();
            Console.WriteLine($"GPU Initializa: {gpuTester.ElapsedMilliseconds}");

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

            for (int i = 0; i < totalRuns; i++)
            {
                var output2 = brain.CalculateOutputs(input);
            }

            stopwatch.Stop();
            var multipleRuns = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"{totalRuns} runs: {multipleRuns}ms. Per run: {multipleRuns / totalRuns}ms");
        }

        [Test]
        public void IntegratedGPU()
        {
            Stopwatch gpuTester = new Stopwatch();
            gpuTester.Start();

            var factory = new NullBrainFactory(columnsWithRows);

            GpuBrain brain = factory.GetGpuBrain();

            var context = Context.CreateDefault();
            var accelerator = context.CreateCLAccelerator(0);
            Console.WriteLine(accelerator);
            brain.cudaContext = context;
            brain.cudaAccelerator = accelerator;

            gpuTester.Stop();
            Console.WriteLine($"GPU Initializa: {gpuTester.ElapsedMilliseconds}");

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

            for (int i = 0; i < totalRuns; i++)
            {
                var output2 = brain.CalculateOutputs(input);
            }

            stopwatch.Stop();
            var multipleRuns = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"{totalRuns} runs: {multipleRuns}ms. Per run: {multipleRuns / totalRuns}ms");
        }
    }
}
