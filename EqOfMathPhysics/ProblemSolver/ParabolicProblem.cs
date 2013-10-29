namespace ProblemSolver
{
    public class ParabolicProblem : IProblem
    {
        public ParabolicProblem(double h, double l, int layer)
        {
            H = h;
            L = l;
            Layer = layer;
        }

        public double H { get; set; }

        public double L { get; set; }

        public int Layer { get; set; }
    }
}
