namespace ProblemSolver.Problems
{
    using System;

    public class AcousticProblem : IProblem
    {
        public double H { get; set; }

        public double L { get; set; }

        public bool IsAgreed
        {
            get
            {
                return true;
            }
        }

        // fi(x, 0)
        public Func<double, double> Fi { get; set; }
        
        // d(fi) / dt
        public Func<double, double> DFi_Dt { get; set; } 

        // v(x, 0)
        public Func<double, double> Vx { get; set; }
        
        // p(x, 0)
        public Func<double, double> Px { get; set; } 
    }
}
