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

namespace SharpKnowledge.Learning.SchoolClasses.SnakeClasses
{
    public class SnakeCpuRandomStrengthEvolution : BaseSnakeClass<CpuBrain>
    {
        Random Random = new Random();
        public SnakeCpuRandomStrengthEvolution(BaseTeacher teacher, BrainEvolutioner brainEvolutioner, int learningThreads) : base("SnakeAggressiveEvolition_400_100_50_4", teacher, brainEvolutioner, null, learningThreads)
        {
        }

        protected override (float mutationChance, float mutationStrength) GetMutationStrength()
        {
            float mutationChance;
            float mutationStrength;

            var rnd = Random.NextDouble();

            if(rnd < 0.1f)
            {
                mutationChance = 0.50f;
                mutationStrength = 1f;
                Console.WriteLine("Super strong mutation");
            }
            else if (rnd < 0.3f)
            {
                mutationChance = 0.30f;
                mutationStrength = 0.33f;
                Console.WriteLine("Strong mutation");
            }
            else if (rnd < 0.6f)
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

        protected override SaveModel<CpuBrain> LoadInitialBrain()
        {
            var latestModel = new IO<CpuBrain>().LoadLatest(StaticVariables.DataPath, "Snake");
            CpuBrain mainBrain;
            if (latestModel == null)
            {
                int[] columnsWithRows = { 400, 100, 50, 4 };
                var factory = new NullBrainFactory(columnsWithRows);
                mainBrain = factory.GetCpuBrain();
                mainBrain.BestScore = -20;
                Console.WriteLine("Created random initial brain");

                return new SaveModel<CpuBrain>(mainBrain, "CPU BRAIN", 0);
            }
            else
            {
                Console.WriteLine($"Loaded generation: {latestModel.Brain.Generation}, with description: {latestModel.Description}");
                return latestModel;
            }
        }
    }
}
