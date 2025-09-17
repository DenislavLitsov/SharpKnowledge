using SharpKnowledge.Knowledge.Utility;

namespace SharpKnowledge.Knowledge.Factories
{
    public class RandomBrainFactory : IBrainFactory
    {
        private readonly float[][][] columnsWithRowsAndDepth;

        public RandomBrainFactory(int[] columnsWithRows)
        {
            this.columnsWithRowsAndDepth = new float[columnsWithRows.Length][][];
            for (int col = 0; col < columnsWithRows.Length; col++)
            {
                this.columnsWithRowsAndDepth[col] = new float[columnsWithRows[col]][];
                for (int row = 0; row < columnsWithRows[col]; row++)
                {
                    if (col == columnsWithRows.Length - 1)
                        continue;

                    this.columnsWithRowsAndDepth[col][row] = new float[columnsWithRows[col + 1]];
                }
            }
        }

        public Brain GetBrain()
        {
            Random rnd = new Random();

            // Initialize biases and nodes as TwoDArray<float>
            // Each column has countsPerColumn[col] rows
            int[] countsPerColumn = columnsWithRowsAndDepth.Select(col => col.Length).ToArray();
            TwoDArray biases = new TwoDArray(countsPerColumn);

            // Fill biases and nodes with random values
            for (int col = 0; col < biases.Array.Length; col++)
            {
                for (int row = 0; row < biases.Array[col].Length; row++)
                {
                    biases.Set(row, col, (float)(rnd.NextDouble() * 10));
                }
            }

            // Initialize weights as ThreeDArray<float>
            ThreeDArray weights = new ThreeDArray(columnsWithRowsAndDepth);

            // Fill weights with random values
            for (int col = 0; col < columnsWithRowsAndDepth.Length; col++)
            {
                for (int row = 0; row < columnsWithRowsAndDepth[col].Length; row++)
                {
                    if (col == columnsWithRowsAndDepth.Length - 1)
                        continue;

                    for (int depth = 0; depth < columnsWithRowsAndDepth[col][row].Length; depth++)
                    {
                        weights.Set(row, col, depth, (float)rnd.NextDouble());
                    }
                }
            }

            // Construct and return the Brain
            var brain = new Brain(weights, biases);
            return brain;
        }
    }
}
