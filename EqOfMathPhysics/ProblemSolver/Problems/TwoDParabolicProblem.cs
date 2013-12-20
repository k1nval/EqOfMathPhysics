namespace ProblemSolver.Problems
{
    using System;

    public class TwoDParabolicProblem : IProblem
    {
        public const double Eps = 0.00001;

        public double H { get; set; }

        public double L { get; set; }

        public double M { get; set; }

        public bool IsAgreed 
        {
            get
            {
                return Check();
            } 
        }

        /// <summary>
        /// fi(x,y,0)
        /// </summary>
        public Func<double, double, double> Fi { get; set; }
 
        /// <summary>
        /// u(0,y,t)
        /// </summary>
        public Func<double, double, double> Psi1 { get; set; } 

        /// <summary>
        ///  u(L, y, t)
        /// </summary>
        public Func<double, double, double> Psi2 { get; set; }
 
        /// <summary>
        /// u(x, 0, t)
        /// </summary>
        public Func<double, double, double> Psi3 { get; set; }
 
        /// <summary>
        /// u(x, M, t)
        /// </summary>
        public Func<double, double, double> Psi4 { get; set; }

        /// <summary>
        /// u(x, y, t)
        /// </summary>
        public Func<double, double, double, double> Psi { get; set; }


        public bool Check()
        {
            if (Math.Abs(Psi3(0, 0) - Psi1(0, 0)) < Eps && Math.Abs(Psi3(L, 0) - Psi2(0, 0)) < Eps && Math.Abs(Psi4(0, 0) - Psi1(M, 0)) < Eps
                && Math.Abs(Psi4(L, 0) - Psi2(M, 0)) < Eps && Math.Abs(Fi(0, 0) - Psi3(0, 0)) < Eps && Math.Abs(Fi(L, 0) - Psi3(L, 0)) < Eps
                && Math.Abs(Fi(0, M) - Psi4(0, 0)) < Eps && Math.Abs(Fi(L, M) - Psi4(L, 0)) < Eps)
            {
                Psi = (x, y, t) =>
                    {
                        if (Math.Abs(x - 0) < Eps) return Psi1(y, t);
                        if (Math.Abs(x - L) < Eps) return Psi2(y, t);
                        if (Math.Abs(y - 0) < Eps) return Psi3(x, t);
                        if (Math.Abs(y - M) < Eps) return Psi4(x, t);
                        return 0;
                    };

                return true;
            }

            return false;
        }
    }
}
