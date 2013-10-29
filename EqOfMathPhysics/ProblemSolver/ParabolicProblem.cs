using System;

namespace ProblemSolver
{
    public class ParabolicProblem : IProblem
    {
        public ParabolicProblem(InputArguments inputArguments)
        {
            h = inputArguments.h;
            L = inputArguments.L;
        }

        public double h { get; set; }

        public double L { get; set; }

        public double K;
        public Func<double, double, double> f;
        public Func<double, double> m0;
        public Func<double, double> m1;
        public Func<double, double> m2;
    }
}
