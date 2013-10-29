using System.Collections.Generic;

namespace ProblemSolver
{
    public class ImplicitParabolicSolver : ISolver
    {
        private readonly ParabolicProblem _problem;
        private double ht, hx;
        private int Nx, Nt;

        public ImplicitParabolicSolver(ParabolicProblem problem)
        {
            _problem = problem;
            hx = problem.h;
            Nx = (int) (problem.L/hx) + 1;
            ht = 1.0/600.0; //TODO
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
            return last[i] + _problem.K*ht/(hx*hx)*(last[i + 1] - 2.0*last[i] + last[i - 1]) +
                   _problem.f(i*hx, last.Number*ht);

        }

        public Layer Next(Layer last)
        {
            var b = new List<double>();
            double mid_val = 2 + (hx*hx)/(_problem.K*ht);
            for (int i = 0; i < Nx; ++i)
            {
                if (i == 0 || i == Nx - 1) b.Add(1);
                else b.Add(mid_val);
            }

            var a = new List<double>();

            for (int i = 0; i < Nx; ++i)
            {
                if (i < Nx - 1) a.Add(-1);
                else a.Add(0);
            }

            var c = new List<double>();
            for (int i = 0; i < Nx - 1; ++i)
            {
                if (i > 0) c.Add(-1);
                else c.Add(0);
            }

            var d = new List<double>();
            for (int i = 0; i < Nx; ++i)
            {
                if (i == 0) d.Add(_problem.m1(last.Number + 1*ht));
                else if (i == Nx - 1) d.Add(_problem.m2(last.Number + 1*ht));
                else
                {
                    d.Add((hx*hx)/(_problem.K*ht)*(last[i] + _problem.f(i*hx, (last.Number + 1)*ht)));
                }
            }

            return SolveTridiag(a, b, c, d);
        }

        private Layer SolveTridiag(List<double> a, List<double> b, List<double> c, List<double> d)
        {
            var x = new List<double>(Nx);
            for (int i = 0; i < Nx; i++)
            {
                x.Add(0);
            }

            var P = new List<double>();
            for (int i = 0; i < Nx - 1; ++i)
            {
                if (i == 0) P.Add(c[i] / b[i]);
                else
                {
                    P.Add(c[i] / (b[i] - P[i - 1] * a[i]));
                }
            }

            var Q = new List<double>();
            for (int i = 0; i < Nx; ++i)
            {
                if (i == 0) Q.Add(d[i] / b[i]);
                else
                {
                    Q.Add((d[i] - Q[i - 1] * a[i]) / (b[i] - P[i - 1] * a[i]));
                }
            }

            x[Nx - 1] = Q[Nx - 1];
            for (int i = Nx - 2; i >= 0; --i)
            {
                x[i] = Q[i] - P[i]*x[i + 1];
            }

            return new Layer(){X = x};
        }
    }
}
