using SharpKnowledge.Common;
using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using SharpKnowledge.Knowledge.IO;
using SharpKnowledge.Learning.BrainManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Learning
{
    public class School
    {
        BrainEvolutioner brainEvolutioner = new BrainEvolutioner();

        public School()
        {
        }

        public void RunSnake()
        {
            int totalThreads = 12;
            long totalRuns = 0;
            SnakeTeacher teacher = new SnakeTeacher(totalThreads, new RandomGeneratorFactory(true, 10000));

            var latestModel = new IO().LoadLatest(StaticVariables.DataPath, "Snake");
            Brain mainBrain;
            if (latestModel == null)
            {
                int[] columnsWithRows = { 10_020, 1000, 100, 50, 4 };
                var factory = new RandomBrainFactory(columnsWithRows);
                mainBrain = factory.GetBrain();
                Console.WriteLine("Created random initial brain");
            }
            else
            {
                mainBrain = latestModel.Brain;
                totalRuns = latestModel.TotalRuns;
                Console.WriteLine($"Loaded generation: {latestModel.Brain.Generation}, with description: {latestModel.Description}");
            }

            float mutationChance = 0.1f;
            float mutationStrength = 0.05f;
            long iterationsSenseLastBetterGeneration = 0;

            while (true)
            {
                Console.WriteLine("New study");

                if (iterationsSenseLastBetterGeneration < 100)
                {
                    mutationChance = 0.1f;
                    mutationStrength = 0.05f;
                    Console.WriteLine("Weak mutation");
                }
                else if (iterationsSenseLastBetterGeneration < 1000)
                {
                    mutationChance = 0.20f;
                    mutationStrength = 0.20f;
                    Console.WriteLine("Moderate mutation");
                }
                else if (iterationsSenseLastBetterGeneration < 10000)
                {
                    mutationChance = 0.20f;
                    mutationStrength = 0.50f;
                    Console.WriteLine("Strong mutation");
                }

                var brains = brainEvolutioner.EvolveBrain(mainBrain, totalThreads, mutationChance, mutationStrength);
                Console.WriteLine("Evolved");
                Console.WriteLine($"Iterations since last mutation: {iterationsSenseLastBetterGeneration}");

                Brain bestBrain = teacher.Teach(brains);
                iterationsSenseLastBetterGeneration++;

                if ((mainBrain.BestScore < bestBrain.BestScore && bestBrain.BestScore != 50 && bestBrain.BestScore != 51) ||
                    (mainBrain.BestScore == bestBrain.BestScore && totalRuns % 10 == 0))
                {
                    iterationsSenseLastBetterGeneration = 0;
                    mainBrain = bestBrain;
                    Console.WriteLine($"Best score: {mainBrain.BestScore}, Generation: {mainBrain.Generation}");

                    if (mainBrain.Generation % 100 == 0)
                    {
                        new IO().Save(mainBrain, totalRuns, $"Best brain with score {mainBrain.BestScore}", StaticVariables.DataPath, "Snake");
                        Console.WriteLine("Saved generation");
                    }
                }

                totalRuns++;
                Console.WriteLine($"Total runs: {totalRuns}");
            }
        }
    }
}