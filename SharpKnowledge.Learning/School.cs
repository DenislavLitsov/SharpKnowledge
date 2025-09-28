using SharpKnowledge.Common;
using SharpKnowledge.Common.RandomGenerators;
using SharpKnowledge.Knowledge;
using SharpKnowledge.Knowledge.Factories;
using SharpKnowledge.Knowledge.IO;
using SharpKnowledge.Learning.BrainManagers;
using SharpKnowledge.Learning.SchoolClasses;
using SharpKnowledge.Learning.SchoolClasses.SnakeClasses;
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
                2);

            cpuClasses.Add(snakeClass);
            
            List<Thread> threads = new List<Thread>();
            foreach (var schoolClass in cpuClasses)
            {
                threads.Add(schoolClass.StartClass());
            }

            threads.ForEach(t => t.Join());
        }
    }
}