namespace SharpKnowledge.Common.RandomGenerators
{
    public class CachedRandom : IRandomGenerator
    {
        double[] _cachedNumbers;
        int currentIndex = 0;

        public CachedRandom(int totalCachedNumbers)
        {
            var filePath = Path.Combine(StaticVariables.DataPath, "randoms", $"cachedRandoms_{totalCachedNumbers}.txt");
            if (File.Exists(filePath))
            {
                double[] doubles = File
                    .ReadAllLines(filePath)
                    .Select(x=>double.Parse(x))
                    .ToArray();

                this._cachedNumbers = doubles;
            }
            else
            {
                double[] doubles = new double[totalCachedNumbers];
                Random random = new Random();
                for (int i = 0; i < totalCachedNumbers; i++)
                {
                    doubles[i] = random.NextDouble();
                }

                File.WriteAllLines(filePath, doubles.Select(d => d.ToString()));
                this._cachedNumbers = doubles;
            }
        }

        public double NextDouble()
        {
            var number = this._cachedNumbers[currentIndex];
            currentIndex++;
            if (currentIndex >= this._cachedNumbers.Length)
            {
                currentIndex = 0;
            }
            return number;
        }
    }
}
