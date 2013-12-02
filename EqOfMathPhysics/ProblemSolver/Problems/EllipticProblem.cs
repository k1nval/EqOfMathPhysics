namespace ProblemSolver.Problems
{
    using System;

    public class EllipticProblem : IProblem
    {
        public EllipticProblem(InputArguments arguments)
        {
            H = arguments.H;
            L = arguments.L;
            M = arguments.M;
        }

        public double H { get; set; }

        public double L { get; set; }

        public double M { get; set; }

        // fi
        public Func<double, double, double> fi { get; set; }
    }
}
