using SharpKnowledge.Common;
using SharpKnowledge.Knowledge.Math;
using SharpKnowledge.Knowledge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpKnowledge.Knowledge
{
    public class CpuBrain : BaseBrain
    {
        public CpuBrain(ThreeDArray weights, TwoDArray biases, int generation = 0, float bestScore = 0) :  base(weights, biases, generation, bestScore)
        {
        }

        protected override void CalculateColumn(int mainNodeCol)
        {
            int mainNodeTotalRows = this.nodes.GetRows(mainNodeCol);
            for (int mainNodeRow = 0; mainNodeRow < mainNodeTotalRows; mainNodeRow++)
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

        public override CpuBrain Clone()
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
            CpuBrain newBrain = new CpuBrain(newWeights, newBiases, this.Generation);
            return newBrain;
        }

    }
}
