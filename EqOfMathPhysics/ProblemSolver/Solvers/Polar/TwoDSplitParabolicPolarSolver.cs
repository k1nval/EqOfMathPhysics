using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsEquationsSolver;
using ProblemSolver.Problems;

namespace ProblemSolver.Solvers.Polar
{
    public class TwoDSplitParabolicPolarSolver
    {
        private ISystemSolver systemSolver = new DefaultSystemSolver();

        private readonly TwoDParabolicPolarProblem problem;

        private readonly double hal;

        private readonly double hr;

        private readonly double tau;

        public TwoDSplitParabolicPolarSolver(TwoDParabolicPolarProblem parabolicProblem, double nhr)
        {
            hal = 2 * Math.PI / 30;
            hr = nhr;
            problem = parabolicProblem;
            L = problem.L;
            J = (int)(L / hr);
            I = (int)(2 * Math.PI / hal);
            tau = 0.25 / (1 / (hr * hr) + 1 / (hal * hal * hr * hr) + 1 / (hr * hr * hr));
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
            //var secondLayer = PrepareLayer();

            for (int i = 1; i <= needLayer; i++)
            {
                //var predLayer = firstLayer;

                var v1 = Next1(firstLayer); // v1

                firstLayer = Next2(v1 /*v1*/, firstLayer /*v0*/);
            }

            return firstLayer;
        }

        private TwoDLayer Next2(TwoDLayer firstLayer, TwoDLayer secondLayer)
        {
            var thirdLayer = new TwoDLayer(I + 1, J + 1);

            var n = firstLayer.Number;

            // передняя грань
            for (int i = 0; i <= I; i++)
            {
                //secondLayer[i, 0] = problem.Psi();
                thirdLayer[i, 0] = problem.Psi(0 * hr, i * hal, (n) * tau);
            }

            // задняя грань
            for (int i = 0; i <= I; i++)
            {
                // secondLayer[i, J] = problem.Psi(GetX(i, n), GetY(J), GetT(i, n));
                thirdLayer[i, J] = problem.Psi(J * hr, i * hal, (n) * tau);
            }

            // левая грань
            for (int i = 0; i <= J; i++)
            {
                //secondLayer[0, i] = problem.Psi(GetX(0, n), GetY(i), GetT(i, n));
                thirdLayer[0, i] = problem.Psi(i * hr, 0 * hal, (n) * tau);
            }

            // правая грань
            for (int i = 0; i <= J; i++)
            {
                //secondLayer[I, i] = problem.Psi(GetX(I, n), GetY(i), GetT(i, n));
                thirdLayer[I, i] = problem.Psi(i * hr, I * hal, (n) * tau);
            }

            for (int i = 1; i <= I - 1; i++)
            {
                for (int j = 1; j <= J - 1; j++)
                {
                    thirdLayer[i, j] = GetValue(i, j, firstLayer, secondLayer);
                }
            }

            thirdLayer.Number = firstLayer.Number;

            return thirdLayer;
        }



        private TwoDLayer Next1(TwoDLayer firstLayer)
        {
            var secondLayer = new TwoDLayer(I + 1, J + 1);

            var n = firstLayer.Number + 1;

            // передняя грань
            for (int i = 0; i <= I; i++)
            {
                //secondLayer[i, 0] = problem.Psi();
                secondLayer[i, 0] = problem.Psi(0 * hr, i * hal, (n) * tau);
            }

            // задняя грань
            for (int i = 0; i <= I; i++)
            {
                // secondLayer[i, J] = problem.Psi(GetX(i, n), GetY(J), GetT(i, n));
                secondLayer[i, J] = problem.Psi(J * hr, i * hal, (n) * tau);
            }

            // левая грань
            for (int i = 0; i <= J; i++)
            {
                //secondLayer[0, i] = problem.Psi(GetX(0, n), GetY(i), GetT(i, n));
                secondLayer[0, i] = problem.Psi(i * hr, 0 * hal, (n) * tau);
            }

            // правая грань
            for (int i = 0; i <= J; i++)
            {
                //secondLayer[I, i] = problem.Psi(GetX(I, n), GetY(i), GetT(i, n));
                secondLayer[I, i] = problem.Psi(i * hr, I * hal, (n) * tau);
            }
            for (int j = 1; j <= J - 1; j++)
            {
                for (int i = 1; i <= I - 1; i++)
                {
                    secondLayer[i, j] = GetValue(i, j, firstLayer);
                }
            }

            secondLayer.Number = firstLayer.Number + 1;

            return secondLayer;
        }

        double GetValue(int i, int j, TwoDLayer flayer)
        {
            double ro = hr*(j + 1);
            return flayer[i, j] + tau/(ro*hr)*(flayer[i, j] - flayer[i, j - 1]) +
                   tau/(hr*hr)*(flayer[i, j + 1] - 2*flayer[i, j] + flayer[i, j - 1]);
        }
        double GetValue(int i, int j, TwoDLayer flayer, TwoDLayer slayer)
        {
            double ro = hr * (j + 1);
            return flayer[i, j] + tau / (ro * ro * hal * hal) * (slayer[i + 1, j] - 2 * slayer[i, j] + slayer[i - 1, j]);
        }
        private TwoDLayer PrepareLayer()
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
