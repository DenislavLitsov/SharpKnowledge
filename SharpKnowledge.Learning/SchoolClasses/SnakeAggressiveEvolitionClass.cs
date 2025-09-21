using SharpKnowledge.Common;
using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using SharpKnowledge.Knowledge.IO;
using SharpKnowledge.Learning.BrainManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Learning.SchoolClasses
{
    public class SnakeAggressiveEvolitionClass : BaseSchoolClass
    {
        public SnakeAggressiveEvolitionClass(BaseTeacher teacher, BrainEvolutioner brainEvolutioner, CpuBrain initialBrain) : base("SnakeAggressiveEvolition", teacher, brainEvolutioner, initialBrain)
        {

        }

        protected override void MainRun()
        {
            int totalThreads = 15;
            long totalRuns = 0;
            SnakeTeacher teacher = new SnakeTeacher(new RandomGeneratorFactory(true, 10000));

            var latestModel = new IO<CpuBrain>().LoadLatest(StaticVariables.DataPath, this.className);
            BaseBrain mainBrain;
            if (latestModel == null)
            {
                int[] columnsWithRows = { 402, 500, 100, 50, 4 };
                var factory = new NullBrainFactory(columnsWithRows);
                mainBrain = factory.GetCpuBrain();
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

                if (mainBrain.BestScore == 1 || mainBrain.BestScore == 50 || mainBrain.BestScore == 51)
                {
                    mutationChance = 0.20f;
                    mutationStrength = 2.50f;
                    Console.WriteLine("Super strong mutation");
                }
                else if (iterationsSenseLastBetterGeneration < 100)
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
                else
                {
                    mutationChance = 0.20f;
                    mutationStrength = 2.50f;
                    Console.WriteLine("Super strong mutation");
                }

                var brains = this.brainEvolutioner.EvolveBrain(mainBrain, totalThreads, mutationChance, mutationStrength);
                Console.WriteLine("Evolved");
                Console.WriteLine($"Iterations since last mutation: {iterationsSenseLastBetterGeneration}");

                BaseBrain bestBrain = teacher.Teach(brains);
                iterationsSenseLastBetterGeneration++;

                if (mainBrain.BestScore < bestBrain.BestScore)
                {
                    iterationsSenseLastBetterGeneration = 0;
                    mainBrain = bestBrain;
                    Console.WriteLine($"Best score: {mainBrain.BestScore}, Generation: {mainBrain.Generation}");

                    if (mainBrain.Generation % 100 == 0)
                    {
                        new IO<BaseBrain>().Save(mainBrain, totalRuns, $"Best brain with score {mainBrain.BestScore}", StaticVariables.DataPath, this.className);
                        Console.WriteLine("Saved generation");
                    }
                }

                totalRuns++;
                Console.WriteLine($"Total runs: {totalRuns}");
            }
        }
    }
}
