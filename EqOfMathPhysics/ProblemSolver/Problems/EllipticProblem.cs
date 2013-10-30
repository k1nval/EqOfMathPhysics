namespace ProblemSolver.Problems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EllipticProblem : IProblem
    {
        public EllipticProblem(InputArguments arguments)
        {
            h = arguments.h;
            L = arguments.L;
            M = arguments.M;
        }

        public double h { get; set; }

        public double L { get; set; }

        public double M { get; set; }

        // fi
        public Func<double, double, double> fi;
    }
}
