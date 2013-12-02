namespace SystemsEquationsSolver.Methods.Iteratives.JacobiMethods
{
    using SystemsEquationsSolver.Methods.Abstract;
    using SystemsEquationsSolver.Results.Concrete;

    internal class SeidelMethod : JacobiMethodsAbstract, IIterativeMethods
    {
        private readonly double eps;

        private readonly int size;

        private readonly ISystemEquations systemEquations;

        public SeidelMethod(ISystemEquations systemEq, double e)
        {
            systemEquations = systemEq;
            eps = e;
            size = systemEquations.Matrix.GetLength(0);
        }

        public IterativeResult Solve()
        {
            var result = Solve(systemEquations, eps);
            result.Method = "SeidelMethod";
            return result;
        }

        public override double[] GetX(double[,] alpha, double[] beta, double[] px)
        {
            var n = px.Length;
            var r = new double[n];
            for (int i = 0; i < size; i++)
            {
                r[i] += beta[i];
                for (int j = 0; j < i; j++)
                {
                    r[i] += alpha[i, j] * r[j];
                }

                for (int j = i; j < size; j++)
                {
                    r[i] += alpha[i, j] * px[j];
                }
            }

            return r;
        }
    }
}
