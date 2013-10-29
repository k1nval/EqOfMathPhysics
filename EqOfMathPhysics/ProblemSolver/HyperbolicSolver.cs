namespace ProblemSolver
{

    public class HyperbolicSolver : ISolver
    {
        private readonly HyperbolicProblem _problem;

        public HyperbolicSolver(HyperbolicProblem problem)
        {
            _problem = problem;
        }
        public Layer Solve(int needLayer)
        {
            throw new System.NotImplementedException();
        }
    }
}
