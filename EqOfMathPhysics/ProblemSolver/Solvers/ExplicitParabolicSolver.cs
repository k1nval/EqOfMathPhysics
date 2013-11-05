using System;

namespace ProblemSolver.Solvers
{
    using ProblemSolver.Problems;

    public class ExplicitParabolicSolver : ISolver
    {
        private readonly ParabolicProblem _problem;
        private double ht, hx;
        private int Nx, Nt;

        public ExplicitParabolicSolver(ParabolicProblem problem) : this(problem, problem.h*problem.h/2.0D/problem.K)
        {
        }

        public ExplicitParabolicSolver(ParabolicProblem problem, double ht)
        {
            this._problem = problem;
            this.hx = problem.h;
            this.Nx = (int)(problem.L/this.hx) + 1;
            this.ht = ht;
        }

        private double GetTimeValue(int iterationNumber)
        {
            return iterationNumber * this.ht;
        }

        private double GetXValue(int iterationNumber)
        {
            return iterationNumber * this.hx;
        }

        public Layer Solve(int needLayer)
        {
            var firstLayer = new Layer(this.Nx) {Number = 0};

            for (int i = 0; i < this.Nx; i++)
            {
                firstLayer[i] = this._problem.m0(GetXValue(i));
            }

            for (int i = 1; i <= needLayer; ++i)
            {
                firstLayer = this.Next(firstLayer);
            }

            return firstLayer;
        }

        private double GetValue(Layer last, int i)
        {
            return last[i] + this._problem.K * this.ht / (this.hx * this.hx) * (last[i + 1] - 2.0 * last[i] + last[i - 1]) + this._problem.f(GetXValue(i), GetTimeValue(last.Number));
        }

        public Layer Next(Layer last)
        {
            var secondLayer = new Layer(this.Nx) { Number = last.Number + 1 };

            secondLayer[0] = this._problem.m1(GetTimeValue(secondLayer.Number));

            secondLayer[this.Nx - 1] = this._problem.m2(GetTimeValue(secondLayer.Number));

            for (int i = 1; i < this.Nx - 1; i++)
            {
                secondLayer[i] = this.GetValue(last, i);
            }

            return secondLayer;
        }
    }
}
