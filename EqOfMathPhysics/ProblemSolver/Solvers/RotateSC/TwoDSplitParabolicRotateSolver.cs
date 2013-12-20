namespace ProblemSolver.Solvers.RotateSC
{
    using System;

    using ProblemSolver.Problems;

    public class TwoDSplitParabolicRotateSolver : ISolver<TwoDLayer>
    {
        private readonly TwoDParabolicProblem problem;

        private readonly double h;

        private readonly double tau;

        private readonly double alpha;

        public TwoDSplitParabolicRotateSolver(TwoDParabolicProblem parabolicProblem, double angle)
        {
            problem = parabolicProblem;
            h = problem.H;
            tau = (h * h) / 4.0;
            I = (int)(problem.L / h);
            J = (int)(problem.M / h);
            alpha = angle;
        }

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
                var v1 = Next1(firstLayer); // v1

                firstLayer = Next2(v1 /*v1*/, firstLayer /*v0*/);
            }

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
            return -i * h * Math.Sin(alpha) + n * tau * Math.Cos(alpha);
        }

        private double GetValue(int i, int j, TwoDLayer firstLayer, TwoDLayer secondLayer)
        {
            var r = tau / (h * h);
            return firstLayer[i, j] + (r * (secondLayer[i, j + 1] - (2 * secondLayer[i, j]) + secondLayer[i, j - 1]));
        }

        private TwoDLayer Next2(TwoDLayer firstLayer, TwoDLayer secondLayer)
        {
            var thirdLayer = new TwoDLayer(I + 1, J + 1);

            var n = firstLayer.Number;

            // передняя грань
            for (int i = 0; i <= I; i++)
            {
                thirdLayer[i, 0] = problem.Psi(GetX(i, n), GetY(0), GetT(i, n));
                //thirdLayer[i, 0] = problem.Psi(i * h, 0 * h, (firstLayer.Number) * tau);
            }

            // задняя грань
            for (int i = 0; i <= I; i++)
            {
                thirdLayer[i, J] = problem.Psi(GetX(i, n), GetY(J), GetT(i, n));
                //thirdLayer[i, J] = problem.Psi(i * h, J * h, (firstLayer.Number) * tau);
            }

            // левая грань
            for (int i = 0; i <= J; i++)
            {
                thirdLayer[0, i] = problem.Psi(GetX(0, n), GetY(i), GetT(i, n));
                //thirdLayer[0, i] = problem.Psi(0 * h, i * h, (firstLayer.Number) * tau);
            }

            // правая грань
            for (int i = 0; i <= J; i++)
            {
                thirdLayer[I, i] = problem.Psi(GetX(I, n), GetY(i), GetT(i, n));
                //thirdLayer[I, i] = problem.Psi(I * h, i * h, (firstLayer.Number) * tau);
            }

            for (int i = 1; i <= I - 1; i++)
            {
                for (int j = 1; j <= J - 1; j++)
                {
                    thirdLayer[i, j] = GetValue(i, j, firstLayer, secondLayer);
                }
            }

            thirdLayer.Number = firstLayer.Number;

            return thirdLayer;
        }

        private TwoDLayer Next1(TwoDLayer firstLayer)
        {
            var secondLayer = new TwoDLayer(I + 1, J + 1);

            var n = firstLayer.Number + 1;

            // передняя грань
            for (int i = 0; i <= I; i++)
            {
                secondLayer[i, 0] = problem.Psi(GetX(i, n), GetY(0), GetT(i, n));
                //secondLayer[i, 0] = problem.Psi(i * h, 0 * h, (firstLayer.Number + 1) * tau);
            }

            // задняя грань
            for (int i = 0; i <= I; i++)
            {
                secondLayer[i, J] = problem.Psi(GetX(i, n), GetY(J), GetT(i, n));
                //secondLayer[i, J] = problem.Psi(i * h, J * h, (firstLayer.Number + 1) * tau);
            }

            // левая грань
            for (int i = 0; i <= J; i++)
            {
                secondLayer[0, i] = problem.Psi(GetX(0, n), GetY(i), GetT(i, n));
                //secondLayer[0, i] = problem.Psi(0 * h, i * h, (firstLayer.Number + 1) * tau);
            }

            // правая грань
            for (int i = 0; i <= J; i++)
            {
                secondLayer[I, i] = problem.Psi(GetX(I, n), GetY(i), GetT(i, n));
                //secondLayer[I, i] = problem.Psi(I * h, i * h, (firstLayer.Number + 1) * tau);
            }
            for (int j = 1; j <= J - 1; j++)
            {
                for (int i = 1; i <= I - 1; i++)
                {
                    secondLayer[i, j] = GetValue(i, j, firstLayer);
                }
            }

            secondLayer.Number = firstLayer.Number + 1;

            return secondLayer;
        }

        private double GetValue(int i, int j, TwoDLayer layer)
        {
            var r = tau / (h * h);
            return layer[i, j] + (r * (layer[i + 1, j] - (2 * layer[i, j]) + layer[i - 1, j]));
        }

        public TwoDLayer PrepareLayer()
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
