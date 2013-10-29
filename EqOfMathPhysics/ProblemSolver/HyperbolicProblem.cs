using System;

namespace ProblemSolver
{
    public class HyperbolicProblem : IProblem
    {
        public HyperbolicProblem(InputArguments inputArguments)
        {
            h = inputArguments.h;
            L = inputArguments.L;
        }

        public double h { get; set; }

        public double L { get; set; }

        public double a;

        public Func<double, double, double> f;
        //U(0, t)
        public Func<double, double> fi0;

        //U(L, t)
        public Func<double, double> fil;

        //U(x, 0)
        public Func<double, double> psi1;

        //dU(x, 0)/dt
        public Func<double, double> psi2;
    }
}
