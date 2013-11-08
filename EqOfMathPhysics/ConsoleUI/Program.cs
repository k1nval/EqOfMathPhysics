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
            /*var problem = new ParabolicProblem(new InputArguments {h = 0.1D, L = 1})
                {
                    m0 = (x) => 4*x*(1 - x),
                    m1 = (t) => 0,
                    m2 = (t) => 0,
                    K = 1,
                    f = (x, t) => 0
                };
            var solver = new ImplicitParabolicSolver(problem);
            var s = solver.Solve(1);

            for (int i = 0; i < s.X.Count; i++)
            {
                Console.WriteLine(s[i]);
            }*/

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

            /*var hyperbolicProblem = new HyperbolicProblem(new InputArguments { h = Math.PI / 4.0, L = Math.PI})
                            {
                                a = 12.0,
                                f = (x, t) => 0,
                                fi0 = (t) => Math.Cos(t),
                                fil = (t) => t + 1,
                                psi1 = (x) => Math.Sin(x) + 1,
                                psi2 = (x) => x
                            };
                var explicitSolverhyper = new ExplicitHyperbolicSolver(hyperbolicProblem);
                var res = explicitSolverhyper.Solve(1);

                for (int i = 0; i < res.X.Count; i++)
                {
                    Console.WriteLine(res[i]);
                }*/
//
//            var parabolicProblem = new ParabolicProblem(new InputArguments { h = 0.001, L = 0.01})
//            {
//                m0 = (x) => Math.Sin(x),
//                m1 = (t) => Math.Sin(t),
//                m2 = (t) => t,
//                f = (x, t) => 0,
//                K = 20
//            };
            var problem = new HyperbolicProblem(new InputArguments {h = Math.PI / 18, L = Math.PI})
                {
                    fi0 = (t) => 0,
                    fil = (t) => 0,
                    psi1 = (x) => x * (Math.PI - x),
                    psi2 = (x) => 0,
                    f = (x, t) => 0,
                    a = 1
                };
            var explicitSolver = new ExplicitHyperbolicSolver(problem);
            var implicitSolver = new ImplicitHyperbolicSolver(problem);
            var expl = explicitSolver.Solve(100);
            var impl = implicitSolver.Solve(100);
            double maxFail = 0.0D;
            Console.WriteLine("Explicit:    Implicit:    Failure");
            for (int i = 0; i < expl.X.Count; i++)
            {
                Console.WriteLine("{0:F8} | {1:F8} | {2:F8}", expl[i], impl[i], Math.Abs(expl[i] - impl[i]));
                maxFail = Math.Max(maxFail, Math.Abs(expl[i] - impl[i]));
            }

            Console.WriteLine("\nMaximal fail = {0:F6}", maxFail);

            Console.ReadLine();
        }
    }
}
