using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpKnowledge.Common
{
    public class QuickMaths
    {
        public static double Exp(double val)
        {
            long tmp = (long)(1512775 * val + 1072632447);
            return BitConverter.Int64BitsToDouble(tmp << 32);
        }

        public static float Sigmoid(double value)
        {
            double k = Exp(value);
            return (float)(k / (1.0f + k));
        }
    }
}
