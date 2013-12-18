using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsEquationsSolver;
using SystemsEquationsSolver.Methods;
using ProblemSolver.Problems;

namespace ProblemSolver.Solvers
{
    public class TwoDImplicitParabolicSolver
    {
        private ISystemSolver systemSolver = new DefaultSystemSolver();

        private readonly TwoDParabolicProblem problem;

        private readonly double tau;

        private readonly double h;

        public TwoDImplicitParabolicSolver(TwoDParabolicProblem parabolicProblem)
        {
            problem = parabolicProblem;
            h = problem.H;
            L = problem.L;
            M = problem.M;
            I = (int)(L / h) + 1;
            J = (int)(M / h) + 1;
            tau = (h * h) / 4.0;
        }

        public double L { get; set; }

        public double M { get; set; }

        public int I { get; set; }

        public int J { get; set; }

        public TwoDLayer Solve(int needLayer)
        {
            if (!problem.IsAgreed)
            {
                return null;
            }

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
            for (int i = 0; i <= I; ++i)
            {
                for (int j = 0; j <= J; ++j)
                {
                    if (i == 0 || i == I || j == 0 || j == J)
                    {
                        A[i*(J + 1) + j, i*(J + 1) + j] = 1;
                        B[i*(J + 1) + j] = problem.Psi(h*i, h*j, (firstLayer.Number + 1)*tau);
                    }
                    else
                    {
                        B[i*(J + 1) + j] = Math.Pow(h, 4)/tau;
                        A[i*(J + 1) + j, i*(J + 1) + j] = Math.Pow(h, 4)/tau + 2*tau + 2*tau;
                        A[i*(J + 1) + j, (i + 1)*(J + 1) + j] = -tau;
                        A[i * (J + 1) + j, (i - 1) * (J + 1) + j] = -tau;
                        A[i * (J + 1) + j, (i) * (J + 1) + (j + 1)] = -tau;
                        A[i * (J + 1) + j, (i) * (J + 1) + (j - 1)] = -tau;
                    }
                }
            }


            var seidel = systemSolver.SolveSystem(new DefaultSystemEquations(A, B), IterativeMethod.Seidel);
            var X = seidel.X;
            for (int i = 0; i <= I; ++i)
            {
                for (int j = 0; j <= J; ++j)
                {
                    newLayer[i, j] = X[i*(J + 1) + j];
                }
            }

            return newLayer;

        }

        private TwoDLayer PrepareLayer()
        {
            var layer = new TwoDLayer(I + 1, J + 1);
            for (int i = 0; i <= I; i++)
            {
                for (int j = 0; j <= J; j++)
                {
                    layer[i, j] = problem.Fi(i * h, j * h);
                }
            }

            layer.Number = 1;

            return layer;
        }
    }
}
