using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public class Analysis
    {
        public static int FindMaxPause(params DateTime[] data)
        {
            return new MaxPauseFinder().Analyze(data);
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            return new AverageDifferenceFinder().Analyze(data);
        }
    }
}
