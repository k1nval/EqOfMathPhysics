namespace ProblemSolver.Solvers
{
    using ProblemSolver.Problems;

    public class ParabolicSplittingSolver : ISolver<TwoDLayer>
    {
        private readonly TwoDParabolicProblem problem;

        public int I { get; set; }

        public int J { get; set; }

        public ParabolicSplittingSolver(TwoDParabolicProblem parabolicProblem)
        {
            problem = parabolicProblem;
            h = problem.H;
            tau = (h * h) / 4.0;
            I = (int)(problem.L / h);
            J = (int)(problem.M / h);
        }

        private readonly double h;

        private readonly double tau;

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
            //var secondLayer = PrepareLayer();

            for (int i = 1; i <= needLayer; i++)
            {
                //var predLayer = firstLayer;

                var v1 = Next1(firstLayer); // v1

                firstLayer = Next2(v1 /*v1*/, firstLayer /*v0*/);
            }

            return firstLayer;
        }

        private double GetValue(int i, int j, TwoDLayer firstLayer, TwoDLayer secondLayer)
        {
            var r = tau / (h * h);
            return firstLayer[i, j] + (r * (secondLayer[i, j + 1] - (2 * secondLayer[i, j]) + secondLayer[i, j - 1]));
        }

        private TwoDLayer Next2(TwoDLayer firstLayer, TwoDLayer secondLayer)
        {
            var thirdLayer = new TwoDLayer(I + 1, J + 1);

            // передняя грань
            for (int i = 0; i <= I; i++)
            {
                thirdLayer[i, 0] = problem.Psi(i * h, 0 * h, (firstLayer.Number) * tau);
            }

            // задняя грань
            for (int i = 0; i <= I; i++)
            {
                thirdLayer[i, J] = problem.Psi(i * h, J * h, (firstLayer.Number) * tau);
            }

            // левая грань
            for (int i = 0; i <= J; i++)
            {
                thirdLayer[0, i] = problem.Psi(0 * h, i * h, (firstLayer.Number) * tau);
            }

            // правая грань
            for (int i = 0; i <= J; i++)
            {
                thirdLayer[I, i] = problem.Psi(I * h, i * h, (firstLayer.Number) * tau);
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

            // передняя грань
            for (int i = 0; i <= I; i++)
            {
                secondLayer[i, 0] = problem.Psi(i * h, 0 * h, (firstLayer.Number + 1) * tau);
            }

            // задняя грань
            for (int i = 0; i <= I; i++)
            {
                secondLayer[i, J] = problem.Psi(i * h, J * h, (firstLayer.Number + 1) * tau);
            }

            // левая грань
            for (int i = 0; i <= J; i++)
            {
                secondLayer[0, i] = problem.Psi(0 * h, i * h, (firstLayer.Number + 1) * tau);
            }

            // правая грань
            for (int i = 0; i <= J; i++)
            {
                secondLayer[I, i] = problem.Psi(I * h, i * h, (firstLayer.Number + 1) * tau);
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
                    layer[i, j] = problem.Fi(i * h, j * h);
                }
            }

            layer.Number = 0;

            return layer;
        }
    }
}
