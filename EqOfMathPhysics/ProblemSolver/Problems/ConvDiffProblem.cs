using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolver.Problems
{
    class ConvDiffProblem
    {
        /// <summary>
        /// U(x, 0)
        /// </summary>
        public Func<double, double> U0;

        /// <summary>
        /// f(x, t)
        /// </summary>
        public Func<double, double, double> f;
    }
}
