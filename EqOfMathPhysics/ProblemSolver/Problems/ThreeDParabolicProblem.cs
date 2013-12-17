namespace ProblemSolver.Problems
{
    using System;

    public class ThreeDParabolicProblem : IProblem
    {
        public const double Eps = 0.001;

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

        // fi(x,y,0)
        public Func<double, double, double> Fi { get; set; }
 
        // u(0,y,t)
        public Func<double, double, double> Psi1 { get; set; } 

        // u(L, y, t)
        public Func<double, double, double> Psi2 { get; set; }
 
        // u(x, 0, t)
        public Func<double, double, double> Psi3 { get; set; }
 
        // u(x, M, t)
        public Func<double, double, double> Psi4 { get; set; }

        public bool Check()
        {
            var a = Psi3(L, 0);
            var b = Psi2(0, 0);

            var c = Psi3(0, 0);
            var d = Psi1(0, 0);

            var f = Psi4(0, 0);
            var g = Psi1(M, 0);

            var k = Psi4(L, 0);
            var l = Psi2(M, 0);
            if (Math.Abs(Psi3(L, 0) - Psi2(0, 0)) < Eps & Math.Abs(Psi3(0, 0) - Psi1(0, 0)) < Eps 
                && Math.Abs(Psi4(0, 0) - Psi1(M, 0)) < Eps && Math.Abs(Psi4(L, 0) - Psi2(M, 0)) < Eps)
            {
                return true;
            }

            return false;
        }
    }
}
