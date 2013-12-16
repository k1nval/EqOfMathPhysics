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

        //U(x, 0)
        public Func<double, double> m0 { get; set; }

        //U(0, t)
        public Func<double, double> m1 { get; set; }

        //U(L, t)
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
