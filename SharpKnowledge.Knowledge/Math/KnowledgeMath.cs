using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Knowledge.Math
{
    public class KnowledgeMath
    {
        public static float Sigmoid(float value)
        {
            float k = (float)System.Math.Exp(value);
            return k / (1.0f + k);
        }
    }
}
