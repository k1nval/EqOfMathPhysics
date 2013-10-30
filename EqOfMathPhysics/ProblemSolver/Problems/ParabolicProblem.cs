namespace ProblemSolver.Problems
{
    using System;

    public class ParabolicProblem : IProblem
    {
        public ParabolicProblem(InputArguments inputArguments)
        {
            this.h = inputArguments.h;
            this.L = inputArguments.L;
        }

        public double h { get; set; }

        public double L { get; set; }

        public double K;
        public Func<double, double, double> f;
        //U(x, 0)
        public Func<double, double> m0;

        //U(0, t)
        public Func<double, double> m1;

        //U(L, t)
        public Func<double, double> m2;
    }
}
