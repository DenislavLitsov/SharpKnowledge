using SharpKnowledge.Knowledge;
using SharpKnowledge.Playing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Learning
{
    public abstract class BaseTeacher
    {
        private Student[] students;

        public BaseTeacher()
        {
        }

        public BaseBrain Teach(BaseBrain[] brains)
        {
            Thread[] threads = new Thread[brains.Length];
            this.students = new Student[brains.Length];

            for (int i = 0; i < brains.Length; i++)
            {
                this.students[i] = new Student(brains[i], this.InitializeNewGame());
                threads[i] = this.students[i].StartPlayingUntilGameOver();
            }

            for (int i = 0; i < brains.Length; i++)
            {
                threads[i].Join();
            }


            var bestBrain = this.students.OrderByDescending(s => s.Brain.BestScore).First().Brain;
            return bestBrain;
        }

        protected abstract BaseGame InitializeNewGame();
    }
}
