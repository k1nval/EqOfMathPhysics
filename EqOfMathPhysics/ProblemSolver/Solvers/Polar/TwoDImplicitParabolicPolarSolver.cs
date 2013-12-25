using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsEquationsSolver;
using SystemsEquationsSolver.Methods;
using ProblemSolver.Problems;

namespace ProblemSolver.Solvers.Polar
{
    public class TwoDImplicitParabolicPolarSolver : TwoDAbstractParabolicPolarSolver
    {
        public TwoDImplicitParabolicPolarSolver(TwoDParabolicPolarProblem parabolicProblem, double nhr)
            : base(parabolicProblem, nhr)
        {
        }
        public override TwoDLayer Solve(int needLayer)
        {
            if (needLayer == 0)
            {
                return PrepareLayer();
            }

            var firstLayer = PrepareLayer();

            for (int i = 1; i <= needLayer; i++)
            {
                firstLayer = Next(firstLayer);
            }

            firstLayer.Number = needLayer;

            return firstLayer;
        }

        private TwoDLayer Next(TwoDLayer firstLayer)
        {
            var newLayer = new TwoDLayer(I + 1, J + 1);
            var A = new double[(I + 1) * (J + 1), (I + 1) * (J + 1)];
            var B = new double[(I + 1) * (J + 1)];
            //var A = new SparseMatrix((I + 1) * (J + 1), (I + 1) * (J + 1));
            //var B = new SparseVector((I + 1) * (J + 1));
            for (int i = 0; i <= I; ++i)
            {
                for (int j = 0; j <= J; ++j)
                {
                    if (i == 0 || i == I || j == 0 || j == J)
                    {
                        A[i*(J + 1) + j, i*(J + 1) + j] = 1;
                        B[i * (J + 1) + j] = problem.Psi(hr * j, hal * i, (firstLayer.Number + 1) * tau);
                    }
                    else
                    {
                        B[i * (J + 1) + j] = firstLayer[i,j];
                        double ro = hr*(j + 1);
                        A[i * (J + 1) + j, i * (J + 1) + j] = 1 - (tau) / (ro * hr) + 2 * tau / (hr * hr) + 2 * tau / (ro * ro * hal * hal);
                        A[i * (J + 1) + j, (i + 1) * (J + 1) + j] = -tau / (ro * ro * hal * hal);
                        A[i * (J + 1) + j, (i - 1) * (J + 1) + j] = -tau / (ro * ro * hal * hal);
                        A[i * (J + 1) + j, (i) * (J + 1) + (j - 1)] = tau / (hr * ro) -tau  / (hr * hr);
                        A[i * (J + 1) + j, (i) * (J + 1) + (j + 1)] = -tau / (hr * hr);
                    }
                }
            }

  
            var seidel = systemSolver.SolveSystem(new DefaultSystemEquations(A, B), IterativeMethod.Seidel);
            var X = seidel.X;
            for (int i = 0; i <= I; ++i)
            {
                for (int j = 0; j <= J; ++j)
                {
                    newLayer[i, j] = X[i * (J + 1) + j];
                }
            }

            newLayer.Number = firstLayer.Number + 1;

            return newLayer;

        }
    }
}
