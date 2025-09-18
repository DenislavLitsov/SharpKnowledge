using SharpKnowledge.Knowledge;
using SharpKnowledge.Playing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Learning
{
    public abstract class BaseTeacher
    {
        private readonly int totalThreads;

        private Student[] students;

        public BaseTeacher(int totalThreads)
        {
            this.totalThreads = totalThreads;
        }

        public Brain Teach(Brain[] brains)
        {
            Thread[] threads = new Thread[this.totalThreads];
            this.students = new Student[this.totalThreads];

            for (int i = 0; i < this.totalThreads; i++)
            {
                this.students[i] = new Student(brains[i], this.InitializeNewGame());
                threads[i] = this.students[i].StartPlayingUntilGameOver();
            }

            for (int i = 0; i < this.totalThreads; i++)
            {
                threads[i].Join();
            }

            var bestBrain = this.students.OrderByDescending(s => s.Brain.BestScore).First().Brain;
            return bestBrain;
        }

        protected abstract BaseGame InitializeNewGame();
    }
}
