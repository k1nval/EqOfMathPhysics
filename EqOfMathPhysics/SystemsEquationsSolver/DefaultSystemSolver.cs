namespace SystemsEquationsSolver
{
    using System.Collections.Generic;
    using System.Linq;

    using SystemsEquationsSolver.Methods;
    using SystemsEquationsSolver.Methods.Directs;
    using SystemsEquationsSolver.Methods.Iteratives.JacobiMethods;
    using SystemsEquationsSolver.Results.Abstract;
    using SystemsEquationsSolver.Results.Concrete;

    public class DefaultSystemSolver : ISystemSolver
    {
        public IterativeResult SolveSystem(ISystemEquations systemEquations, IterativeMethod method, double eps = 0.001)
        {
            switch (method)
            {
                case IterativeMethod.SimpleIterations:
                    // todo выполнить проверку на возможность приведения к типу
                    return
                        new SimpleIterationsMethod(systemEquations, eps).Solve();

                case IterativeMethod.Seidel:
                    return new SeidelMethod(systemEquations, eps).Solve();

                default:
                    return null;
            }
        }

        public DirectResult SolveSystem(ISystemEquations systemEquations, DirectMethod methods)
        {
            switch (methods)
            {
                case DirectMethod.Tridiag:
                    return new TridiagonalMethod((TridiagonalSystemEquations)systemEquations).Solve();

                case DirectMethod.Gauss:
                    return new Gauss((DefaultSystemEquations)systemEquations).Solve();

                default:
                    return null;
            }
        }

        public IEnumerable<IterativeResult> SolveSystem(ISystemEquations systemEquations, IEnumerable<IterativeMethod> methods, double eps = 0.001)
        {
            return methods.Select(iterativeMethod => SolveSystem(systemEquations, iterativeMethod, eps));
        }

        public IEnumerable<DirectResult> SolveSystem(ISystemEquations systemEquations, IEnumerable<DirectMethod> methods)
        {
            return methods.Select(directMethod => SolveSystem(systemEquations, directMethod));
        }

        public IEnumerable<IterativeResult> SolveSystems(
            IEnumerable<ISystemEquations> systemEquations,
            IterativeMethod iterativeMethod,
            double eps = 0.001)
        {
            return systemEquations.Select(systemEquation => SolveSystem(systemEquation, iterativeMethod, eps));
        }

        public IEnumerable<DirectResult> SolveSystems(
            IEnumerable<ISystemEquations> systemEquations,
            DirectMethod directMethod)
        {
            return systemEquations.Select(systemEquation => SolveSystem(systemEquation, directMethod));
        }
    }
}
