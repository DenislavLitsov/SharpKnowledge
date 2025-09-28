using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;
using SharpKnowledge.Common;
using SharpKnowledge.Knowledge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpKnowledge.Knowledge
{
    public class GpuBrain : BaseBrain
    {
        [JsonIgnore]
        public Context cudaContext;
        [JsonIgnore]
        public Accelerator cudaAccelerator;

        public GpuBrain(Context cudaContext, Accelerator cudaAccelerator, ThreeDArray weights, TwoDArray biases, int generation = 0) : base(weights, biases, generation)
        {
            this.cudaContext = cudaContext;
            this.cudaAccelerator = cudaAccelerator;
        }

        public GpuBrain(ThreeDArray weights, TwoDArray biases, int generation = 0) : base(weights, biases, generation)
        {
        }

        protected override void CalculateColumn(int mainNodeCol)
        {
            //base.CalculateColumn(mainNodeCol);
            int mainNodeTotalRows = this.nodes.GetRows(mainNodeCol);

            var colBiases = this.biases.Array[mainNodeCol];
            var colWeights = this.weights.Array[mainNodeCol - 1];
            var prevColNodes = this.nodes.Array[mainNodeCol - 1];

            MemoryBuffer1D<float, Stride1D.Dense> biasesOnDevice = cudaAccelerator.Allocate1D(colBiases);
            MemoryBuffer2D<float, Stride2D.DenseX> weightsOnDevice = cudaAccelerator.Allocate2DDenseX(colWeights);
            MemoryBuffer1D<float, Stride1D.Dense> prevNodes = cudaAccelerator.Allocate1D(prevColNodes);

            MemoryBuffer1D<float, Stride1D.Dense> deviceOutput = cudaAccelerator.Allocate1D<float>(mainNodeTotalRows);

            int prevCol = mainNodeCol - 1;
            int prevTotalRows = this.biases.GetRows(prevCol);

            Action<Index1D, int, int, ArrayView1D<float, Stride1D.Dense>, ArrayView1D<float, Stride1D.Dense>,
                ArrayView2D<float, Stride2D.DenseX>, ArrayView1D<float, Stride1D.Dense>> loadedCalcKernel =
                    cudaAccelerator.LoadAutoGroupedStreamKernel<Index1D, int, int, ArrayView1D<float, Stride1D.Dense>,
                        ArrayView1D<float, Stride1D.Dense>, ArrayView2D<float, Stride2D.DenseX>, ArrayView1D<float, Stride1D.Dense>>(Kernel);


            loadedCalcKernel(mainNodeTotalRows, mainNodeCol, prevTotalRows, prevNodes, biasesOnDevice, weightsOnDevice, deviceOutput);

            // wait for the accelerator to be finished with whatever it's doing
            // in this case it just waits for the kernel to finish.
            cudaAccelerator.Synchronize();

            float[] deviceReadOutputs = deviceOutput.GetAsArray1D();
            for (int i = 0; i < mainNodeTotalRows; i++)
            {
                this.nodes.Set(i, mainNodeCol, deviceReadOutputs[i]);
            }
        }

        private static void Kernel(Index1D mainNodeRow, int mainNodeCol, int prevTotalRows, ArrayView1D<float, Stride1D.Dense> prevNodes,
            ArrayView1D<float, Stride1D.Dense> biases, ArrayView2D<float, Stride2D.DenseX> weights, ArrayView1D<float, Stride1D.Dense> output)
        {
            int prevCol = mainNodeCol - 1;

            //float bias = this.biases.Get(mainNodeRow, mainNodeCol);
            float bias = biases[mainNodeRow];
            float calculatedValue = bias;

            for (int calcRow = 0; calcRow < prevTotalRows; calcRow++)
            {
                //float prevNodeValue = this.nodes.Get(calcRow, prevCol);
                float prevNodeValue = prevNodes[calcRow];

                //float weight = this.weights.Get(calcRow, prevCol, mainNodeRow);
                float weight = weights[calcRow, mainNodeRow];

                calculatedValue += prevNodeValue * weight;
            }

            //float sigmoidedValue = QuickMaths.Sigmoid(calculatedValue);

            //this.nodes.Set(mainNodeRow, mainNodeCol, calculatedValue);
            output[mainNodeRow] = calculatedValue;
        }

        public override GpuBrain Clone()
        {
            float[][,] newWeightsArray = new float[this.weights.Array.Length][,];
            for (int col = 0; col < this.weights.Array.Length; col++)
            {
                newWeightsArray[col] = new float[this.weights.Array[col].GetLength(0), this.weights.Array[col].GetLength(1)];
                Array.Copy(this.weights.Array[col], newWeightsArray[col], this.weights.Array[col].Length);
            }
            ThreeDArray newWeights = new ThreeDArray(newWeightsArray);
            float[][] newBiasesArray = new float[this.biases.Array.Length][];
            for (int col = 0; col < this.biases.Array.Length; col++)
            {
                newBiasesArray[col] = new float[this.biases.Array[col].Length];
                Array.Copy(this.biases.Array[col], newBiasesArray[col], this.biases.Array[col].Length);
            }
            TwoDArray newBiases = new TwoDArray(newBiasesArray);
            GpuBrain newBrain = new GpuBrain(cudaContext, cudaAccelerator, newWeights, newBiases, this.Generation);
            return newBrain;
        }
    }
}
