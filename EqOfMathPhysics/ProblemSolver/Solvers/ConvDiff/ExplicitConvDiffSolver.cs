using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolver.Problems;
using ProblemSolver.Solvers.Results;

namespace ProblemSolver.Solvers.ConvDiff
{
    public class ExplicitConvDiffSolver
    {
        private readonly ConvDiffProblem problem;

        private readonly double ht;

        private readonly double hx;

        public ExplicitConvDiffSolver(ConvDiffProblem problem, double hx)
        {
            this.hx = hx;
            this.Nx = (int)(problem.L/hx + 1E-6);
            this.ht = hx / Math.Abs(problem.a) / 2.0;
            this.problem = problem;
        }
        private int Nx { get; set; }

        public Layer Solve(int needLayer)
        {
            if (!problem.IsAgreed)
            {
                return null;
            }

            var firstLayer = new Layer(Nx + 1) { Number = 0 };

            Func<Layer, Layer> Next;

            if (problem.a > 0)
            {
                Next = NextApositive;
            }
            else
            {
                Next = NextAnegative;
            }

            for (int i = 0; i <= Nx; i++)
            {
                firstLayer[i] = problem.U0(GetXValue(i));
            }

            for (int i = 1; i <= needLayer; ++i)
            {
                firstLayer = Next(firstLayer);
            }

            return firstLayer;
        }

        private double GetValueApositive(Layer last, int i)
        {
            return last[i] - (problem.a * ht / hx * (last[i] - last[i - 1]))
            + ht * problem.f(GetXValue(i), GetTimeValue(last.Number));
        }

        private double GetValueAnegative(Layer last, int i)
        {
            return last[i] - (problem.a * ht / hx * (last[i + 1] - last[i]))
            + ht * problem.f(GetXValue(i), GetTimeValue(last.Number));
        }

        private Layer NextApositive(Layer last)
        {
            var secondLayer = new Layer(Nx + 1) { Number = last.Number + 1 };

            secondLayer[0] = problem.psi(GetTimeValue(secondLayer.Number));

            for (int i = 1; i <= Nx; i++)
            {
                secondLayer[i] = GetValueApositive(last, i);
            }

            return secondLayer;
        }

        private Layer NextAnegative(Layer last)
        {
            var secondLayer = new Layer(Nx + 1) { Number = last.Number + 1 };

            secondLayer[Nx] = problem.psi(GetTimeValue(secondLayer.Number));

            for (int i = Nx - 1; i >= 0; i--)
            {
                secondLayer[i] = GetValueAnegative(last, i);
            }

            return secondLayer;
        }

        private double GetTimeValue(int iterationNumber)
        {
            return iterationNumber * ht;
        }

        private double GetXValue(int iterationNumber)
        {
            return iterationNumber * hx;
        }
    }
}
