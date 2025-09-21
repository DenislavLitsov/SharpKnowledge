using SharpKnowledge.Common;
using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using SharpKnowledge.Knowledge.IO;
using SharpKnowledge.Learning.BrainManagers;
using System.Diagnostics;

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
            int totalThreads = 500;

            long totalRuns = 0;
            SnakeTeacher teacher = new SnakeTeacher(new RandomGeneratorFactory(true, 10000));

            var latestModel = new IO<CpuBrain>().LoadLatest(StaticVariables.DataPath, "Snake");
            BaseBrain mainBrain;
            if (latestModel == null)
            {
                int[] columnsWithRows = { 400, 100, 50, 4 };
                var factory = new NullBrainFactory(columnsWithRows);
                mainBrain = factory.GetCpuBrain();
                mainBrain.BestScore = -20;
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

            var brains = brainEvolutioner.EvolveBrain(mainBrain, totalThreads, mutationChance, mutationStrength);
            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                stopwatch.Restart();
                Console.WriteLine("New study");

                if (mainBrain.BestScore == 0 || mainBrain.BestScore == 1 || mainBrain.BestScore == 50 || mainBrain.BestScore == 51)
                {
                    mutationChance = 0.20f;
                    mutationStrength = 1f;
                    Console.WriteLine("Super strong mutation");
                }
                else if (iterationsSenseLastBetterGeneration < 500)
                {
                    mutationChance = 0.1f;
                    mutationStrength = 0.05f;
                    Console.WriteLine("Weak mutation");
                }
                else if (iterationsSenseLastBetterGeneration < 1500)
                {
                    mutationChance = 0.20f;
                    mutationStrength = 0.25f;
                    Console.WriteLine("Moderate mutation");
                }
                else if (iterationsSenseLastBetterGeneration < 5000)
                {
                    mutationChance = 0.30f;
                    mutationStrength = 0.33f;
                    Console.WriteLine("Strong mutation");
                }
                else
                {
                    mutationChance = 0.50f;
                    mutationStrength = 1f;
                    Console.WriteLine("Super strong mutation");
                }

                Console.WriteLine($"Start evolving brain with best {mainBrain.BestScore} generation: {mainBrain.Generation} iterations since last better: {iterationsSenseLastBetterGeneration}");
                var threadedFunction = new ThreadedFunction<BaseBrain[]>();
                threadedFunction.Run(() =>
                {
                    var newBrains = brainEvolutioner.EvolveBrain(mainBrain, totalThreads, mutationChance, mutationStrength);
                    return newBrains;
                });

                Console.WriteLine("Start teaching");
                BaseBrain bestBrain = teacher.Teach(brains);
                Console.WriteLine("Finished teach");
                var cachedNewBrains = threadedFunction.WaitResult();
                Console.WriteLine("Finished evolving");

                Console.WriteLine($"Best score of class: {bestBrain.BestScore}");

                iterationsSenseLastBetterGeneration++;

                if (mainBrain.BestScore < bestBrain.BestScore || 
                    (mainBrain.BestScore == bestBrain.BestScore && iterationsSenseLastBetterGeneration > 10000))
                {
                    iterationsSenseLastBetterGeneration = 0;
                    mainBrain = bestBrain;
                    brains = brainEvolutioner.EvolveBrain(mainBrain, totalThreads, mutationChance, mutationStrength);
                    Console.WriteLine($"Best score: {mainBrain.BestScore}, Generation: {mainBrain.Generation}");

                    if (mainBrain.Generation % 1 == 0)
                    {
                        new IO<BaseBrain>().Save(mainBrain, totalRuns, $"Best brain with score {mainBrain.BestScore}", StaticVariables.DataPath, "Snake");
                    }
                }
                else
                {
                    brains = cachedNewBrains;
                }

                totalRuns++;
                stopwatch.Stop();
                Console.WriteLine($"Total runs: {totalRuns}. Last run lasted: {stopwatch.ElapsedMilliseconds}ms for total of {totalThreads} brains or {stopwatch.ElapsedMilliseconds / totalThreads}ms per brain");
                Console.WriteLine("--------");
            }
        }
    }
}