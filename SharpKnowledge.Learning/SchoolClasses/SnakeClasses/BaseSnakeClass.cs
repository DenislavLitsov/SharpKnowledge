using SharpKnowledge.Common;
using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Data.Models;
using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using SharpKnowledge.Knowledge.IO;
using SharpKnowledge.Learning.BrainManagers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Learning.SchoolClasses.SnakeClasses
{
    public abstract class BaseSnakeClass<BrainType> : BaseSchoolClass<BrainType> where BrainType : BaseBrain
    {
        protected long iterationsSenseLastBetterGeneration = 0;

        protected BrainModel loadedModel;

        public BaseSnakeClass(string className, BaseTeacher teacher, BrainEvolutioner brainEvolutioner, BaseBrain initialBrain, int learningThreads) : base(className, teacher, brainEvolutioner, initialBrain, learningThreads)
        {
        }

        protected abstract void LoadInitialBrain();

        protected abstract (float mutationChance, float mutationStrength) GetMutationStrength();

        protected override void MainRun()
        {
            long totalRuns = 0;

            LoadInitialBrain();
            totalRuns = loadedModel.TotalRuns;

            BaseBrain mainBrain = this.initialBrain;

            float mutationChance = 0.1f;
            float mutationStrength = 0.05f;

            var brains = brainEvolutioner.EvolveBrain(mainBrain, learningThreads, mutationChance, mutationStrength);
            Stopwatch stopwatch = new Stopwatch();
            while (true)
            {
                stopwatch.Restart();
                Console.WriteLine("New study");

                var mutationSettings = this.GetMutationStrength();

                mutationChance = mutationSettings.mutationChance;
                mutationStrength = mutationSettings.mutationStrength;

                Console.WriteLine($"Start evolving brain with best {mainBrain.BestScore} generation: {mainBrain.Generation} iterations since last better: {iterationsSenseLastBetterGeneration}");
                var threadedFunction = new ThreadedFunction<BaseBrain[]>();

                Console.WriteLine($"TotalChange: {mutationChance * mutationStrength * 100}% Mutation chance: {mutationChance}, strength: {mutationStrength}");
                threadedFunction.Run(() =>
                {
                    var newBrains = brainEvolutioner.EvolveBrain(mainBrain, learningThreads, mutationChance, mutationStrength);
                    return newBrains;
                });

                Console.WriteLine($"Start teaching brains total: {brains.Length}");
                BrainType bestBrain = (BrainType)teacher.Teach(brains);
                Console.WriteLine("Finished teach");
                var cachedNewBrains = threadedFunction.WaitResult();
                Console.WriteLine("Finished evolving");

                Console.WriteLine($"Best score of class: {bestBrain.BestScore}");

                iterationsSenseLastBetterGeneration++;

                if (mainBrain.BestScore < bestBrain.BestScore)
                // ||
                // (mainBrain.BestScore == bestBrain.BestScore && iterationsSenseLastBetterGeneration > 10000))
                {
                    iterationsSenseLastBetterGeneration = 0;
                    mainBrain = bestBrain;
                    brains = brainEvolutioner.EvolveBrain(mainBrain, learningThreads, mutationChance, mutationStrength);
                    Console.WriteLine($"Best score: {mainBrain.BestScore}, Generation: {mainBrain.Generation}");

                    if (mainBrain.Generation % 1 == 0)
                    {
                        this.SaveBrain(mainBrain, totalRuns, $"Best brain with score {mainBrain.BestScore}", className);
                    }
                }
                else
                {
                    brains = cachedNewBrains;
                }

                totalRuns++;
                stopwatch.Stop();
                //Console.WriteLine($"Total runs: {totalRuns}. Last run lasted: {stopwatch.ElapsedMilliseconds}ms for total of {totalThreads} brains or {stopwatch.ElapsedMilliseconds / totalThreads}ms per brain");
                Console.WriteLine("--------");
            }
        }

        protected void SaveBrain(BaseBrain brain, long totalRuns, string description, string gameName)
        {
            new IO().Save(brain, totalRuns, description, gameName);
        }
    }
}
