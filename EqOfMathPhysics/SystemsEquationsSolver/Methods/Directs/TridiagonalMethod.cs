namespace SystemsEquationsSolver.Methods.Directs
{
    using System.Collections.Generic;
    using System.Linq;

    using SystemsEquationsSolver.Methods.Abstract;
    using SystemsEquationsSolver.Results.Concrete;

    internal class TridiagonalMethod : IDirectMethods
    {
        private readonly DirectResult result = new DirectResult();

        private readonly List<double> a;

        private readonly List<double> b;

        private readonly List<double> c;

        private readonly List<double> d;

        private readonly int n;

        public TridiagonalMethod(TridiagonalSystemEquations systemEquations)
        {
            a = systemEquations.A.ToList();
            b = systemEquations.B1.ToList();
            c = systemEquations.C.ToList();
            d = systemEquations.B.ToList();
            n = d.Count;
            result.Method = "TridiagonalMethod";
        }

        public DirectResult Solve()
        {
            var x = new double[n];

            var p = new List<double>();
            for (int i = 0; i < n - 1; ++i)
            {
                if (i == 0)
                {
                    p.Add(c[i] / b[i]);
                }
                else
                {
                    p.Add(c[i] / (b[i] - (p[i - 1] * a[i])));
                }
            }

            var q = new List<double>();
            for (int i = 0; i < n; ++i)
            {
                if (i == 0)
                {
                    q.Add(d[i] / b[i]);
                }
                else
                {
                    q.Add((d[i] - (q[i - 1] * a[i])) / (b[i] - (p[i - 1] * a[i])));
                }
            }

            x[n - 1] = q[n - 1];
            for (int i = n - 2; i >= 0; --i)
            {
                x[i] = q[i] - (p[i] * x[i + 1]);
            }

            result.X = x;

            return result;
        }
    }
}
