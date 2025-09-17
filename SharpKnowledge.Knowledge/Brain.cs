using SharpKnowledge.Knowledge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpKnowledge.Knowledge
{
    [JsonSerializable(typeof(Brain))]
    public class Brain
    {
        public readonly ThreeDArray weights;
        public readonly TwoDArray biases;

        public TwoDArray nodes;

        public int Generation;

        public Brain(ThreeDArray weights, TwoDArray biases, int generation = 0)
        {
            this.weights = weights;
            this.biases = biases;

            int[] totalNodes = new int[this.biases.TotalColumns];
            for (int i = 0; i < biases.TotalColumns; i++)
            {
                totalNodes[i] = biases.GetRows(i);
            }

            this.nodes = new TwoDArray(totalNodes);

            this.Generation = generation;
        }

        public float[] CalculateOutputs(float[] inputs)
        {
            this.SetInputs(inputs);

            for (int mainNodeCol = 1; mainNodeCol < this.nodes.TotalColumns; mainNodeCol++)
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

                    this.nodes.Set(mainNodeRow, mainNodeCol, calculatedValue);
                }
            }

            float[] result = this.nodes.GetLastCol();
            return result;
        }

        private void SetInputs(float[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                this.nodes.Set(i, 0, inputs[i]);
            }
        }
    }
}
