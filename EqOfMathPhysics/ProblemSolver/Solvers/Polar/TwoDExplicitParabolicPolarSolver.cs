using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsEquationsSolver;
using ProblemSolver.Problems;

namespace ProblemSolver.Solvers.Polar
{
    public class TwoDExplicitParabolicPolarSolver
    {
        private ISystemSolver systemSolver = new DefaultSystemSolver();

        private readonly TwoDParabolicPolarProblem problem;

        private readonly double hal;

        private readonly double hr;

        private readonly double tau;

        public TwoDExplicitParabolicPolarSolver(TwoDParabolicPolarProblem parabolicProblem, double nhr)
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
            var secondLayer = new TwoDLayer(I + 1, J + 1);

            var n = firstLayer.Number + 1;

            // передняя грань
            for (int i = 0; i <= I; i++)
            {
                //secondLayer[i, 0] = problem.Psi();
                secondLayer[i, 0] = problem.Psi(i * hal, 0 * hr, (n) * tau);
            }

            // задняя грань
            for (int i = 0; i <= I; i++)
            {
               // secondLayer[i, J] = problem.Psi(GetX(i, n), GetY(J), GetT(i, n));
                secondLayer[i, J] = problem.Psi(i * hal, J * hr, (n) * tau);
            }

            // левая грань
            for (int i = 0; i <= J; i++)
            {
                //secondLayer[0, i] = problem.Psi(GetX(0, n), GetY(i), GetT(i, n));
                secondLayer[0, i] = problem.Psi(0 * hal, i * hr, (n) * tau);
            }

            // правая грань
            for (int i = 0; i <= J; i++)
            {
                //secondLayer[I, i] = problem.Psi(GetX(I, n), GetY(i), GetT(i, n));
                secondLayer[I, i] = problem.Psi(I * hal, i * hr, (n) * tau);
            }

            for (int i = 1; i < I; i++)
            {
                for (int j = 1; j < J; j++)
                {
                    secondLayer[i, j] = GetValue(i, j, firstLayer);
                }
            }

            secondLayer.Number = ++firstLayer.Number;

            return secondLayer;
        }

        private double GetValue(int i, int j, TwoDLayer layer)
        {
            double ro = hr*(j + 1);
            return layer[i, j] + tau/(ro*hr)*(layer[i, j] - layer[i, j - 1]) +
                   tau/(hr*hr)*(layer[i, j + 1] - 2*layer[i, j] + layer[i, j - 1]) +
                   tau/(ro*ro*hal*hal)*(layer[i + 1, j] - 2*layer[i, j] + layer[i - 1, j]);
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
