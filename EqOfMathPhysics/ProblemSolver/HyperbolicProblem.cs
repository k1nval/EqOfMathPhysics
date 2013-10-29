namespace ProblemSolver
{
    public class HyperbolicProblem : IProblem
    {
        public HyperbolicProblem(double h, double l, int layer)
        {
            this.h = h;
            L = l;
        }

        public double h { get; set; }

        public double L { get; set; }
    }
}
