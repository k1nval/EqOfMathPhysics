﻿using ProblemSolver.Solvers.Polar;

namespace ConsoleUI
{
    using System;

    using ProblemSolver.Problems;
    using ProblemSolver.Solvers;

    public class Program
    {
        public static void Main()
        {
            //Elliptic();

           // Hyperbolic();

            //Splitting();

            Polar();

            Console.ReadKey();
        }

        public static void Splitting()
        {
            do
            {
                Console.Write("h = ");
                var h = double.Parse(Console.ReadLine());

                Console.Write("L = ");
                var L = double.Parse(Console.ReadLine());

                Console.Write("M = ");
                var M = double.Parse(Console.ReadLine());

                Console.Write("J = ");
                var J = int.Parse(Console.ReadLine());

                var parabolicProblem = new TwoDParabolicProblem()
                {
                    H = h,
                    L = L,
                    M = M,
                    Fi = (x, y) => (2 * L) + y,
                    Psi1 = (y, t) => y + t + (2 * L),
                    Psi2 = (y, t) => (2 * L) + t + y,
                    Psi3 = (x, t) => 2 * L,
                    Psi4 = (x, t) => (2 * L) + M
                };
                var parabolicSolver = new ParabolicSplittingSolver(parabolicProblem, Math.PI / 4.0);
                var parabolicSolver1 = new TwoDExplicitParabolicSolver(parabolicProblem, Math.PI / 4.0);
                var parabolicSolver2 = new TwoDImplicitParabolicSolver(parabolicProblem);
                var ans = parabolicSolver.Solve(J);
                var ans1 = parabolicSolver1.Solve(J);
                var ans2 = parabolicSolver2.Solve(J);

                if (ans == null)
                {
                    Console.WriteLine("Условия согласования не выполнены");
                }
                else
                {
                    var maxFail = 0.00;
                    Console.WriteLine("Layer # {0}", ans.Number);
                    Console.WriteLine("Explicit:        Implicit:       Splitting:      Failure(Splitting and Explicit):");
                    for (int i = 0; i < ans.Xy.GetLength(0); i++)
                    {
                        for (int j = 0; j < ans.Xy.GetLength(1); j++)
                        {
                            Console.WriteLine("{0:F6}   |   {1:F6}      |   {2:F6}  |   {3:F6}  |", ans1[i, j], ans2[i, j], ans[i, j], Math.Abs(ans1[i, j] - ans[i, j]));
                            maxFail = Math.Max(maxFail, Math.Abs(ans1[i, j] - ans[i, j]));
                        }
                    }

                    Console.WriteLine("Max Fail = {0:F6}", maxFail);
                }
            }
            while (true);
        }

        public static void Polar()
        {
            do
            {
                Console.Write("hr = ");
                var h = double.Parse(Console.ReadLine());

                Console.Write("L = ");
                var L = double.Parse(Console.ReadLine());

                Console.Write("J = ");
                var J = int.Parse(Console.ReadLine());

                var parabolicProblem = new TwoDParabolicPolarProblem()
                {
                    L = L,
                    Fi = (r, al) => Math.Cos(r),
                    Psi = (r, al, t) => Math.Cos(r) + Math.Sin(t)
                };
                var parabolicSolver = new TwoDExplicitParabolicPolarSolver(parabolicProblem, h);
                //var parabolicSolver1 = new TwoDExplicitParabolicSolver(parabolicProblem, Math.PI / 4.0);
                var parabolicSolver2 = new TwoDImplicitParabolicPolarSolver(parabolicProblem, h);
                var ans = parabolicSolver.Solve(J);
                var ans1 = ans;
                var ans2 = parabolicSolver2.Solve(J);
                var tmp = 666;
                if (ans == null)
                {
                    Console.WriteLine("Условия согласования не выполнены");
                }
                else
                {
                    var maxFail = 0.00;
                    Console.WriteLine("Layer # {0}", ans.Number);
                    Console.WriteLine("Explicit:        Implicit:       Splitting:      Failure(Splitting and Explicit):");
                    for (int i = 0; i < ans.Xy.GetLength(0); i++)
                    {
                        for (int j = 0; j < ans.Xy.GetLength(1); j++)
                        {
                            Console.WriteLine("{0:F6}   |   {1:F6}      |   {2:F6}  |   {3:F6}  |", ans1[i, j], ans2[i, j], ans[i, j], Math.Abs(ans1[i, j] - ans[i, j]));
                            maxFail = Math.Max(maxFail, Math.Abs(ans1[i, j] - ans[i, j]));
                        }
                    }

                    Console.WriteLine("Max Fail = {0:F6}", maxFail);
                }
            }
            while (true);
        }

        public static void Elliptic()
        {
            do
            {
                Console.Write("h = ");
                var h = double.Parse(Console.ReadLine());

                Console.Write("L = ");
                var L = double.Parse(Console.ReadLine());

                Console.Write("M = ");
                var M = double.Parse(Console.ReadLine());

                var ellipticProblem = new EllipticProblem
                {
                    L = L,
                    M = M,
                    fi = (x, y) => Math.Abs(y) < 1E-10 ? Math.Sin(Math.PI * x) : 0.00,
                    psi1 = (x) => Math.Sin(Math.PI * x),
                    psi2 = (y) => Math.Abs(y) < 1E-10 ? Math.Sin(Math.PI * L) : 0.00,
                    psi3 = (x) => 0.0,
                    psi4 = (y) => 0.00
                };

                var ellipticSolver = new EllipticSolver(ellipticProblem, h);

                var answer = ellipticSolver.Solve(0);
                if (answer == null)
                {
                    Console.WriteLine("Условия согласования не выполнены");
                    return;
                }

                var k = 0;

                Console.WriteLine("SimpleIterations:    Sediel:   Failure:");
                var x1 = answer.SimpleIterationsX;
                var x2 = answer.SeidelX;
                var maxFail = 0.00;
                for (int i = 0; i < answer.Count; i++)
                {
                    Console.WriteLine("{0:F8}       | {1:F8} | {2:F8}", x1[i], x2[i], Math.Abs(x1[i] - x2[i]));
                    maxFail = Math.Max(maxFail, Math.Abs(x1[i] - x2[i]));
                }
                
                Console.WriteLine("\nMax Fail = {0:F6}", maxFail);
                Console.WriteLine(answer.Conclusion + Environment.NewLine);
                Console.WriteLine(new string('-', 50));
            }
            while (true);
        }

        public static void Hyperbolic()
        {
            do
            {
                Console.Write("h = ");
                var h = double.Parse(Console.ReadLine());

                Console.Write("L = ");
                var L = double.Parse(Console.ReadLine());

                Console.Write("J = ");
                var J = int.Parse(Console.ReadLine());

                var hyperbolicProblem = new HyperbolicProblem()
                {
                    A = 1,
                    L = L,
                    f = (x, t) => 0,
                    fi0 = (t) => Math.Sin(t),
                    fil = (t) => t + 6.0,
                    psi1 = (x) => x * x,
                    psi2 = (x) => 2.0
                };

                int needLayer = J;
                double maxFail = 0.0D;
                var explicitSolver = new ExplicitHyperbolicSolver(hyperbolicProblem, h, h / 2.0);
                var implicitSolver = new ImplicitHyperbolicSolver(hyperbolicProblem, h, h / 2.0);
                var expl = explicitSolver.Solve(needLayer);

                if (expl == null)
                {
                    Console.WriteLine("Условия согласования не выполнены");
                    return;
                }
                var impl = implicitSolver.Solve(needLayer);

                Console.WriteLine("Layer # {0}\nExplicit:    Implicit:    Failure", needLayer);
                for (int i = 0; i < expl.X.Length; i++)
                {

                    Console.WriteLine("{0:F8} | {1:F8} | {2:F8}", expl[i], impl[i], Math.Abs(expl[i] - impl[i]));
                    maxFail = Math.Max(maxFail, Math.Abs(expl[i] - impl[i]));
                }

                Console.WriteLine("\nMaximal fail = {0:F6}", maxFail);
                Console.WriteLine(new string('-', 50) + Environment.NewLine);
            }
            while (true);
        }
    }
}
