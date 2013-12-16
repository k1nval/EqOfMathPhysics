namespace ProblemSolver.Problems
{
    using System;

    public class HyperbolicProblem : IProblem
    {
        public const double Eps = 1E-10;

        public double H { get; set; }

        public double L { get; set; }

        public double A { get; set; }

        public bool IsAgreed
        {
            get
            {
                return Check();
            }
        }

        public Func<double, double, double> f { get; set; }

        // U(0, t)
        public Func<double, double> fi0 { get; set; }

        // U(L, t)
        public Func<double, double> fil { get; set; }

        // U(x, 0)
        public Func<double, double> psi1 { get; set; }

        // dU(x, 0)/dt
        public Func<double, double> psi2 { get; set; }

        public bool Check()
        {
            if (Math.Abs(psi1(0) - fi0(0)) < Eps && Math.Abs(psi2(L) - fil(0)) < Eps)
            {
                return true;
            }

            return false;
        }
    }
}
