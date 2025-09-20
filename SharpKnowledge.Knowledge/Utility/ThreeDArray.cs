using System.Text.Json.Serialization;

namespace SharpKnowledge.Knowledge.Utility
{
    [JsonSerializable(typeof(ThreeDArray))]
    public class ThreeDArray
    {
        public float[][,] Array;

        // countsPerColumn: array of arrays, each describing the number of rows per column, and for each row, the number of elements (depth)
        //public ThreeDArray(int[,] countsPerColumn)
        //{
        //    int totalColumns = countsPerColumn.Length;
        //    Array = new float[totalColumns][,];
        //
        //    for (int col = 0; col < totalColumns; col++)
        //    {
        //        int totalRows = countsPerColumn.GetLength(0);
        //
        //        int depth = countsPerColumn[col, 0];
        //        Array[col] = new float[totalRows, depth];
        //    }
        //}

        public ThreeDArray(float[][,] array)
        {
            this.Array = array;
        }

        public ThreeDArray()
        {
        }

        public float Get(int row, int col, int depth)
        {
            return Array[col][row,depth];
        }

        public void Set(int row, int col, int depth, float value)
        {
            Array[col][row,depth] = value;
        }

        public void SetToAll(float value)
        {
            for (int col = 0; col < Array.Length; col++)
            {
                for (int row = 0; row < Array[col].Length; row++)
                {
                    for (int d = 0; d < Array[col].GetLength(1); d++)
                    {
                        Array[col][row,d] = value;
                    }
                }
            }
        }
    }
}