namespace ProblemSolver.Solvers.RotateSC
{
    using System;

    using ProblemSolver.Problems;
    using ProblemSolver.Solvers.Results;

    public class TwoDExplicitParabolicRotateSolver : ISolver<TwoDLayer>
    {
                private readonly TwoDParabolicProblem problem;

        private readonly double tau;

        private readonly double h;

        private readonly double alpha;

        public TwoDExplicitParabolicRotateSolver(TwoDParabolicProblem parabolicProblem, double angle)
        {
            problem = parabolicProblem;
            h = problem.H;
            L = problem.L;
            M = problem.M;
            I = (int)(L / h);
            J = (int)(M / h);
            tau = (h * h) / 4.0;
            alpha = angle;
        }

        public double L { get; set; }

        public double M { get; set; }

        public int I { get; set; }

        public int J { get; set; }

        public TwoDLayer Solve(int needLayer)
        {
            if (!problem.IsAgreed)
            {
                return null;
            }

            if (needLayer == 0)
            {
                return PrepareLayer();
            }

            var firstLayer = PrepareLayer();

            for (int i = 1; i <= needLayer; i++)
            {
                firstLayer = Next(firstLayer);
            }

            firstLayer.Number = needLayer;

            return firstLayer;
        }

        private double GetX(int i, int n)
        {
            return i * h * Math.Cos(alpha) + n * tau * Math.Sin(alpha);
        }

        private double GetY(int i)
        {
            return i * h;
        }

        private double GetT(int i, int n)
        {
            return - i * h * Math.Sin(alpha) + n * tau * Math.Cos(alpha);
        }

        private TwoDLayer Next(TwoDLayer firstLayer)
        {
            var secondLayer = new TwoDLayer(I + 1, J + 1);

            var n = firstLayer.Number + 1;

            // передняя грань
            for (int i = 0; i <= I; i++)
            {
                secondLayer[i, 0] = problem.Psi(GetX(i, n), GetY(0), GetT(i, n));
                //secondLayer[i, 0] = problem.Psi(i * h, 0 * h, (n) * tau);
            }

            // задняя грань
            for (int i = 0; i <= I; i++)
            {
                secondLayer[i, J] = problem.Psi(GetX(i, n), GetY(J), GetT(i, n));
                //secondLayer[i, J] = problem.Psi(i * h, J * h, (n) * tau);
            }

            // левая грань
            for (int i = 0; i <= J; i++)
            {
                secondLayer[0, i] = problem.Psi(GetX(0, n), GetY(i), GetT(i, n));
                //secondLayer[0, i] = problem.Psi(0 * h, i * h, (n) * tau);
            }

            // правая грань
            for (int i = 0; i <= J; i++)
            {
                secondLayer[I, i] = problem.Psi(GetX(I, n), GetY(i), GetT(i, n));
                //secondLayer[I, i] = problem.Psi(I * h, i * h, (n) * tau);
            }

            for (int i = 1; i < I; i++)
            {
                for (int j = 1; j < J; j++)
                {
                    secondLayer[i, j] = GetValue(i, j, firstLayer);
                }
            }

            secondLayer.Number = ++firstLayer.Number;

            return secondLayer;
        }

        private double GetValue(int i, int j, TwoDLayer layer)
        {
            var r = tau / (h * h);
            return layer[i, j] + (r * (layer[i + 1, j] + layer[i - 1, j] + layer[i, j - 1] + layer[i, j + 1] - (4 * layer[i, j])));
        }

        private TwoDLayer PrepareLayer()
        {
            var layer = new TwoDLayer(I + 1, J + 1);
            for (int i = 0; i <= I; i++)
            {
                for (int j = 0; j <= J; j++)
                {
                    layer[i, j] = problem.Fi(GetX(i, 0), GetY(j));
                }
            }

            layer.Number = 0;

            return layer;
        }
    }
}
