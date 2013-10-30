using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolver;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
//            var problem = new ParabolicProblem(new InputArguments {h = 0.1D, L = 1})
//                {
//                    m0 = (x) => 4*x*(1 - x),
//                    m1 = (t) => 0,
//                    m2 = (t) => 0,
//                    K = 1,
//                    f = (x, t) => 0
//                };
//            var solver = new ImplicitParabolicSolver(problem);
//            var l = solver.Solve(6);

            var problem = new HyperbolicProblem(new InputArguments() {h = 2.0/3.0, L = 2})
                {
                    fi0 = (t) => t,
                    fil = (t) => t + 2,
                    psi1 = (x) => x,
                    psi2 = (x) => 2*x,
                    f = (x, t) => 0,
                    a = 1.0
                };

            var solver = new ImplicitHyperbolicSolver(problem);
            var l = solver.Solve(10);

            for (int i = 0; i < l.X.Count; i++)
            {
                Console.WriteLine(l[i]);
            }
        }
    }
}
