namespace ProblemSolver.Solvers
{
    using ProblemSolver.Problems;

    public class ExplicitParabolicSolver : ISolver
    {
        private readonly ParabolicProblem problem;

        private readonly double ht;

        private readonly double hx;

        public ExplicitParabolicSolver(ParabolicProblem problem)
            : this(problem, problem.H * problem.H / 2.0D / problem.K)
        {
        }

        public ExplicitParabolicSolver(ParabolicProblem parabolicProblem, double htau)
        {
            problem = parabolicProblem;
            hx = problem.H;
            Nx = (int)(problem.L / hx) + 1;
            ht = htau;
        }

        public int Nx { get; set; }

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

        private double GetValue(Layer last, int i)
        {
            return last[i] + ((problem.K * ht) / (hx * hx)) * (last[i + 1] - (2.0 * last[i]) + last[i - 1]) + problem.f(GetXValue(i), GetTimeValue(last.Number));
        }

        public Layer Next(Layer last)
        {
            var secondLayer = new Layer(Nx) { Number = last.Number + 1 };

            secondLayer[0] = problem.m1(GetTimeValue(secondLayer.Number));

            secondLayer[Nx - 1] = problem.m2(GetTimeValue(secondLayer.Number));

            for (int i = 1; i < Nx - 1; i++)
            {
                secondLayer[i] = GetValue(last, i);
            }

            return secondLayer;
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
