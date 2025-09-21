using SharpKnowledge.Knowledge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpKnowledge.Knowledge
{
    [JsonSerializable(typeof(BaseBrain))]
    public abstract class BaseBrain
    {
        public readonly ThreeDArray weights;
        public readonly TwoDArray biases;

        public TwoDArray nodes;

        public int Generation;

        public float BestScore = 0;

        public BaseBrain(ThreeDArray weights, TwoDArray biases, int generation = 0)
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
                this.CalculateColumn(mainNodeCol);
            }

            float[] result = this.nodes.GetLastCol();
            return result;
        }

        public abstract BaseBrain Clone();

        protected abstract void CalculateColumn(int mainNodeCol);

        private void SetInputs(float[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                this.nodes.Set(i, 0, inputs[i]);
            }
        }
    }
}
