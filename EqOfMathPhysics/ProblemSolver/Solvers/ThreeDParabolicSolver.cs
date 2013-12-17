namespace ProblemSolver.Solvers
{
    using ProblemSolver.Problems;

    public class ThreeDParabolicSolver : ISolver<ThreeDLayer>
    {
        private readonly ThreeDParabolicProblem problem;

        private readonly double tau;

        private readonly double h;

        public ThreeDParabolicSolver(ThreeDParabolicProblem parabolicProblem)
        {
            problem = parabolicProblem;
            h = problem.H;
            L = problem.L;
            M = problem.M;
            I = (int)(L / h) + 1;
            J = (int)(M / h) + 1;
            tau = (h * h) / 4.0;
        }

        public double L { get; set; }

        public double M { get; set; }

        public int I { get; set; }

        public int J { get; set; }

        public ThreeDLayer Solve(int needLayer)
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

        private ThreeDLayer Next(ThreeDLayer firstLayer)
        {
            var secondLayer = new ThreeDLayer(I, J);

            // передняя грань
            for (int i = 0; i < I; i++)
            {
                secondLayer[i, 0] = problem.Psi3(i * h, firstLayer.Number * tau);
            }

            // задняя грань
            for (int i = 0; i < I; i++)
            {
                secondLayer[i, I - 1] = problem.Psi4(i * h, firstLayer.Number * tau);
            }

            // левая грань
            for (int i = 0; i < J; i++)
            {
                secondLayer[0, i] = problem.Psi1(i * h, firstLayer.Number * tau);
            }

            // правая грань
            for (int i = 0; i < J; i++)
            {
                secondLayer[J - 1, i] = problem.Psi2(i * h, firstLayer.Number * tau);
            }

            for (int i = 1; i < I - 1; i++)
            {
                for (int j = 1; j < J - 1; j++)
                {
                    secondLayer[i, j] = GetValue(i, j, firstLayer);
                }
            }

            secondLayer.Number = ++firstLayer.Number;

            return secondLayer;
        }

        private double GetValue(int i, int j, ThreeDLayer layer)
        {
            var r = tau / (h * h);
            return layer[i, j] + (r * (layer[i + 1, j] + layer[i - 1, j] + layer[i, j - 1] + layer[i, j + 1] - (4 * layer[i, j])));
        }

        private ThreeDLayer PrepareLayer()
        {
            var layer = new ThreeDLayer(I, J);
            for (int i = 0; i < I; i++)
            {
                for (int j = 0; j < J; j++)
                {
                    layer[i, j] = problem.Fi(i * h, j * h);
                }
            }

            layer.Number = 1;

            return layer;
        }
    }
}
