namespace ProblemSolver
{
    public class Layer
    {
        public Layer()
        {
        }

        public Layer(int size)
        {
            X = new double[size];
        }

        public Layer(double[] x)
        {
            X = x;
        }

        public double[] X { get; set; }

        public int Number { get; set; }

        public double this[int i]
        {
            get { return X[i]; }
            set { X[i] = value; }
        }
    }
}
