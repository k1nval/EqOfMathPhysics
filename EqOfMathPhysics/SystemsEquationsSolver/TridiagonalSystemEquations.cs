namespace SystemsEquationsSolver
{
    using System.Collections.Generic;

    // todo add function "transform a,b,c,d collections to tridiagonal matrix"
    public class TridiagonalSystemEquations : ISystemEquations
    {
        public TridiagonalSystemEquations(double[,] matrix, IEnumerable<double> b)
        {
            Matrix = matrix;
            B = b;
        }

        public TridiagonalSystemEquations(
            IEnumerable<double> a,
            IEnumerable<double> b,
            IEnumerable<double> c,
            IEnumerable<double> d)
        {
            A = a;
            B1 = b;
            C = c;
            D = d;
        }

        public double[,] Matrix { get; set; }

        public IEnumerable<double> B { get; set; }

        public IEnumerable<double> A { get; set; }

        // todo change name later
        public IEnumerable<double> B1 { get; set; }

        public IEnumerable<double> C { get; set; }
 
        public IEnumerable<double> D { get; set; } 
    }
}
