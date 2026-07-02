using SharpKnowledge.Common;
using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Data.Models;
using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using SharpKnowledge.Knowledge.IO;
using SharpKnowledge.Learning.BrainManagers;
using SharpKnowledge.Learning.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Learning.SchoolClasses.SnakeClasses;

public class SnakeGpuRandomStrengthEvolutionBigNeuralNetwork: BaseSnakeClass<GpuBrain>
{
        Random Random = new Random();

        public SnakeGpuRandomStrengthEvolutionBigNeuralNetwork(BaseTeacher teacher, BrainEvolutioner brainEvolutioner, int learningThreads, int consumerCount = 50) : base("GPU_Snake_ 400, 4000, 4000, 200, 4", teacher, brainEvolutioner, null, learningThreads, consumerCount)
        {
            System.Console.WriteLine($"Create {nameof(SnakeCpuRandomStrengthEvolutionBigNeuralNetwork)} with {learningThreads} learning threads");
        }

        protected override (float mutationChance, float mutationStrength) GetMutationStrength()
        {
            float mutationChance;
            float mutationStrength;

            var rnd = Random.NextDouble();

            if (rnd < 0.2f)
            {
                mutationChance = 1f;
                mutationStrength = 1f;
                Console.WriteLine("MEGA mutation");
            }
            else if (rnd < 0.4f)
            {
                mutationChance = 0.50f;
                mutationStrength = 1f;
                Console.WriteLine("Super strong mutation");
            }
            else if (rnd < 0.6f)
            {
                mutationChance = 0.30f;
                mutationStrength = 0.33f;
                Console.WriteLine("Strong mutation");
            }
            else if (rnd < 0.8f)
            {
                mutationChance = 0.20f;
                mutationStrength = 0.25f;
                Console.WriteLine("Moderate mutation");
            }
            else
            {
                mutationChance = 0.1f;
                mutationStrength = 0.05f;
                Console.WriteLine("Weak mutation");
            }

            return (mutationChance, mutationStrength);
        }

        protected override void LoadInitialBrain()
        {
            var latestModel = new IO().LoadLatestGpuBrain(this.className);
            GpuBrain mainBrain;
            if (latestModel.gpuBrain == null)
            {
                int[] columnsWithRows = { 400, 4000, 4000, 200, 4 };
                var factory = new NullBrainFactory(columnsWithRows);
                mainBrain = factory.GetGpuBrain();
                mainBrain.BestScore = -20;
                Console.WriteLine("Created random initial brain");

                this.initialBrain = mainBrain;
                this.loadedModel = new BrainModel();
                this.loadedModel.Name = this.className;
            }
            else
            {
                this.loadedModel = latestModel.brainModel;
                this.initialBrain = latestModel.gpuBrain;

                Console.WriteLine($"Loaded generation: {latestModel.brainModel.Generation}, with description: {latestModel.brainModel.Description}");
            }
        }
}
