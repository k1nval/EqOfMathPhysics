namespace ProblemSolver.Problems
{
    using System;

    public class EllipticProblem : IProblem
    {
        public const double Eps = 1E-100;

        // fi
        public Func<double, double, double> fi { get; set; }

        public double L { get;  set; }

        public double M { get;  set; }

        public double H { get; set; }

        // psi1(x)
        public Func<double, double> psi1 { get; set; }

        // psi2(y)
        public Func<double, double> psi2 { get; set; } 

        // psi3(x)
        public Func<double, double> psi3 { get; set; }

        // psi4(y)
        public Func<double, double> psi4 { get; set; } 

        public bool IsAgreed
        {
            get
            {
                return Check();
            }
        }

        public bool Check()
        {
            if (Math.Abs(psi1(L) - psi2(0)) < Eps && Math.Abs(psi2(M) - psi3(L)) < Eps && Math.Abs(psi4(0) - psi1(0)) < Eps && Math.Abs(psi3(0) - psi4(0)) < Eps)
            {
                return true;
            }

            return false;
        }

        public void SetFi()
        {
            fi = (x, y) =>
            {
                if (Math.Abs(x) < Eps) return psi4(y);
                if (Math.Abs(x - L) < Eps) return psi2(y);
                if (Math.Abs(y) < Eps) return psi1(x);
                if (Math.Abs(y - M) < Eps) return psi3(x);

                return Math.Abs(y) < Eps ? Math.Sin(Math.PI * x) : 0.00;
            };
        }
    }
}
