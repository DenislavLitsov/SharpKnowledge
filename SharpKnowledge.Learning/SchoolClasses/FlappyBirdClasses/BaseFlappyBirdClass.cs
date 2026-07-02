using SharpKnowledge.Data.Models;
using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.IO;
using SharpKnowledge.Learning.BrainManagers;
using SharpKnowledge.Learning.Teachers;
using System.Diagnostics;

namespace SharpKnowledge.Learning.SchoolClasses.FlappyBirdClasses
{
    public abstract class BaseFlappyBirdClass<BrainType> : BaseSchoolClass<BrainType> where BrainType : BaseBrain
    {
        protected long iterationsSenseLastBetterGeneration = 0;

        protected BrainModel loadedModel;

        protected BaseFlappyBirdClass(string className, BaseTeacher teacher, BrainEvolutioner brainEvolutioner, BaseBrain initialBrain, int learningThreads, int consumerCount = 32) : base(className, teacher, brainEvolutioner, initialBrain, learningThreads, consumerCount)
        {
        }

        protected abstract void LoadInitialBrain();

        protected abstract (float mutationChance, float mutationStrength) GetMutationStrength();

        protected override void MainRun()
        {
            if (this.learningThreads == 0)
                return;

            long totalRuns = 0;

            LoadInitialBrain();
            totalRuns = loadedModel.TotalRuns;

            BaseBrain mainBrain = this.initialBrain;

            Stopwatch stopwatch = new Stopwatch();
            while (!StopToken)
            {
                stopwatch.Restart();
                Console.WriteLine("New study");

                var mutationSettings = this.GetMutationStrength();
                float mutationChance = mutationSettings.mutationChance;
                float mutationStrength = mutationSettings.mutationStrength;

                Console.WriteLine($"Start evolving brain with best {mainBrain.BestScore} generation: {mainBrain.Generation} iterations since last better: {iterationsSenseLastBetterGeneration}");
                Console.WriteLine($"TotalChange: {mutationChance * mutationStrength * 100}% Mutation chance: {mutationChance}, strength: {mutationStrength}");

                var brains = brainEvolutioner.EvolveBrain(mainBrain, learningThreads, mutationChance, mutationStrength);

                // Producer: push all brains into the pipeline
                pipeline.SetExpectedResults(brains.Length);
                for (int i = 0; i < brains.Length; i++)
                {
                    pipeline.EnqueueWork(brains[i]);
                }

                Console.WriteLine($"Start teaching brains total: {brains.Length}");

                // Wait for all consumers to finish scoring
                var results = pipeline.WaitForResults();

                Console.WriteLine("Finished teach");

                BrainType bestBrain = (BrainType)results.OrderByDescending(b => b.BestScore).First();

                Console.WriteLine($"Best score of class: {bestBrain.BestScore}");

                iterationsSenseLastBetterGeneration++;

                if (mainBrain.BestScore < bestBrain.BestScore)
                {
                    iterationsSenseLastBetterGeneration = 0;
                    mainBrain = bestBrain;
                    Console.WriteLine($"Best score: {mainBrain.BestScore}, Generation: {mainBrain.Generation}");

                    if (mainBrain.Generation % 1 == 0)
                    {
                        this.SaveBrain(mainBrain, totalRuns, $"Best brain with score {mainBrain.BestScore}", className);
                    }
                }
                else if (mainBrain.BestScore == bestBrain.BestScore && iterationsSenseLastBetterGeneration >= 200)
                {
                    iterationsSenseLastBetterGeneration = 0;
                    mainBrain = bestBrain;
                    Console.WriteLine($"Replaced brain");
                    Console.WriteLine($"Best score: {mainBrain.BestScore}, Generation: {mainBrain.Generation}");

                    if (mainBrain.Generation % 1 == 0)
                    {
                        this.SaveBrain(mainBrain, totalRuns, $"Replaced brain with: Best brain with score {mainBrain.BestScore}", className);
                    }
                }

                totalRuns++;
                stopwatch.Stop();
                Console.WriteLine("--------");
            }
        }

        protected void SaveBrain(BaseBrain brain, long totalRuns, string description, string gameName)
        {
            new IO().Save(brain, totalRuns, description, gameName);
        }
    }
}
