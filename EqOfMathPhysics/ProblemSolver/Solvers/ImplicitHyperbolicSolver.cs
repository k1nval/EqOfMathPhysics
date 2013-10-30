namespace ProblemSolver.Solvers
{
    using System.Collections.Generic;

    using ProblemSolver.Problems;

    public class ImplicitHyperbolicSolver : ISolver
    {
        private readonly HyperbolicProblem _problem;
        private double ht, hx;
        private int Nx, Nt;

        public ImplicitHyperbolicSolver(HyperbolicProblem problem)
        {
            this._problem = problem;
            this.hx = problem.h;
            this.Nx = (int)(problem.L / this.hx) + 1;
            this.ht = 1.0 / 600.0; // TODO
        }

        public Layer Solve(int needLayer)
        {
            var firstLayer = this.PrepareLayer(0);

            for (int i = 1; i < this.Nx - 1; i++)
            {
                firstLayer[i] = this._problem.psi1(i * this.hx);
            }

            var secondLayer = this.PrepareLayer(1);

            for (int i = 1; i < this.Nx - 1; i++)
            {
                secondLayer[i] = this._problem.psi1(i * this.hx) + (this._problem.psi2(i * this.hx) * this.ht);
            }

            for (int i = 2; i <= needLayer; ++i)
            {
                this.Next(ref firstLayer, ref secondLayer);
            }

            return secondLayer;
        }

        private double GetValue(Layer firstLayer, Layer secondLayer, int i)
        {
            return this._problem.a*this._problem.a*((this.ht*this.ht)/(this.hx*this.hx))*(secondLayer[i + 1] - 2*secondLayer[i] + secondLayer[i - 1]) +
                   2*secondLayer[i] - firstLayer[i];

        }

        public void Next(ref Layer firstLayer, ref Layer secondLayer)
        {
            var b = new List<double>();

            double lambda = this._problem.a * this._problem.a * (this.ht * this.ht) / (this.hx * this.hx);

            for (int i = 0; i < this.Nx; ++i)
            {
                if (i == 0 || i == this.Nx - 1)
                {
                    b.Add(1);
                }
                else
                {
                    b.Add((2 * lambda) + 1);
                }
            }

            var a = new List<double>();

            for (int i = 0; i < this.Nx; ++i)
            {
                if (i < this.Nx - 1) a.Add(-lambda);
                else a.Add(0);
            }

            var c = new List<double>();
            for (int i = 0; i < this.Nx - 1; ++i)
            {
                if (i > 0) c.Add(-lambda);
                else c.Add(0);
            }

            var d = new List<double>();
            for (int i = 0; i < this.Nx; ++i)
            {
                if (i == 0) d.Add(this._problem.fi0((secondLayer.Number + 1) * this.ht));
                else if (i == this.Nx - 1) d.Add(this._problem.fil((secondLayer.Number + 1) * this.ht));
                else
                {
                    d.Add(2 * secondLayer[i] - firstLayer[i] + this._problem.f(secondLayer.Number * this.hx, (secondLayer.Number + 1) * this.ht));
                }
            }

            firstLayer = secondLayer;
            secondLayer = this.SolveTridiag(a, b, c, d);
            secondLayer.Number = firstLayer.Number + 1;
        }

        private Layer SolveTridiag(List<double> a, List<double> b, List<double> c, List<double> d)
        {
            var x = new List<double>(this.Nx);
            for (int i = 0; i < this.Nx; i++)
            {
                x.Add(0);
            }

            var P = new List<double>();
            for (int i = 0; i < this.Nx - 1; ++i)
            {
                if (i == 0) P.Add(c[i] / b[i]);
                else
                {
                    P.Add(c[i] / (b[i] - P[i - 1] * a[i]));
                }
            }

            var Q = new List<double>();
            for (int i = 0; i < this.Nx; ++i)
            {
                if (i == 0) Q.Add(d[i] / b[i]);
                else
                {
                    Q.Add((d[i] - Q[i - 1] * a[i]) / (b[i] - P[i - 1] * a[i]));
                }
            }

            x[this.Nx - 1] = Q[this.Nx - 1];
            for (int i = this.Nx - 2; i >= 0; --i)
            {
                x[i] = Q[i] - P[i] * x[i + 1];
            }

            return new Layer() { X = x };
        }

        public Layer PrepareLayer(int number)
        {
            var newLayer = new Layer(this.Nx) { Number = number };

            newLayer[0] = this._problem.fi0(this.ht * newLayer.Number);

            newLayer[this.Nx - 1] = this._problem.fil(this.ht * newLayer.Number);

            return newLayer;
        }
    }
}
