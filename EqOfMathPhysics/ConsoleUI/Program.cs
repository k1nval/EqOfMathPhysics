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
            var ellipticProblem = new EllipticProblem(new InputArguments { H = 1.0 / 3.0, L = 1.0, M = 1.0 })
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

            Console.ReadLine();
        }
    }
}
