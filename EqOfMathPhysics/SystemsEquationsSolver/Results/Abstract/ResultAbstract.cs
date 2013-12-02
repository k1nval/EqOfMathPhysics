namespace SystemsEquationsSolver.Results.Abstract
{
    using System.Text;

    public abstract class ResultAbstract
    {
        public double[] X { get; set; }

        public string Method { get; set; }

        public double this[int index]
        {
            get
            {
                return X[index];
            }

            set
            {
                X[index] = value;
            }
        }

        public virtual string OutputResult()
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

            return result.ToString();
        }
    }
}
