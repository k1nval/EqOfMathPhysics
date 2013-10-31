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

            var parabolicProblem = new ParabolicProblem(new InputArguments { h = 1.0 / 3.0, L = 1.0 })
            {
                m0 = (x) => x,
                m1 = (t) => t,
                m2 = (t) => Math.Cos(2.0 * t),
                f = (x, t) => 0,
                K = 12.0
            };

            var explicitSolver = new ExplicitParabolicSolver(parabolicProblem);
            var implicitSolver = new ImplicitParabolicSolver(parabolicProblem);
            var l = explicitSolver.Solve(1);
            var d = implicitSolver.Solve(1);

            Console.WriteLine("Неявный:\n");
            for (int i = 0; i < d.X.Count; i++)
            {
                Console.WriteLine(d[i]);
            }

            Console.WriteLine("\nЯвный:\n");
            for (int i = 0; i < l.X.Count; i++)
            {
                Console.WriteLine(l[i]);
            }

            Console.ReadLine();
        }
    }
}
