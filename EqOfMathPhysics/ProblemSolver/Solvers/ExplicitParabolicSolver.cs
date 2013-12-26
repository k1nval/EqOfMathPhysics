namespace ProblemSolver.Solvers
{
    using ProblemSolver.Problems;
    using ProblemSolver.Solvers.Results;

    public class ExplicitParabolicSolver : ISolver<Layer>
    {
        private readonly ParabolicProblem problem;

        private readonly double ht;

        private readonly double hx;

        public ExplicitParabolicSolver(ParabolicProblem problem, double Hx)
            : this(problem, Hx, Hx * Hx / 2.0D / problem.K)
        {
        }

        public ExplicitParabolicSolver(ParabolicProblem parabolicProblem, double Hx, double Ht)
        {
            problem = parabolicProblem;
            hx = Hx;
            Nx = (int) (problem.L/hx + 1E-6 + 1);
            ht = Ht;
        }

        private int Nx { get; set; }

        public Layer Solve(int needLayer)
        {
            if (!problem.IsAgreed)
            {
                return null;
            }

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

        private Layer Next(Layer last)
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
