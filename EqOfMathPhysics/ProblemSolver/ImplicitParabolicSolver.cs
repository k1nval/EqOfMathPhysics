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
            var secondLayer = new Layer(Nx) {Number = last.Number + 1};

            secondLayer[0] = _problem.m1(ht*secondLayer.Number);

            secondLayer[Nx - 1] = _problem.m2(ht*secondLayer.Number);

            var b = new List<double>();
            double mid_val = 2 + (hx*hx)/(_problem.K*ht);
            for (int i = 0; i < Nx; ++i)
            {
                if (i == 0 || i == Nx - 1) b.Add(-1);
                else b.Add(-mid_val);
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

            c[0] = c[0]/b[0];
            x[0] = x[0]/b[0];

            for (int i = 1; i < Nx - 1; i++)
            {
                double m = 1.0/(b[i] - a[i]*c[i - 1]);
                c[i] = c[i]*m;
                x[i] = (x[i] - a[i]*x[i - 1])*m;
            }

            for (int i = Nx - 2; i >= 0; i--)
            {
                x[i] = x[i] - c[i]*x[i + 1];
            }

            var ans = new Layer {X = x};
            return ans;

        }
    }
}
