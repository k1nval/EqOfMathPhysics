namespace ProblemSolver.Solvers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ProblemSolver.Problems;

    public class EllipticSolver : ISolver
    {
        private EllipticProblem _problem;

        private int _nx;

        private int _ny;

        public EllipticSolver(EllipticProblem problem)
        {
            _problem = problem;
            _nx = (int)(_problem.L / _problem.h) + 1;
            _ny = (int)(_problem.M / _problem.h) + 1;
        }

        public Layer Solve(int needLayer)
        {
            var lastLayer = this.PrepareLayer(_ny);

            for (int i = 1; i < _ny - 1; i++)
            {
                lastLayer[i] = _problem.fi(i * _problem.h, _ny - 1);
            }

            var matrix = new double[_nx + 1, _ny];
            var k = 0;
            for (int j = 1; j < _ny - 1; j++)
            {
                for (int i = 1; i < _nx - 1; i++)
                {
                    matrix[k, 0] = i + 1 == _nx - 1 ? _problem.fi(_nx - 1, j * _problem.h) : 1;
                    matrix[k, 1] = i - 1 == 0 ? _problem.fi(0, j * _problem.h) : 1;
                    matrix[k, 2] = j + 1 == _ny - 1 ? _problem.fi(i * _problem.h, _ny - 1) : 1;
                    matrix[k, 3] = j - 1 == 0 ? _problem.fi(i * _problem.h, 0) : 1;
                    matrix[k, 4] = -4.00;
                    k++;
                }
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    var elem = matrix[i, j] < 0.001 && matrix[i, j] > -0.0001 ? 0.00 : matrix[i, j];
                    Console.Write("{0:F2} ", elem);
                }

                Console.WriteLine();
            }

            return default(Layer);
        }

        private Layer PrepareLayer(int number)
        {
            var newLayer = new Layer(_nx) { Number = number };

            newLayer[0] = _problem.fi(0, number * _problem.h);

            if (Math.Abs(newLayer[0]) < 0.001)
            {
                newLayer[0] = 0;
            }

            newLayer[_nx - 1] = _problem.fi(_nx - 1, number * _problem.h);

            if (Math.Abs(newLayer[_nx - 1]) < 0.001)
            {
                newLayer[_nx - 1] = 0;
            }

            return newLayer;
        }
    }
}
