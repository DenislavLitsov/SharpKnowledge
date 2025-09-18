using SharpKnowledge.Knowledge;
using System;
using System.Collections.Generic;
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

        public Brain[] EvolveBrain(Brain mainBrain, int newBrains, float mutationChance, float mutationMax)
        {
            Brain[] brains = new Brain[newBrains];

            for (int i = 0; i < newBrains; i++)
            {
                brains[i] = GetEvolvedBrain(mainBrain, mutationChance, mutationMax);
                brains[i].Generation = mainBrain.Generation + 1;
            }

            return brains;
        }

        public Brain GetEvolvedBrain(Brain brain, float mutationChance, float mutationMax)
        {
            Brain newBrain = brain.Clone();
            for (int col = 0; col < newBrain.biases.Array.Length; col++)
            {
                for (int row = 0; row < newBrain.biases.Array[col].Length; row++)
                {
                    if (_random.NextDouble() < mutationChance)
                    {
                        var prevValue = newBrain.biases.Get(row, col);
                        var procDifference = (float)(_random.NextDouble() * mutationMax);

                        var add = prevValue * procDifference;

                        var addOrRemove = _random.Next(0, 2);
                        if (addOrRemove == 0)
                        {
                            add *= -1;
                        }

                        var newValue = prevValue + add;

                        if (newValue > 10)
                        {
                            newValue = 10;
                        }
                        else if (newValue < -10)
                        {
                            newValue = -10;
                        }

                        newBrain.biases.Set(row, col, newValue);
                    }
                }
            }
            for (int col = 0; col < newBrain.weights.Array.Length - 1; col++)
            {
                for (int row = 0; row < newBrain.weights.Array[col].Length; row++)
                {
                    if (newBrain.weights.Array[col][row] == null) continue;
                    for (int k = 0; k < newBrain.weights.Array[col][row].Length; k++)
                    {
                        if (_random.NextDouble() < mutationChance)
                        {
                            var prevValue = newBrain.weights.Get(row, col, k);
                            var procDifference = (float)(_random.NextDouble() * mutationMax);

                            var add = prevValue * procDifference;

                            var addOrRemove = _random.Next(0, 2);
                            if (addOrRemove == 0)
                            {
                                add *= -1;
                            }

                            var newValue = prevValue + add;

                            if (prevValue == 0 && addOrRemove == 1)
                            {
                                newValue = (float)_random.NextDouble();
                            }

                            if (newValue > 1)
                            {
                                newValue = 1;
                            }
                            else if (newValue < 0)
                            {
                                newValue = 0;
                            }

                            newBrain.weights.Set(row, col, k, newValue);
                        }
                    }
                }
            }
            return newBrain;
        }
    }
}
