using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolver.Problems
{
    public class ConvDiffProblem
    {
        public double L { get; set; }

        public double a { get; set; }

        /// <summary>
        /// U(x, 0)
        /// </summary>
        public Func<double, double> U0;

        /// <summary>
        /// f(x, t)
        /// </summary>
        public Func<double, double, double> f;

        /// <summary>
        /// Условие на левом(a > 0) или правом (a ( 0) конце
        /// </summary>
        public Func<double, double> psi;

        public bool IsAgreed
        {
            get
            {
                return Check();
            }
        }

        public bool Check()
        {
            if (a < 0)
            {
                return Math.Abs(U0(L) - psi(0)) < 1E-10;
            }

            return Math.Abs(U0(0) - psi(0)) < 1E-10;
        }
    }
}
