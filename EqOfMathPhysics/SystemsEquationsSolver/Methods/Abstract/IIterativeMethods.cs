namespace SystemsEquationsSolver.Methods.Abstract
{
    using SystemsEquationsSolver.Results.Concrete;

    public interface IIterativeMethods
    {
        IterativeResult Solve();
    }
}
