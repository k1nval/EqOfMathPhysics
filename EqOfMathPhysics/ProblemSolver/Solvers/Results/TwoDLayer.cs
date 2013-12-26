namespace ProblemSolver.Solvers.Results
{
    public class TwoDLayer
    {
        public TwoDLayer(int n, int m)
        {
            this.Xy = new double[n, m];
        }

        public double[,] Xy { get; set; }

        public int Number { get; set; }

        public double this[int i, int j]
        {
            get { return this.Xy[i, j]; }
            set { this.Xy[i, j] = value; }
        }
    }
}