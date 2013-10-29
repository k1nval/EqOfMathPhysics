namespace ProblemSolver
{
    public class ExplicitParabolicSolver : ISolver
    {
        private readonly ParabolicProblem _problem;
        private double ht, hx;
        private int Nx, Nt;

        public ExplicitParabolicSolver(ParabolicProblem problem)
        {
            _problem = problem;
            hx = problem.h;
            Nx = (int)(problem.L/hx) + 1;
            ht = 1.0/600.0;//TODO
        }

        public Layer Solve(int needLayer)
        {
            var firstLayer = new Layer(Nx) {Number = 0};

            for (int i = 0; i < Nx; i++)
            {
                firstLayer[i] = _problem.m0(i*hx);
            }

            for (int i = 1; i <= needLayer; ++i)
            {
                firstLayer = Next(firstLayer);
            }

            return firstLayer;
        }

        private double GetValue(Layer last, int i)
        {
            return last[i] + _problem.K * ht / (hx * hx) * (last[i + 1] - 2.0 * last[i] + last[i - 1]) + _problem.f(i * hx, last.Number * ht);

        }

        public Layer Next(Layer last)
        {
            var secondLayer = new Layer(Nx) { Number = last.Number + 1 };

            secondLayer[0] = _problem.m1(ht * secondLayer.Number);

            secondLayer[Nx - 1] = _problem.m2(ht*secondLayer.Number);

            for (int i = 1; i < Nx - 1; i++)
            {
                secondLayer[i] = GetValue(last, i);
            }

            return secondLayer;
        }
    }
}
