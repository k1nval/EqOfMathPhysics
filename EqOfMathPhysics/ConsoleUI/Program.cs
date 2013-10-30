namespace ConsoleUI
{
    using System;

    using ProblemSolver;
    using ProblemSolver.Problems;
    using ProblemSolver.Solvers;

    public class Program
    {
        public static void Main()
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

//            var problem = new HyperbolicProblem(new InputArguments { h = 3.14 / 4.0, L = 3.14 }) 
//            {       
//                fi0 = (t) => Math.Cos(t),
//                fil = (t) => t + 1,
//                psi1 = (x) => Math.Sin(x) + 1,
//                psi2 = (x) => x,
//                f = (x, t) => 0,
//                a = 12.0
//            };
//
//            var solver = new ExplicitHyperbolicSolver(problem);
//            var l = solver.Solve(0);
//
//            for (int i = 0; i < l.X.Count; i++)
//            {
//                Console.WriteLine(l[i]);
//            }

            var problem = new EllipticProblem(new InputArguments { h = 1.0 / 3.0, L = 1, M = 1 })
                              {
                                  fi = (x, y) => Math.Sin(Math.PI * x)
                              };

            var solver = new EllipticSolver(problem);
            var l = solver.Solve(3);
        }
    }
}
