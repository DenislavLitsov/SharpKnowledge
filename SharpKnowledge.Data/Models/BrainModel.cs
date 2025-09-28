
    using System.ComponentModel.DataAnnotations;
namespace SharpKnowledge.Data.Models
{
    public class BrainModel
    {
        public BrainModel()
        {
            this.Id = Guid.NewGuid();
            this.Time = DateTime.UtcNow;

            this.WeightsData = new HashSet<Weight>();
            this.BiasesData = new HashSet<Bias>();
        }

        [Key]
        public Guid Id { get; set; }
        public string? BrainType { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Time { get; set; }
        public long TotalRuns { get; set; }
        public int Generation { get; set; }
        public float BestScore { get; set; }

        //public float[][,] Weights { get; set; }
        //public float[,] Biases { get; set; }

        public ICollection<Weight> WeightsData { get; set; }

        public ICollection<Bias> BiasesData { get; set; }

        public float[][,] GetWeightsArray()
        {
            int layers = WeightsData.Count;
            if (layers == 0) return new float[0][,];

            float[][,] weightsArray = new float[layers][,];
            int index = 0;
            foreach (var weight in WeightsData)
            {
                var weights = weight.GetWeights();

                weightsArray[index] = weights;
                index++;
            }

            return weightsArray;
        }

        public void SetWeightsArray(float[][,] weightsArray)
        {
            WeightsData = new List<Weight>();
            foreach (var weightData in weightsArray)
            {
                var parsedWeightData = new List<WeightCol>();
                for (int i = 0; i < weightData.GetLength(0); i++)
                {
                    float[] row = new float[weightData.GetLength(1)];
                    for (int j = 0; j < weightData.GetLength(1); j++)
                    {
                        row[j] = weightData[i, j];
                    }
                    parsedWeightData.Add(new WeightCol { WeightData = row });
                }
                
                WeightsData.Add(new Weight { WeightData = parsedWeightData });
            }
        }

        public float[][] GetBiasesArray()
        {
            return BiasesData.Select(b => b.Biases).ToArray();
        }

        public void SetBiasesArray(float[][] biasesArray)
        {
            BiasesData = new List<Bias>();
            foreach (var biasData in biasesArray)
            {
                BiasesData.Add(new Bias { Biases = biasData });
            }
        }
    }
}