namespace SystemsEquationsSolver.Methods.Iteratives.JacobiMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SystemsEquationsSolver.Results.Concrete;
    using SystemsEquationsSolver.Utils;

    internal abstract class JacobiMethodsAbstract
    {
        public abstract double[] GetX(double[,] alpha, double[] beta, double[] px);

        public virtual IterativeResult Solve(ISystemEquations systemEquations, double eps = 0.001)
        {
            var result = new IterativeResult();
            var size = systemEquations.Matrix.GetLength(0);
            double[,] alpha;
            double[] beta;
            Transform(systemEquations.Matrix, systemEquations.B.ToArray(), size, out alpha, out beta);

            if (alpha.Norm() > 1.0)
            {
                return null;
            }

            double epsK;
            var px = beta;
            do
            {
                result.X = GetX(alpha, beta, px);
                epsK = (alpha.Norm() / (1.0 - alpha.Norm())) * DifferentAbs(result.X, px).Max();
                result.CountIterations++;
                px = result.X;
            }
            while (epsK > eps);

            return result;
        }

        public void Transform(double[,] matrix, double[] b, int size, out double[,] alpha, out double[] beta)
        {
            alpha = new double[size, size];
            beta = new double[size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i != j)
                    {
                        alpha[i, j] = -(matrix[i, j] / matrix[i, i]);
                    }
                }

                beta[i] = b[i] / matrix[i, i];
            }
        }

        public IEnumerable<double> DifferentAbs(double[] a, double[] b)
        {
            var n = a.Length;
            var result = new double[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = Math.Abs(a[i] - b[i]);
            }

            return result;
        }
    }
}
