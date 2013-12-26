using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsEquationsSolver;
using ProblemSolver.Problems;

namespace ProblemSolver.Solvers.Polar
{
    using ProblemSolver.Solvers.Results;

    public abstract class TwoDAbstractParabolicPolarSolver
    {
        protected ISystemSolver systemSolver = new DefaultSystemSolver();

        protected readonly TwoDParabolicPolarProblem problem;

        protected readonly double hal;

        protected readonly double hr;

        protected readonly double tau;

        protected TwoDAbstractParabolicPolarSolver(TwoDParabolicPolarProblem parabolicProblem, double nhr)
        {
            hal = 2 * Math.PI / 10;
            hr = nhr;
            problem = parabolicProblem;
            L = problem.L;
            J = (int)(L / hr);
            I = (int)(2 * Math.PI / hal);
            tau = 0.25 / (1 / (hr * hr) + 1 / (hal * hal * hr * hr) + 1 / (hr * hr * hr));
            //tau = 0.001;
        }

        protected TwoDAbstractParabolicPolarSolver()
        {
            throw new NotImplementedException();
        }

        public double L { get; set; }

        public int I { get; set; }

        public int J { get; set; }

        public abstract TwoDLayer Solve(int needlayer);

        protected TwoDLayer PrepareLayer()
        {
            var layer = new TwoDLayer(I + 1, J + 1);
            for (int i = 0; i <= I; i++)
            {
                for (int j = 0; j <= J; j++)
                {
                    layer[i, j] = problem.Fi(j * hr, i * hal);
                }
            }

            layer.Number = 0;

            return layer;
        }
    }
}
