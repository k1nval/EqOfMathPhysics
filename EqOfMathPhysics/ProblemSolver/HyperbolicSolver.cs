namespace ProblemSolver
{
    class HyperbolicSolver : ISolver
    {
        private readonly HyperbolicProblem _problem;

        public HyperbolicSolver(HyperbolicProblem problem)
        {
            _problem = problem;
        }

        public ProblemResult Solve(IProblem problem)
        {
            return null;
        }
    }
}
