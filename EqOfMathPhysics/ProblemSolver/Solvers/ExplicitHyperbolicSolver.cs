namespace ProblemSolver.Solvers
{
    using ProblemSolver.Problems;

    public class ExplicitHyperbolicSolver : ISolver
    {
        private readonly HyperbolicProblem _problem;
        private double ht, hx;
        private int Nx, Nt;

        public ExplicitHyperbolicSolver(HyperbolicProblem problem)
        {
            this._problem = problem;
            this.hx = problem.h;
            this.Nx = (int)(problem.L / this.hx) + 1;
            this.ht = 1.0 / 600.0; // TODO
        }

        private double GetTimeValue(int iterationNumber)
        {
            return iterationNumber * this.ht;
        }

        private double GetXValue(int iterationNumber)
        {
            return iterationNumber * this.hx;
        }

        public Layer Solve(int needLayer)
        {
            var firstLayer = this.PrepareLayer(0);

            for (int i = 1; i < this.Nx - 1; i++)
            {
                firstLayer[i] = this._problem.psi1(GetXValue(i));
            }

            var secondLayer = this.PrepareLayer(1);

            for (int i = 1; i < this.Nx - 1; i++)
            {
                secondLayer[i] = this._problem.psi1(GetXValue(i)) + (this._problem.psi2(GetXValue(i)) * this.ht);
            }

            for (int i = 2; i <= needLayer; ++i)
            {
                this.Next(ref firstLayer, ref secondLayer);
            }

            return secondLayer;
        }

        public void Next(ref Layer firstLayer, ref Layer secondLayer)
        {
            var newLayer = this.PrepareLayer(secondLayer.Number + 1);

            for (int i = 1; i < this.Nx - 1; i++)
            {
                newLayer[i] = this.GetValue(firstLayer, secondLayer, i);
            }

            firstLayer = secondLayer;
            secondLayer = newLayer;
        }

        public Layer PrepareLayer(int number)
        {
            var newLayer = new Layer(this.Nx) { Number = number };

            newLayer[0] = this._problem.fi0(GetTimeValue(newLayer.Number));

            newLayer[this.Nx - 1] = this._problem.fil(GetTimeValue(newLayer.Number));

            return newLayer;
        }

        private double GetValue(Layer firstLayer, Layer secondLayer, int i)
        {
            return _problem.a * _problem.a * ((ht * ht) / (hx * hx)) * (secondLayer[i + 1] - (2 * secondLayer[i]) + secondLayer[i - 1]) + (2 * secondLayer[i]) - firstLayer[i];
        }
    }
}
