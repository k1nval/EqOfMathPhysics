namespace SystemsEquationsSolver.Results.Concrete
{
    using System.Text;

    using SystemsEquationsSolver.Results.Abstract;

    public class IterativeResult : ResultAbstract
    {
        public IterativeResult()
        {
        }

        public IterativeResult(double[] x, int countIterations, string method)
        {
            X = x;
            CountIterations = countIterations;
            Method = method;
        }

        public int CountIterations { get; set; }

        public override string OutputResult()
        {
            var result = new StringBuilder();

            result.Append(new string('-', 20));
            result.Append(Method);
            result.Append(new string('-', 20));
            for (int i = 0; i < X.Length; i++)
            {
                result.AppendFormat("x{0} = {1}\n", i + 1, X[i]);
            }

            result.Append(new string('-', 20));
            result.AppendFormat("Iterations Count = {0}", CountIterations);
            result.Append(new string('-', 20));

            return result.ToString();
        }
    }
}
