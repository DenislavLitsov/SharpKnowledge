using System.Text.Json.Serialization;

namespace SharpKnowledge.Knowledge.Utility
{
    [JsonSerializable(typeof(ThreeDArray))]
    public class ThreeDArray
    {
        public float[][][] Array;

        // countsPerColumn: array of arrays, each describing the number of rows per column, and for each row, the number of elements (depth)
        public ThreeDArray(int[][] countsPerColumn)
        {
            int totalColumns = countsPerColumn.Length;
            Array = new float[totalColumns][][];

            for (int col = 0; col < totalColumns; col++)
            {
                int[] countsPerRow = countsPerColumn[col];
                int totalRows = countsPerRow.Length;
                Array[col] = new float[totalRows][];

                for (int row = 0; row < totalRows; row++)
                {
                    int depth = countsPerRow[row];
                    Array[col][row] = new float[depth];
                }
            }
        }

        public ThreeDArray(float[][][] array)
        {
            this.Array = array;
        }

        public ThreeDArray()
        {
        }

        public float Get(int row, int col, int depth)
        {
            return Array[col][row][depth];
        }

        public void Set(int row, int col, int depth, float value)
        {
            Array[col][row][depth] = value;
        }

        public void SetToAll(float value)
        {
            for (int col = 0; col < Array.Length; col++)
            {
                for (int row = 0; row < Array[col].Length; row++)
                {
                    for (int d = 0; d < Array[col][row].Length; d++)
                    {
                        Array[col][row][d] = value;
                    }
                }
            }
        }
    }
}