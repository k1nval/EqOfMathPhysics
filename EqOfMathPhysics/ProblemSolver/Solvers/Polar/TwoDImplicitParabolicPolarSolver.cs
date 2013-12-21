﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsEquationsSolver;
using SystemsEquationsSolver.Methods;
using ProblemSolver.Problems;

namespace ProblemSolver.Solvers.Polar
{
    public class TwoDImplicitParabolicPolarSolver
    {
        private ISystemSolver systemSolver = new DefaultSystemSolver();

        private readonly TwoDParabolicPolarProblem problem;

        private readonly double hal;

        private readonly double hr;

        private readonly double tau;

        public TwoDImplicitParabolicPolarSolver(TwoDParabolicPolarProblem parabolicProblem, double nhr)
        {
            hal = 2 * Math.PI / 10;
            hr = nhr;
            problem = parabolicProblem;
            L = problem.L;
            J = (int)(L / hr);
            I = (int)(2 * Math.PI / hal);
            tau = (hal * hal * hr * hr) / (4.0 * (hal * hal + hr * hr));
            //tau = 0.001;
        }

        public double L { get; set; }

        public int I { get; set; }

        public int J { get; set; }

        public TwoDLayer Solve(int needLayer)
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
                        B[i*(J + 1) + j] = problem.Psi(hal*i, hr*j, (firstLayer.Number + 1)*tau);
                    }
                    else
                    {
                        B[i * (J + 1) + j] = firstLayer[i,j];
                        double ro = hr*(j);
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

        private TwoDLayer PrepareLayer()
        {
            var layer = new TwoDLayer(I + 1, J + 1);
            for (int i = 0; i <= I; i++)
            {
                for (int j = 0; j <= J; j++)
                {
                    layer[i, j] = problem.Fi(i * hal, j * hr);
                }
            }

            layer.Number = 0;

            return layer;
        }
    }
}