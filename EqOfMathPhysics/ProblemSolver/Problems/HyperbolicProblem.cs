namespace ProblemSolver.Problems
{
    using System;

    public class HyperbolicProblem : IProblem
    {
        public HyperbolicProblem(InputArguments inputArguments)
        {
            H = inputArguments.H;
            L = inputArguments.L;
        }

        public double H { get; set; }

        public double L { get; set; }

        public double A { get; set; }

        public Func<double, double, double> f { get; set; }

        // U(0, t)
        public Func<double, double> fi0 { get; set; }

        // U(L, t)
        public Func<double, double> fil { get; set; }

        // U(x, 0)
        public Func<double, double> psi1 { get; set; }

        // dU(x, 0)/dt
        public Func<double, double> psi2 { get; set; }
    }
}
