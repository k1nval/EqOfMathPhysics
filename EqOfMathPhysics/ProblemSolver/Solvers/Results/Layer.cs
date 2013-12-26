namespace ProblemSolver.Solvers.Results
{
    public class Layer
    {
        public Layer()
        {
        }

        public Layer(int size)
        {
            this.X = new double[size];
        }

        public Layer(double[] x)
        {
            this.X = x;
        }

        public double[] X { get; set; }

        public int Number { get; set; }

        public int Count
        {
            get
            {
                return this.X.Length;
            }
        }

        public double this[int i]
        {
            get { return this.X[i]; }
            set { this.X[i] = value; }
        }
    }
}
