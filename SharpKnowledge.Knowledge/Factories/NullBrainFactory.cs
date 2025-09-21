using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;
using SharpKnowledge.Knowledge.Utility;

namespace SharpKnowledge.Knowledge.Factories
{
    public class NullBrainFactory : IBrainFactory
    {
        private readonly float[][,] columnsWithRowsAndDepth;

        public NullBrainFactory(int[] columnsWithRows)
        {
            this.columnsWithRowsAndDepth = new float[columnsWithRows.Length][,];
            for (int col = 0; col < columnsWithRows.Length; col++)
            {
                if (col == columnsWithRows.Length -1)
                {
                    this.columnsWithRowsAndDepth[col] = new float[columnsWithRows[col], 0];
                    continue;
                }

                this.columnsWithRowsAndDepth[col] = new float[columnsWithRows[col], columnsWithRows[col + 1]];
            }
        }

        public CpuBrain GetCpuBrain()
        {
            //Random rnd = new Random();

            // Initialize biases and nodes as TwoDArray<float>
            // Each column has countsPerColumn[col] rows
            int[] countsPerColumn = columnsWithRowsAndDepth.Select(col => col != null ? col.GetLength(0) : 0).ToArray();
            TwoDArray biases = new TwoDArray(countsPerColumn);

            // Fill biases and nodes with random values
            //for (int col = 0; col < biases.Array.Length; col++)
            //{
            //    for (int row = 0; row < biases.Array[col].Length; row++)
            //    {
            //        var bias = (float)(rnd.NextDouble() * 0.1);
            //        if ((int)(rnd.NextDouble() * 2) == 0)
            //        {
            //            bias = -bias;
            //        }
            //
            //        bias = 0; // Start with zero biases
            //
            //        biases.Set(row, col, bias);
            //    }
            //}

            // Initialize weights as ThreeDArray<float>
            ThreeDArray weights = new ThreeDArray(columnsWithRowsAndDepth);

            // Fill weights with random values
            // for (int col = 0; col < columnsWithRowsAndDepth.Length - 1; col++)
            // {
            //     for (int row = 0; row < columnsWithRowsAndDepth[col].GetLength(0); row++)
            //     {
            //         if (col == columnsWithRowsAndDepth.Length - 1)
            //             continue;
            //
            //         for (int depth = 0; depth < columnsWithRowsAndDepth[col].GetLength(1); depth++)
            //         {
            //             //weights.Set(row, col, depth, (float)rnd.NextDouble() * 0.005f);
            //             //weights.Set(row, col, depth, 0);
            //         }
            //     }
            // }

            // Construct and return the Brain
            var brain = new CpuBrain(weights, biases);
            return brain;
        }

        public GpuBrain GetGpuBrain()
        {
            int[] countsPerColumn = columnsWithRowsAndDepth.Select(col => col != null ? col.GetLength(0) : 0).ToArray();
            TwoDArray biases = new TwoDArray(countsPerColumn);

            ThreeDArray weights = new ThreeDArray(columnsWithRowsAndDepth);

            var brain = new GpuBrain(weights, biases);
            return brain;
        }
    }
}
