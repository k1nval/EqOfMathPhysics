//using System.Collections.Generic;
//
//namespace ProblemSolver
//{
//    public class ImplicitHyperbolicSolver : ISolver
//    {
//        private readonly HyperbolicProblem _problem;
//        private double ht, hx;
//        private int Nx, Nt;
//
//        public ImplicitHyperbolicSolver(HyperbolicProblem problem)
//        {
//            _problem = problem;
//            hx = problem.h;
//            Nx = (int)(problem.L / hx) + 1;
//            ht = 2.0 / 3.0;//TODO
//        }
//
//        public Layer Solve(int needLayer)
//        {
//            var firstLayer = PrepareLayer(0);
//
//            for (int i = 1; i < Nx - 1; i++)
//            {
//                firstLayer[i] = _problem.psi1(i * hx);
//            }
//
//            var secondLayer = PrepareLayer(1);
//
//            for (int i = 1; i < Nx - 1; i++)
//            {
//                secondLayer[i] = _problem.psi1(i * hx) + _problem.psi2(i * hx) * (ht);
//            }
//
//            for (int i = 2; i <= needLayer; ++i)
//            {
//                Next(ref firstLayer, ref secondLayer);
//            }
//
//            return secondLayer;
//        }
//
//        private double GetValue(Layer firstLayer, Layer secondLayer, int i)
//        {
//            return _problem.a*_problem.a*((ht*ht)/(hx*hx))*(secondLayer[i + 1] - 2*secondLayer[i] + secondLayer[i - 1]) +
//                   2*secondLayer[i] - firstLayer[i];
//
//        }
//
//        public Layer Next(Layer last)
//        {
//            var b = new List<double>();
//            double mid_val = 2 + (hx * hx) / (_problem.K * ht);
//            for (int i = 0; i < Nx; ++i)
//            {
//                if (i == 0 || i == Nx - 1) b.Add(1);
//                else b.Add(mid_val);
//            }
//
//            var a = new List<double>();
//
//            for (int i = 0; i < Nx; ++i)
//            {
//                if (i < Nx - 1) a.Add(-1);
//                else a.Add(0);
//            }
//
//            var c = new List<double>();
//            for (int i = 0; i < Nx - 1; ++i)
//            {
//                if (i > 0) c.Add(-1);
//                else c.Add(0);
//            }
//
//            var d = new List<double>();
//            for (int i = 0; i < Nx; ++i)
//            {
//                if (i == 0) d.Add(_problem.fi0(last.Number + 1 * ht));
//                else if (i == Nx - 1) d.Add(_problem.fil(last.Number + 1 * ht));
//                else
//                {
//                    d.Add((hx * hx) / (_problem.K * ht) * (last[i] + _problem.f(i * hx, (last.Number + 1) * ht)));
//                }
//            }
//
//            return SolveTridiag(a, b, c, d);
//        }
//
//        private Layer SolveTridiag(List<double> a, List<double> b, List<double> c, List<double> d)
//        {
//            var x = new List<double>(Nx);
//            for (int i = 0; i < Nx; i++)
//            {
//                x.Add(0);
//            }
//
//             
//
//            var P = new List<double>();
//            for (int i = 0; i < Nx - 1; ++i)
//            {
//                if (i == 0) P.Add(c[i] / b[i]);
//                else
//                {
//                    P.Add(c[i] / (b[i] - P[i - 1] * a[i]));
//                }
//            }
//
//            var Q = new List<double>();
//            for (int i = 0; i < Nx; ++i)
//            {
//                if (i == 0) Q.Add(d[i] / b[i]);
//                else
//                {
//                    Q.Add((d[i] - Q[i - 1] * a[i]) / (b[i] - P[i - 1] * a[i]));
//                }
//            }
//
//            x[Nx - 1] = Q[Nx - 1];
//            for (int i = Nx - 2; i >= 0; --i)
//            {
//                x[i] = Q[i] - P[i] * x[i + 1];
//            }
//
//            return new Layer() { X = x };
//        }
//
//        public Layer PrepareLayer(int number)
//        {
//            var newLayer = new Layer(Nx) { Number = number };
//
//            newLayer[0] = _problem.fi0(ht * newLayer.Number);
//
//            newLayer[Nx - 1] = _problem.fil(ht * newLayer.Number);
//
//            return newLayer;
//        }
//    }
//}
