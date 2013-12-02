namespace SystemsEquationsSolver
{
    using System.Collections.Generic;

    public interface ISystemEquations
    {
        double[,] Matrix { get; set; }

        IEnumerable<double> B { get; set; }
    }
}
