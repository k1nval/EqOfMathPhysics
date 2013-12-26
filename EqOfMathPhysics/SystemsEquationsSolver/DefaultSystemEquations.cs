namespace SystemsEquationsSolver
{
    using System.Collections.Generic;
    using System.Linq;

    public class DefaultSystemEquations : ISystemEquations
    {
        public DefaultSystemEquations()
        {
        }

        public DefaultSystemEquations(double[,] matrix, IEnumerable<double> b)
        {
            Matrix = matrix;
            B = b;
        }

        public double[,] Matrix { get; set; }

        public IEnumerable<double> B { get; set; }

        public int Count
        {
            get
            {
                return B.Count();
            }
        }
    }
}
