namespace SharpKnowledge.Common.RandomGenerators
{
    public class CachedRandom : IRandomGenerator
    {
        private static Dictionary<int, double[]> cachedNumbers = new Dictionary<int, double[]>();
        double[] _cachedNumbers;
        int currentIndex = 0;

        public CachedRandom(int totalCachedNumbers)
        {
            if (cachedNumbers.ContainsKey(totalCachedNumbers))
            {
                this._cachedNumbers = cachedNumbers[totalCachedNumbers];
            }
            else
            {
                var filePath = Path.Combine(StaticVariables.DataPath, "randoms", $"cachedRandoms_{totalCachedNumbers}.txt");

                if (!Directory.Exists(Path.Combine(StaticVariables.DataPath, "randoms")))
                {
                    Directory.CreateDirectory(Path.Combine(StaticVariables.DataPath, "randoms"));
                }

                if (File.Exists(filePath))
                {
                    double[] doubles = File
                        .ReadAllLines(filePath)
                        .Select(x => double.Parse(x))
                        .ToArray();

                    this._cachedNumbers = doubles;
                    cachedNumbers[totalCachedNumbers] = doubles;
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
                    cachedNumbers[totalCachedNumbers] = doubles;
                }
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
