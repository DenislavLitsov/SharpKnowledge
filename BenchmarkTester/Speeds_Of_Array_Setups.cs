using System.Diagnostics;

namespace BenchmarkTester
{
    // Conclusions:
    // 1. Single dimension arrays are faster than two-dimensional arrays.
    // 2. Two-dimensional arrays represented in a single dimension array are almost as fast as single dimension arrays.
    // 3. Lists are significantly slower than arrays, especially for two-dimensional structures.
    // 4. Using for loops is ever so slightly slower than foreach loops for these benchmarks. !!WILD!!
    // 5. Float is faster than double with around 10%.
    // 6. Jagged arrays are the fastest for two-dimensional structures, even faster than two-dimensional arrays.

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SingleDimensionArrayBenchmark1BilElements_Float()
        {
            const int totalElements = 1_000_000_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            float[] array = new float[totalElements];
            for (int i = 0; i < totalElements; i++)
            {
                array[i] = i;
            }

            float sum = 0;
            for (int i = 0; i < totalElements; i++)
            {
                sum += array[i];
            }

            stopwatch.Stop();
            Console.WriteLine($"Single Dimension Array: Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionArrayBenchmark1BilElements_Float()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            float[,] array = new float[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = j;
                }
            }

            float sum = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sum += array[i, j];
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension Array: Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionArrayRepresentedInOneDimessionArrayBenchmark1BilElements_Float()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            float[] array = new float[totalElements];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    array[index] = j;
                }
            }

            float sum = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    sum += array[index];
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"One Dimension Array (2D Logic): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void SingleDimensionArrayBenchmark1BilElements_Double()
        {
            const int totalElements = 1_000_000_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double[] array = new double[totalElements];
            for (int i = 0; i < totalElements; i++)
            {
                array[i] = i;
            }

            double sum = 0;
            for (int i = 0; i < totalElements; i++)
            {
                sum += array[i];
            }

            stopwatch.Stop();
            Console.WriteLine($"Single Dimension Array (double): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionArrayBenchmark1BilElements_Double()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double[,] array = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = j;
                }
            }

            double sum = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sum += array[i, j];
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension Array (double): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionArrayRepresentedInOneDimessionArrayBenchmark1BilElements_Double()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double[] array = new double[totalElements];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    array[index] = j;
                }
            }

            double sum = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    sum += array[index];
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"One Dimension Array (2D Logic, double): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void SingleDimensionListBenchmark1BilElements_Float()
        {
            const int totalElements = 1_000_000_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<float>(totalElements);
            for (int i = 0; i < totalElements; i++)
            {
                list.Add(i);
            }

            float sum = 0;
            for (int i = 0; i < totalElements; i++)
            {
                sum += list[i];
            }

            stopwatch.Stop();
            Console.WriteLine($"Single Dimension List: Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionListBenchmark1BilElements_Float()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<List<float>>(rows);
            for (int i = 0; i < rows; i++)
            {
                var rowList = new List<float>(cols);
                for (int j = 0; j < cols; j++)
                {
                    rowList.Add(j);
                }
                list.Add(rowList);
            }

            float sum = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sum += list[i][j];
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension List: Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionListRepresentedInOneDimensionListBenchmark1BilElements_Float()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<float>(totalElements);
            for (int i = 0; i < totalElements; i++)
            {
                list.Add(0);
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    list[index] = j;
                }
            }

            float sum = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    sum += list[index];
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"One Dimension List (2D Logic): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void SingleDimensionListBenchmark1BilElements_Double()
        {
            const int totalElements = 1_000_000_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<double>(totalElements);
            for (int i = 0; i < totalElements; i++)
            {
                list.Add(i);
            }

            double sum = 0;
            for (int i = 0; i < totalElements; i++)
            {
                sum += list[i];
            }

            stopwatch.Stop();
            Console.WriteLine($"Single Dimension List (double): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionListBenchmark1BilElements_Double()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<List<double>>(rows);
            for (int i = 0; i < rows; i++)
            {
                var rowList = new List<double>(cols);
                for (int j = 0; j < cols; j++)
                {
                    rowList.Add(j);
                }
                list.Add(rowList);
            }

            double sum = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sum += list[i][j];
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension List (double): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionListRepresentedInOneDimensionListBenchmark1BilElements_Double()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<double>(totalElements);
            for (int i = 0; i < totalElements; i++)
            {
                list.Add(0);
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    list[index] = j;
                }
            }

            double sum = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    sum += list[index];
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"One Dimension List (2D Logic, double): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void SingleDimensionArrayBenchmark1BilElements_Float_Foreach()
        {
            const int totalElements = 1_000_000_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            float[] array = new float[totalElements];
            for (int i = 0; i < totalElements; i++)
            {
                array[i] = i;
            }

            float sum = 0;
            foreach (var value in array)
            {
                sum += value;
            }

            stopwatch.Stop();
            Console.WriteLine($"Single Dimension Array (foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionArrayBenchmark1BilElements_Float_Foreach()
        {
            const int rows = 100_000;
            const int cols = 10_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            float[,] array = new float[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = j;
                }
            }

            float sum = 0;
            foreach (var value in array)
            {
                sum += value;
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension Array (foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionArrayRepresentedInOneDimessionArrayBenchmark1BilElements_Float_Foreach()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            float[] array = new float[totalElements];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    array[index] = j;
                }
            }

            float sum = 0;
            foreach (var value in array)
            {
                sum += value;
            }

            stopwatch.Stop();
            Console.WriteLine($"One Dimension Array (2D Logic, foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void SingleDimensionArrayBenchmark1BilElements_Double_Foreach()
        {
            const int totalElements = 1_000_000_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double[] array = new double[totalElements];
            for (int i = 0; i < totalElements; i++)
            {
                array[i] = i;
            }

            double sum = 0;
            foreach (var value in array)
            {
                sum += value;
            }

            stopwatch.Stop();
            Console.WriteLine($"Single Dimension Array (double, foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionArrayBenchmark1BilElements_Double_Foreach()
        {
            const int rows = 100_000;
            const int cols = 10_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double[,] array = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    array[i, j] = j;
                }
            }

            double sum = 0;
            foreach (var value in array)
            {
                sum += value;
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension Array (double, foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionArrayRepresentedInOneDimessionArrayBenchmark1BilElements_Double_Foreach()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double[] array = new double[totalElements];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    array[index] = j;
                }
            }

            double sum = 0;
            foreach (var value in array)
            {
                sum += value;
            }

            stopwatch.Stop();
            Console.WriteLine($"One Dimension Array (2D Logic, double, foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void SingleDimensionListBenchmark1BilElements_Float_Foreach()
        {
            const int totalElements = 1_000_000_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<float>(totalElements);
            for (int i = 0; i < totalElements; i++)
            {
                list.Add(i);
            }

            float sum = 0;
            foreach (var value in list)
            {
                sum += value;
            }

            stopwatch.Stop();
            Console.WriteLine($"Single Dimension List (foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionListBenchmark1BilElements_Float_Foreach()
        {
            const int rows = 100_000;
            const int cols = 10_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<List<float>>(rows);
            for (int i = 0; i < rows; i++)
            {
                var rowList = new List<float>(cols);
                for (int j = 0; j < cols; j++)
                {
                    rowList.Add(j);
                }
                list.Add(rowList);
            }

            float sum = 0;
            foreach (var rowList in list)
            {
                foreach (var value in rowList)
                {
                    sum += value;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension List (foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionListRepresentedInOneDimensionListBenchmark1BilElements_Float_Foreach()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<float>(totalElements);
            for (int i = 0; i < totalElements; i++)
            {
                list.Add(0);
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    list[index] = j;
                }
            }

            float sum = 0;
            foreach (var value in list)
            {
                sum += value;
            }

            stopwatch.Stop();
            Console.WriteLine($"One Dimension List (2D Logic, foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void SingleDimensionListBenchmark1BilElements_Double_Foreach()
        {
            const int totalElements = 1_000_000_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<double>(totalElements);
            for (int i = 0; i < totalElements; i++)
            {
                list.Add(i);
            }

            double sum = 0;
            foreach (var value in list)
            {
                sum += value;
            }

            stopwatch.Stop();
            Console.WriteLine($"Single Dimension List (double, foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionListBenchmark1BilElements_Double_Foreach()
        {
            const int rows = 100_000;
            const int cols = 10_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<List<double>>(rows);
            for (int i = 0; i < rows; i++)
            {
                var rowList = new List<double>(cols);
                for (int j = 0; j < cols; j++)
                {
                    rowList.Add(j);
                }
                list.Add(rowList);
            }

            double sum = 0;
            foreach (var rowList in list)
            {
                foreach (var value in rowList)
                {
                    sum += value;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension List (double, foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionListRepresentedInOneDimensionListBenchmark1BilElements_Double_Foreach()
        {
            const int rows = 100_000;
            const int cols = 10_000;
            const int totalElements = rows * cols;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var list = new List<double>(totalElements);
            for (int i = 0; i < totalElements; i++)
            {
                list.Add(0);
            }
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + j;
                    list[index] = j;
                }
            }

            double sum = 0;
            foreach (var value in list)
            {
                sum += value;
            }

            stopwatch.Stop();
            Console.WriteLine($"One Dimension List (2D Logic, double, foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionJaggedArrayBenchmark1BilElements_Float()
        {
            const int rows = 100_000;
            const int cols = 10_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            float[][] array = new float[rows][];
            for (int i = 0; i < rows; i++)
            {
                array[i] = new float[cols];
                for (int j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = j;
                }
            }

            float sum = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    sum += array[i][j];
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension Jagged Array: Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionJaggedArrayBenchmark1BilElements_Double()
        {
            const int rows = 100_000;
            const int cols = 10_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double[][] array = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                array[i] = new double[cols];
                for (int j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = j;
                }
            }

            double sum = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    sum += array[i][j];
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension Jagged Array (double): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionJaggedArrayBenchmark1BilElements_Float_Foreach()
        {
            const int rows = 100_000;
            const int cols = 10_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            float[][] array = new float[rows][];
            for (int i = 0; i < rows; i++)
            {
                array[i] = new float[cols];
                for (int j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = j;
                }
            }

            float sum = 0;
            foreach (var row in array)
            {
                foreach (var value in row)
                {
                    sum += value;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension Jagged Array (foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }

        [Test]
        public void TwoDimensionJaggedArrayBenchmark1BilElements_Double_Foreach()
        {
            const int rows = 100_000;
            const int cols = 10_000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            double[][] array = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                array[i] = new double[cols];
                for (int j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = j;
                }
            }

            double sum = 0;
            foreach (var row in array)
            {
                foreach (var value in row)
                {
                    sum += value;
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Two Dimension Jagged Array (double, foreach): Time taken = {stopwatch.ElapsedMilliseconds} ms, Sum = {sum}");
        }
    }
}
