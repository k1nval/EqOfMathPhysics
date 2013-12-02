namespace ProblemSolver.Solvers
{
    using System;

    using ProblemSolver.Problems;

    public class ExplicitHyperbolicSolver : ISolver
    {
        private readonly HyperbolicProblem _problem;

        private readonly double ht;

        private readonly double hx;

        public ExplicitHyperbolicSolver(HyperbolicProblem problem) : this(problem, Math.Sqrt(problem.H * problem.H / problem.A) / 2.0D)
        {
        }

        public ExplicitHyperbolicSolver(HyperbolicProblem problem, double htau)
        {
            _problem = problem;
            hx = problem.H;
            Nx = (int)(problem.L / hx) + 1;
            ht = htau; // TODO tau
        }

        public int Nx { get; set; }

        public Layer Solve(int needLayer)
        {
            var firstLayer = PrepareLayer(0);

            for (int i = 1; i < Nx - 1; i++)
            {
                firstLayer[i] = _problem.psi1(GetXValue(i));
            }

            var secondLayer = PrepareLayer(1);

            for (int i = 1; i < Nx - 1; i++)
            {
                secondLayer[i] = _problem.psi1(GetXValue(i)) + (_problem.psi2(GetXValue(i)) * ht);
            }

            for (int i = 2; i <= needLayer; ++i)
            {
                Next(ref firstLayer, ref secondLayer);
            }

            return secondLayer;
        }

        public void Next(ref Layer firstLayer, ref Layer secondLayer)
        {
            var newLayer = PrepareLayer(secondLayer.Number + 1);

            for (int i = 1; i < Nx - 1; i++)
            {
                newLayer[i] = GetValue(firstLayer, secondLayer, i);
            }

            firstLayer = secondLayer;
            secondLayer = newLayer;
        }

        public Layer PrepareLayer(int number)
        {
            var newLayer = new Layer(Nx) { Number = number };

            newLayer[0] = _problem.fi0(GetTimeValue(newLayer.Number));

            newLayer[Nx - 1] = _problem.fil(GetTimeValue(newLayer.Number));

            return newLayer;
        }

        private double GetValue(Layer firstLayer, Layer secondLayer, int i)
        {
            double al = (ht * ht) / (hx * hx);
            return (al * secondLayer[i - 1]) + (2.0 * (1 - al) * secondLayer[i]) + (al * secondLayer[i + 1]) - firstLayer[i];
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
