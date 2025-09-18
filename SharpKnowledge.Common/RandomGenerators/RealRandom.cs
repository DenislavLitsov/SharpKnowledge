namespace SharpKnowledge.Common.RandomGenerators
{
    public class RealRandom : IRandomGenerator
    {
        private Random rnd = new Random();

        public double NextDouble()
        {
            return rnd.NextDouble();
        }
    }
}
