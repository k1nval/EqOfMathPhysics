namespace ProblemSolver.Solvers
{
    using System.Collections.Generic;

    using ProblemSolver.Problems;

    public class ImplicitParabolicSolver : ISolver
    {
        private readonly ParabolicProblem _problem;
        private double ht, hx;
        private int Nx, Nt;

        public ImplicitParabolicSolver(ParabolicProblem problem)
        {
            this._problem = problem;
            this.hx = problem.h;
            this.Nx = (int) (problem.L/this.hx) + 1;
            this.ht = 1.0 / 600.0; //TODO
        }

        public Layer Solve(int needLayer)
        {
            var firstLayer = new Layer(this.Nx) {Number = 0};

            for (int i = 0; i < this.Nx; i++)
            {
                firstLayer[i] = this._problem.m0(i*this.hx);
            }

            for (int i = 1; i <= needLayer; ++i)
            {
                firstLayer = this.Next(firstLayer);
            }

            return firstLayer;
        }

        private double GetValue(Layer last, int i)
        {
            return last[i] + this._problem.K*this.ht/(this.hx*this.hx)*(last[i + 1] - 2.0*last[i] + last[i - 1]) +
                   this._problem.f(i*this.hx, last.Number*this.ht);

        }

        public Layer Next(Layer last)
        {
            var b = new List<double>();
            double mid_val = 2 + (this.hx*this.hx)/(this._problem.K*this.ht);
            for (int i = 0; i < this.Nx; ++i)
            {
                if (i == 0 || i == this.Nx - 1) b.Add(1);
                else b.Add(mid_val);
            }

            var a = new List<double>();

            for (int i = 0; i < this.Nx; ++i)
            {
                if (i < this.Nx - 1) a.Add(-1);
                else a.Add(0);
            }

            var c = new List<double>();
            for (int i = 0; i < this.Nx - 1; ++i)
            {
                if (i > 0) c.Add(-1);
                else c.Add(0);
            }

            var d = new List<double>();
            for (int i = 0; i < this.Nx; ++i)
            {
                if (i == 0) d.Add(this._problem.m1(last.Number + 1*this.ht));
                else if (i == this.Nx - 1) d.Add(this._problem.m2(last.Number + 1*this.ht));
                else
                {
                    d.Add((this.hx*this.hx)/(this._problem.K*this.ht)*(last[i] + this._problem.f(i*this.hx, (last.Number + 1)*this.ht)));
                }
            }

            return this.SolveTridiag(a, b, c, d);
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
                x[i] = Q[i] - P[i]*x[i + 1];
            }

            return new Layer(){X = x};
        }
    }
}
