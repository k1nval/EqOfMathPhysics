namespace ProblemSolver.Solvers
{
    public interface ISolver<out T>
    {
        // todo fix needLayer
        T Solve(int needLayer);
    }
}
