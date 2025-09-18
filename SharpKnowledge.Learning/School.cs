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
            SnakeTeacher teacher = new SnakeTeacher(10, new RandomGeneratorFactory(true, 10000));

            var latestModel = new IO().LoadLatest(StaticVariables.DataPath, "Snake");
            Brain mainBrain;
            if (latestModel == null)
            {
                int[] columnsWithRows = { 10_020, 1000, 100, 50, 4 };
                var factory = new RandomBrainFactory(columnsWithRows);
                mainBrain = factory.GetBrain();
            }
            else
            {
                mainBrain = latestModel.Brain;
            }

            int totalRuns = 0;
            while (true)
            {
                var brains = brainEvolutioner.EvolveBrain(mainBrain, 10, 0.1f, 0.5f);
                Brain bestBrain = teacher.Teach(brains);
                if (mainBrain.BestScore < bestBrain.BestScore && bestBrain.BestScore != 50 && bestBrain.BestScore != 51 && bestBrain.BestScore != 1)
                {
                    mainBrain = bestBrain;
                    Console.WriteLine($"Best score: {mainBrain.BestScore}, Generation: {mainBrain.Generation}");

                    if (mainBrain.Generation % 100 == 0)
                    {
                        new IO().Save(mainBrain, $"Best brain with score {mainBrain.BestScore}", StaticVariables.DataPath, "Snake");
                    }
                }

                totalRuns++;
                Console.WriteLine($"Total runs: {totalRuns}");
            }
        }
    }
}