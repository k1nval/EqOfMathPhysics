namespace SystemsEquationsSolver
{
    using System.Collections.Generic;

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
    }
}
