namespace ProblemSolver
{
    public class TwoDLayer
    {
        public TwoDLayer(int n, int m)
        {
            Xy = new double[n, m];
        }

        public double[,] Xy { get; set; }

        public int Number { get; set; }

        public double this[int i, int j]
        {
            get { return Xy[i, j]; }
            set { Xy[i, j] = value; }
        }
    }
}
