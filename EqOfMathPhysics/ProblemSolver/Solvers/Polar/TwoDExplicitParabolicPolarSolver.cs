using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemsEquationsSolver;
using ProblemSolver.Problems;

namespace ProblemSolver.Solvers.Polar
{
    public class TwoDExplicitParabolicPolarSolver : TwoDAbstractParabolicPolarSolver
    {
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
                secondLayer[i, J] = problem.Psi(J * hr, i * hal , (n) * tau);
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
            double ro = hr * (j + 1);
            return layer[i, j] + tau/(ro*hr)*(layer[i, j] - layer[i, j - 1]) +
                   tau/(hr*hr)*(layer[i, j + 1] - 2*layer[i, j] + layer[i, j - 1]) +
                   tau/(ro*ro*hal*hal)*(layer[i + 1, j] - 2*layer[i, j] + layer[i - 1, j]);
        }
    }
}
