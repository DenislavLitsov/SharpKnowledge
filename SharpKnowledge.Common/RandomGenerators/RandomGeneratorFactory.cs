using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Common.RandomGenerators
{
    public class RandomGeneratorFactory
    {
        private readonly bool isChached;
        private readonly int chachedSize;

        public RandomGeneratorFactory(bool isChached, int chachedSize)
        {
            this.isChached = isChached;
            this.chachedSize = chachedSize;
        }

        public IRandomGenerator GetRandomGenerator()
        {
            if (this.isChached)
            {
                return new CachedRandom(this.chachedSize);
            }
            else
            {
                return new RealRandom();
            }
        }
    }
}
