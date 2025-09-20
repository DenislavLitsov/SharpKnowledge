using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;
using SharpKnowledge.Common;
using SharpKnowledge.Knowledge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Knowledge
{
    public class GpuBrain : Brain
    {
        private static Context context;
        private static Accelerator accelerator;

        public GpuBrain(ThreeDArray weights, TwoDArray biases, int generation = 0) : base(weights, biases, generation)
        {
        }

        public static void InitializeGpu()
        {
            Context context = Context.CreateDefault();
            Accelerator accelerator = context.CreateCudaAccelerator(0);
        }

        protected override void CalculateColumn(int mainNodeCol)
        {
            //base.CalculateColumn(mainNodeCol);

            int mainNodeTotalRows = this.nodes.GetRows(mainNodeCol);
            for (int mainNodeRow = 0; mainNodeRow < mainNodeTotalRows; mainNodeRow++)
            {
                var colBiases = this.biases.Array[mainNodeCol];
                var colWeights = this.weights.Array[mainNodeCol - 1];

                MemoryBuffer1D<float, Stride1D.Dense> biasesOnDevice = accelerator.Allocate1D(colBiases);
                MemoryBuffer2D<float, Stride2D.DenseX> weightsOnDevice = accelerator.Allocate2DDenseX(colWeights);

            }
        }

        private void Kernel(Index1D mainNodeRow, int mainNodeCol, ArrayView<int> data, ArrayView<int> output)
        {
            int prevCol = mainNodeCol - 1;
            int prevTotalRows = this.biases.GetRows(prevCol);

            float bias = this.biases.Get(mainNodeRow, mainNodeCol);
            float calculatedValue = bias;

            for (int calcRow = 0; calcRow < prevTotalRows; calcRow++)
            {
                float prevNodeValue = this.nodes.Get(calcRow, prevCol);
                float weight = this.weights.Get(calcRow, prevCol, mainNodeRow);

                calculatedValue += prevNodeValue * weight;
            }

            float sigmoidedValue = QuickMaths.Sigmoid(calculatedValue);

            this.nodes.Set(mainNodeRow, mainNodeCol, sigmoidedValue);
        }
    }
}
