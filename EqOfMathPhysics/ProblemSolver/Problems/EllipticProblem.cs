namespace ProblemSolver.Problems
{
    using System;

    public class EllipticProblem
    {
        public EllipticProblem()
        {
        }

        // fi
        public Func<double, double, double> fi { get; set; }

        public double L { get;  set; }
        public double M { get;  set; }
    }
}
