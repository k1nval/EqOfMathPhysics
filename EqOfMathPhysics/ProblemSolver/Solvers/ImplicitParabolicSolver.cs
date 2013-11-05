namespace ProblemSolver.Solvers
{
    using System.Collections.Generic;

    using ProblemSolver.Problems;

    public class ImplicitParabolicSolver : ISolver
    {
        private readonly ParabolicProblem _problem;
        private double ht, hx;
        private int Nx, Nt;

        public ImplicitParabolicSolver(ParabolicProblem problem) : this(problem, problem.h*problem.h/2.0D/problem.K)
        {
        }

        public ImplicitParabolicSolver(ParabolicProblem problem, double ht)
        {
            this._problem = problem;
            this.hx = problem.h;
            this.Nx = (int)(problem.L/this.hx) + 1;
            this.ht = ht;
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
            var firstLayer = new Layer(this.Nx) {Number = 0};

            for (int i = 0; i < this.Nx; i++)
            {
                firstLayer[i] = this._problem.m0(GetXValue(i));
            }

            for (int i = 1; i <= needLayer; ++i)
            {
                firstLayer = this.Next(firstLayer);
            }

            return firstLayer;
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
                if (i == 0) d.Add(this._problem.m1(GetTimeValue(last.Number + 1)));
                else if (i == this.Nx - 1) d.Add(this._problem.m2(GetTimeValue(last.Number + 1)));
                else
                {
                    d.Add((this.hx * this.hx) / (this._problem.K * this.ht) * (last[i] + this._problem.f(GetXValue(i), GetTimeValue(last.Number + 1))));
                }
            }

            var resultLayer =  this.SolveTridiag(a, b, c, d);
            resultLayer.Number = last.Number + 1;
            return resultLayer;
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
