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
            /*var ellipticProblem = new EllipticProblem(new InputArguments { H = 1.0 / 3.0, L = 1.0, M = 1.0 })
                                      {
                                          fi = (x, y) => Math.Abs(y) < 1E-10 ? Math.Sin(Math.PI * x) : 0.00
                                      };

            var ellipticSolver = new EllipticSolver(ellipticProblem);
            var answer = ellipticSolver.Solve();
            var k = 0;

            for (int i = 0; i < answer.GetLength(0); i++)
            {
                Console.WriteLine("u{0}{1} = {2:F5}", i + 1, k + 1, answer[k++]);
            }

            */

            var hyperbolicProblem = new HyperbolicProblem()
                {
                    A = 1,
                    L = 3,
                    f = (x, t) => 0,
                    fi0 = (t) => Math.Sin(t),
                    fil = (t) => t + 6.0,
                    psi1 = (x) => x * x,
                    psi2 = (x) => 2.0
                };
            int needLayer = 0;
            double maxFail = 0.0D;
            var explicitSolver = new ExplicitHyperbolicSolver(hyperbolicProblem, 1.0, 1.0);
            var implicitSolver = new ImplicitHyperbolicSolver(hyperbolicProblem, 1.0, 1.0);
            var expl = explicitSolver.Solve(needLayer);
            var impl = implicitSolver.Solve(needLayer);

            Console.WriteLine("Layer # {0}\nExplicit:    Implicit:    Failure", needLayer);
            for (int i = 0; i < expl.X.Length; i++)
            {

                Console.WriteLine("{0:F8} | {1:F8} | {2:F8}", expl[i], impl[i], Math.Abs(expl[i] - impl[i]));
                maxFail = Math.Max(maxFail, Math.Abs(expl[i] - impl[i]));
            }

            Console.WriteLine("\nMaximal fail = {0:F6}", maxFail);
            Console.ReadLine();
        }
    }
}
