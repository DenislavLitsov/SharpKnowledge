using System;

namespace SharpKnowledge.Data.Models;

public class Weight
{
    public int Id { get; set; }

    public IList<WeightCol> WeightData { get; set; }

    public Guid BrainModelId { get; set; }

    public float[,] GetWeights()
    {
        if (WeightData == null || WeightData.Count == 0) throw new InvalidOperationException("WeightData is null or empty.");

        int rows = WeightData.Count;
        int cols = WeightData[0].WeightData.Length;

        float[,] weightsArray = new float[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                weightsArray[i, j] = WeightData[i].WeightData[j];
            }
        }

        return weightsArray;
    }
}
