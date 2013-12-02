namespace ProblemSolver.Problems
{
    using System;

    public class ParabolicProblem : IProblem
    {
        public ParabolicProblem(InputArguments inputArguments)
        {
            H = inputArguments.H;
            L = inputArguments.L;
        }

        public double H { get; set; }

        public double L { get; set; }

        public double K { get; set; }

        public Func<double, double, double> f { get; set; }

        //U(x, 0)
        public Func<double, double> m0 { get; set; }

        //U(0, t)
        public Func<double, double> m1 { get; set; }

        //U(L, t)
        public Func<double, double> m2 { get; set; }
    }
}
