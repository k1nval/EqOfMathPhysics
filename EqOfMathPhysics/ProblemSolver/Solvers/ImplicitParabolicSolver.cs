namespace ProblemSolver.Solvers
{
    using System.Collections.Generic;

    using SystemsEquationsSolver;
    using SystemsEquationsSolver.Methods;

    using ProblemSolver.Problems;

    public class ImplicitParabolicSolver : ISolver
    {
        private readonly ParabolicProblem problem;

        private readonly ISystemSolver systemSolver = new DefaultSystemSolver();

        private readonly double ht;

        private readonly double hx;

        public ImplicitParabolicSolver(ParabolicProblem problem)
            : this(problem, problem.H * problem.H / 2.0D / problem.K)
        {
        }

        public ImplicitParabolicSolver(ParabolicProblem parabolicProblem, double ht)
        {
            problem = parabolicProblem;
            hx = problem.H;
            Nx = (int)(problem.L / hx) + 1;
            this.ht = ht;
        }

        public int Nx { get; private set; }

        public Layer Solve(int needLayer)
        {
            var firstLayer = new Layer(Nx) { Number = 0 };

            for (int i = 0; i < Nx; i++)
            {
                firstLayer[i] = problem.m0(GetXValue(i));
            }

            for (int i = 1; i <= needLayer; ++i)
            {
                firstLayer = Next(firstLayer);
            }

            return firstLayer;
        }

        public Layer Next(Layer last)
        {
            var b = new List<double>();
            double midVal = 2 + ((hx * hx) / (problem.K * ht));
            for (int i = 0; i < Nx; ++i)
            {
                if (i == 0 || i == Nx - 1)
                {
                    b.Add(1);
                }
                else
                {
                    b.Add(midVal);
                }
            }

            var a = new List<double>();

            for (int i = 0; i < Nx; ++i)
            {
                if (i < Nx - 1)
                {
                    a.Add(-1);
                }
                else
                {
                    a.Add(0);
                }
            }

            var c = new List<double>();
            for (int i = 0; i < Nx - 1; ++i)
            {
                if (i > 0)
                {
                    c.Add(-1);
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
                    d.Add(problem.m1(GetTimeValue(last.Number + 1)));
                }
                else if (i == Nx - 1)
                {
                    d.Add(problem.m2(GetTimeValue(last.Number + 1)));
                }
                else
                {
                    d.Add((hx * hx) / (problem.K * ht) * (last[i] + problem.f(GetXValue(i), GetTimeValue(last.Number + 1))));
                }
            }

            var systemResult = systemSolver.SolveSystem(new TridiagonalSystemEquations(a, b, c, d), DirectMethod.Tridiag);

            var resultLayer = new Layer { X = systemResult.X, Number = last.Number + 1 };
            return resultLayer;
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
