namespace SystemsEquationsSolver.Utils
{
    public static class NormUtil
    {
        public static double Norm(this double[,] matrix)
        {
            var sum = 0.00;
            var norm = 0.00;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    sum += matrix[i, j];
                }

                if (sum > norm)
                {
                    norm = sum;
                }

                sum = 0.00;
            }

            return norm;
        }
    }
}
