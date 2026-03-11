using SharpKnowledge.Common;
using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using SharpKnowledge.Knowledge.IO;
using SharpKnowledge.Learning.BrainManagers;
using SharpKnowledge.Learning.SchoolClasses;
using SharpKnowledge.Learning.SchoolClasses.FlappyBirdClasses;
using SharpKnowledge.Learning.SchoolClasses.SnakeClasses;
using SharpKnowledge.Learning.Teachers;
using System.Diagnostics;

namespace SharpKnowledge.Learning
{
    public class School
    {
        BrainEvolutioner brainEvolutioner = new BrainEvolutioner();

        public School()
        {
        }

        public void StartLearning()
        {
            List<BaseSchoolClass<CpuBrain>> cpuClasses = new List<BaseSchoolClass<CpuBrain>>();
            var snakeClass = new SnakeCpuRandomStrengthEvolution(
                new SnakeTeacher(new RandomGeneratorFactory(true, 10000)), 
                brainEvolutioner, 
                EnvironmentManager.GetAggresiveCPUSnakeLearningTotalThreads());

            var snakeClass2 = new SnakeCpuRandomStrengthEvolutionBigNeuralNetwork(
                new SnakeTeacher(new RandomGeneratorFactory(true, 10000)), 
                brainEvolutioner, 
                EnvironmentManager.GetAggresiveCPUSnakeBigLearningTotalThreads());


            var flappyBird1 = new FlappyBirdCPURandomStrenthEvolution(
                new FlappyBirdTeacher(new RandomGeneratorFactory(true, 10000)),
                brainEvolutioner,
                EnvironmentManager.GetAggresiveCPUFlappyBirdBigLearningTotalThreads());

            //cpuClasses.Add(snakeClass);
            cpuClasses.Add(snakeClass2);
            //cpuClasses.Add(flappyBird1);

            List<Thread> threads = new List<Thread>();
            foreach (var schoolClass in cpuClasses)
            {
                threads.Add(schoolClass.StartClass());
            }

            threads.ForEach(t => t.Join());
        }
    }
}