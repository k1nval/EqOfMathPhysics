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
            var firstLayer = new Layer(_nx);
            var secondLayer = new Layer(_nx);
            var thirdLayer = new Layer(_nx);

            var lastLayer = PrepareLayer(_nx);

            for (int i = 1; i < _nx - 1; i++)
            {
                firstLayer[i] = _problem.fi(i * _problem.h, 0);
            }

            for (int i = 1; i < _nx; i++)
            {
                lastLayer[i] = _problem.fi(i * _problem.h, _ny);
            }

            for (int j = 1; j < _ny - 1; j++)
            {
                firstLayer = this.PrepareLayer(j - 1);
                secondLayer = this.PrepareLayer(j);
                thirdLayer = this.PrepareLayer(j + 1);

                for (int i = 1; i < _nx - 1; i++)
                {
                    var arr = new double[5];
                    arr[0] = i + 1 == _nx - 1 ? secondLayer[_nx - 1] : 1; // 1
                    arr[1] = i - 1 == 0 ? secondLayer[0] : 1; // 0
                    arr[2] = j + 1 == _ny - 1 ? thirdLayer[i] : 1; // 1
                    arr[3] = j - 1 == 0 ? firstLayer[i] : 1; // 0.866
                    arr[4] = -4;

                    var d = arr;
                }
            }

            var result = new Layer((_nx - 1) * (_ny - 1)) { Number = needLayer };

            return result;
        }

        private Layer PrepareLayer(int number)
        {
            var newLayer = new Layer(_nx) { Number = number };

            newLayer[0] = _problem.fi(0, number * _problem.h);

            newLayer[_nx - 1] = _problem.fi(_nx, number * _problem.h);

            return newLayer;
        }
    }
}
