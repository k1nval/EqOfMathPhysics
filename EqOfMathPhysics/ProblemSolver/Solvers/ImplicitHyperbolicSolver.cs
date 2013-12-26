namespace ProblemSolver.Solvers
{
    using System;
    using System.Collections.Generic;

    using SystemsEquationsSolver;
    using SystemsEquationsSolver.Methods;

    using ProblemSolver.Problems;
    using ProblemSolver.Solvers.Results;

    public class ImplicitHyperbolicSolver : ISolver<Layer>
    {
        private readonly HyperbolicProblem problem;

        private readonly ISystemSolver systemSolver = new DefaultSystemSolver();

        private readonly double ht;

        private readonly double hx;

        public ImplicitHyperbolicSolver(HyperbolicProblem problem, double Hx)
            : this(problem, Hx, Hx / problem.A / 2.0D)
        {
        }

        public ImplicitHyperbolicSolver(HyperbolicProblem hyperbolicProblem, double Hx, double Ht)
        {
            problem = hyperbolicProblem;
            hx = Hx;
            Nx = (int)(problem.L / hx) + 1;
            ht = Ht; // TODO tau
        }

        public int Nx { get; set; }

        public Layer Solve(int needLayer)
        {
            if (!problem.IsAgreed)
            {
                return null;
            }

            var firstLayer = PrepareLayer(0);

            for (int i = 1; i < Nx - 1; i++)
            {
                firstLayer[i] = problem.psi1(GetXValue(i));
            }
            if (needLayer == 0) return firstLayer;
            var secondLayer = PrepareLayer(1);

            for (int i = 1; i < Nx - 1; i++)
            {
                secondLayer[i] = problem.psi1(GetXValue(i)) + (problem.psi2(GetXValue(i)) * ht);
            }

            for (int i = 2; i <= needLayer; ++i)
            {
                Next(ref firstLayer, ref secondLayer);
            }

            return secondLayer;
        }

        private void Next(ref Layer firstLayer, ref Layer secondLayer)
        {
            var b = new List<double>();

            double lambda = problem.A * problem.A * (ht * ht) / (hx * hx);

            for (int i = 0; i < Nx; ++i)
            {
                if (i == 0 || i == Nx - 1)
                {
                    b.Add(1);
                }
                else
                {
                    b.Add((2 * lambda) + 1);
                }
            }

            var a = new List<double>();

            for (int i = 0; i < Nx; ++i)
            {
                if (i < Nx - 1)
                {
                    a.Add(-lambda);
                }
                else
                {
                    a.Add(0);
                }
            }

            var c = new List<double>();
            for (int i = 0; i < Nx; ++i)
            {
                if (i > 0)
                {
                    c.Add(-lambda);
                }
                else
                {
                    c.Add(0);
                }
            }

            var d = new List<double>();
            for (int i = 0; i < Nx; ++i)
            {
                if (i == 0)
                {
                    d.Add(problem.fi0(GetTimeValue(secondLayer.Number + 1)));
                }
                else if (i == Nx - 1)
                {
                    d.Add(problem.fil(GetTimeValue(secondLayer.Number + 1)));
                }
                else
                {
                    d.Add((2 * secondLayer[i]) - firstLayer[i] + problem.f(GetXValue(i), GetTimeValue(secondLayer.Number + 1)));
                }
            }

            var systemResult = systemSolver.SolveSystem(
                new TridiagonalSystemEquations(a, b, c, d),
                DirectMethod.Tridiag);

            firstLayer = secondLayer;
            secondLayer = new Layer { X = systemResult.X, Number = firstLayer.Number + 1 };
        }

        private Layer PrepareLayer(int number)
        {
            var newLayer = new Layer(Nx) { Number = number };

            newLayer[0] = problem.fi0(GetTimeValue(newLayer.Number));

            newLayer[Nx - 1] = problem.fil(GetTimeValue(newLayer.Number));

            return newLayer;
        }

        private double GetTimeValue(int iterationNumber)
        {
            return iterationNumber * ht;
        }

        private double GetXValue(int iterationNumber)
        {
            return iterationNumber * hx;
        }
    }
}
