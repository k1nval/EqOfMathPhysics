namespace ProblemSolver
{
    public class ExplicitHyperbolicSolver : ISolver
    {
        private readonly HyperbolicProblem _problem;
        private double ht, hx;
        private int Nx, Nt;

        public ExplicitHyperbolicSolver(HyperbolicProblem problem)
        {
            _problem = problem;
            hx = problem.h;
            Nx = (int)(problem.L / hx) + 1;
            ht = 1.0 / 600.0;//TODO
        }

        public Layer Solve(int needLayer)
        {
            var firstLayer = PrepareLayer(0);

            for (int i = 1; i < Nx - 1; i++)
            {
                firstLayer[i] = _problem.psi1(i * hx);
            }

            var secondLayer = PrepareLayer(1);

            for (int i = 1; i < Nx - 1; i++)
            {
                secondLayer[i] = _problem.psi1(i * hx) + _problem.psi2(i * hx) * (ht);
            }

            for (int i = 2; i <= needLayer; ++i)
            {
                Next(ref firstLayer, ref secondLayer);
            }

            return secondLayer;
        }

        private double GetValue(Layer firstLayer, Layer secondLayer, int i)
        {
            return _problem.a*_problem.a*((ht*ht)/(hx*hx))*(secondLayer[i + 1] - 2*secondLayer[i] + secondLayer[i - 1]) +
                   2*secondLayer[i] - firstLayer[i];

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

            newLayer[0] = _problem.fi0(ht * newLayer.Number);

            newLayer[Nx - 1] = _problem.fil(ht * newLayer.Number);

            return newLayer;
        }
    }
}
