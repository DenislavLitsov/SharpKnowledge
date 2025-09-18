namespace SharpKnowledge.Common.RandomGenerators
{
    public interface IRandomGenerator
    {
        /// <summary>
        /// Returns a random double between 0.0 and 1.0
        /// </summary>
        /// <returns></returns>
        public double NextDouble();
    }
}
