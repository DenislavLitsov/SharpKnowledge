using ILGPU;
using ILGPU.Runtime;
using SharpKnowledge.Knowledge;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Learning.BrainManagers
{
    public class BrainEvolutioner
    {
        private Random _random = new Random();

        public BrainEvolutioner()
        {
        }

        public BaseBrain[] EvolveBrain(BaseBrain mainBrain, int newBrains, float mutationChance, float mutationMax)
        {
            BaseBrain[] brains = new BaseBrain[newBrains];

            Parallel.For(0, newBrains, () => new Random(), (i, state, localRandom) =>
            {
                brains[i] = GetEvolvedBrain(mainBrain, mutationChance, mutationMax, localRandom);
                brains[i].Generation = mainBrain.Generation + 1;
                return localRandom;
            }, _ => { });

            return brains;
        }

        public BaseBrain GetEvolvedBrain(BaseBrain brain, float mutationChance, float mutationMax, Random? random = null)
        {
            random ??= _random;
            BaseBrain newBrain = brain.Clone();

            for (int col = 0; col < newBrain.biases.Array.Length; col++)
            {
                for (int row = 0; row < newBrain.biases.Array[col].Length; row++)
                {
                    if (random.NextDouble() < mutationChance)
                    {
                        var prevValue = newBrain.biases.Get(row, col);
                        var newValue = MutateValue(random, prevValue, mutationMax, -10f, 10f);
                        newBrain.biases.Set(row, col, newValue);
                    }
                }
            }

            for (int col = 0; col < newBrain.weights.Array.Length; col++)
            {
                int length0 = newBrain.weights.Array[col].GetLength(0);
                int length1 = newBrain.weights.Array[col].GetLength(1);

                for (int row = 0; row < length0; row++)
                {
                    for (int k = 0; k < length1; k++)
                    {
                        if (random.NextDouble() < mutationChance)
                        {
                            var prevValue = newBrain.weights.Get(row, col, k);
                            var newValue = MutateValue(random, prevValue, mutationMax, -1f, 1f);
                            newBrain.weights.Set(row, col, k, newValue);
                        }
                    }
                }
            }

            return newBrain;
        }

        private float MutateValue(Random random, float prevValue, float mutationMax, float clampMin, float clampMax)
        {
            var procDifference = (float)(random.NextDouble() * mutationMax);
            var add = prevValue * procDifference;

            if (random.Next(0, 2) == 0)
            {
                add *= -1;
            }

            var newValue = prevValue + add;

            // When prevValue is 0, normal mutation does nothing (0 * anything = 0).
            // Generate a fresh random value to escape zero.
            if (prevValue == 0)
            {
                newValue = (float)random.NextDouble();
                if (random.Next(0, 2) == 0)
                {
                    newValue = -newValue;
                }
            }

            if (newValue > clampMax)
            {
                newValue = clampMax;
            }
            else if (newValue < clampMin)
            {
                newValue = clampMin;
            }

            return newValue;
        }
    }
}
