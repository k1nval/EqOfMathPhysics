namespace ProblemSolver
{
    public class ParabolicSolver : ISolver
    {
        private readonly ParabolicProblem _problem;

        public ParabolicSolver(ParabolicProblem problem)
        {
            _problem = problem;
        }

        public ProblemResult Solve(IProblem problem)
        {
            return null;
        }
    }
}
