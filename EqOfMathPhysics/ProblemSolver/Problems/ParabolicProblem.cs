namespace ProblemSolver.Problems
{
    using System;

    public class ParabolicProblem : IProblem
    {
        public double H { get; set; }

        public double L { get; set; }

        public bool IsAgreed {
            get
            {
                return Check();
            }
        }

        public double K { get; set; }

        public Func<double, double, double> f { get; set; }

        /// <summary>
        /// U(x, 0)
        /// </summary>

        public Func<double, double> m0 { get; set; }

        /// <summary>
        /// U(0, t)
        /// </summary>
        public Func<double, double> m1 { get; set; }

        /// <summary>
        /// U(L, t)
        /// </summary>
        public Func<double, double> m2 { get; set; }

        public bool Check()
        {
            if (Math.Abs(m0(0) - m1(0)) < 1E-10 && Math.Abs(m0(L) - m2(0)) < 1E-10)
            {
                return true;
            }

            return false;
        }
    }
}
