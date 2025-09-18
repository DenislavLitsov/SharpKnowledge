namespace SharpKnowledge.Knowledge.Utility
{
    public class TwoDArray
    {
        public float[][] Array;

        public int TotalColumns => this.Array.Length;

        public TwoDArray()
        {
        }

        public TwoDArray(int[] countsPerColumn)
        {
            int totalColumns = countsPerColumn.Length;
            this.Array = new float[totalColumns][];

            for (int i = 0; i < totalColumns; i++)
            {
                int rowsInColumn = countsPerColumn[i];
                this.Array[i] = new float[rowsInColumn];
            }
        }

        public TwoDArray(float[][] array)
        {
            this.Array = array;
        }

        public float Get(int row, int col)
        {
            return Array[col][row];
        }

        public void Set(int row, int col, float value)
        {
            Array[col][row] = value;
        }

        public int GetRows(int col)
        {
            return this.Array[col].Length;
        }

        public float[] GetLastCol()
        {
            return this.Array[this.Array.Length - 1];
        }

        public void SetToAll(float value)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                for (int j = 0; j < Array[i].Length; j++)
                {
                    Array[i][j] = value;
                }
            }
        }
    }
}