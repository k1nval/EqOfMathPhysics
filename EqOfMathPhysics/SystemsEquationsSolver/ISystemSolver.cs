namespace SystemsEquationsSolver
{
    using System.Collections.Generic;

    using SystemsEquationsSolver.Methods;
    using SystemsEquationsSolver.Results.Concrete;

    public interface ISystemSolver
    {
        IterativeResult SolveSystem(ISystemEquations systemEquations, IterativeMethod method, double eps = 0.00001);

        DirectResult SolveSystem(ISystemEquations systemEquations, DirectMethod methods);

        IEnumerable<IterativeResult> SolveSystem(ISystemEquations systemEquations, IEnumerable<IterativeMethod> methods, double eps = 0.00001);

        IEnumerable<DirectResult> SolveSystem(ISystemEquations systemEquations, IEnumerable<DirectMethod> methods);

        IEnumerable<IterativeResult> SolveSystems(
            IEnumerable<ISystemEquations> systemEquations,
            IterativeMethod iterativeMethod,
            double eps = 0.001);

        IEnumerable<DirectResult> SolveSystems(
            IEnumerable<ISystemEquations> systemEquations,
            DirectMethod directMethod);
    }
}
