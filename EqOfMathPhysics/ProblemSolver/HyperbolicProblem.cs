namespace ProblemSolver
{
    public class HyperbolicProblem : IProblem
    {
        public HyperbolicProblem(double h, double l, int layer)
        {
            H = h;
            L = l;
            Layer = layer;
        }

        public double H { get; set; }

        public double L { get; set; }
    }
}
