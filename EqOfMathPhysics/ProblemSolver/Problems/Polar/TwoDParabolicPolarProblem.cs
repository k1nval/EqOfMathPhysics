namespace ProblemSolver.Problems
{
    using System;

    public class TwoDParabolicPolarProblem
    {
        public const double Eps = 0.00001;

        public double L { get; set; }

        /// <summary>
        /// fi(x,y,0)
        /// </summary>

        public Func<double, double, double> Fi { get; set; }

        /// <summary>
        /// u(r, al, t)
        /// </summary>
        /// <returns></returns>
        public Func<double, double, double, double> Psi { get; set; }
    }
}
