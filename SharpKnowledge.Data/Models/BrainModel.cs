namespace SharpKnowledge.Data.Models
{
    public class BrainModel
    {
        public int Id { get; set; }
        public string BrainType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public long TotalRuns { get; set; }
        public int Generation { get; set; }
        public float BestScore { get; set; }
        public float[,,] Weights { get; set; }
        public float[,] Biases { get; set; }
    }
}