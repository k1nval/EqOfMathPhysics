namespace ProblemSolver.Solvers
{
    using System;

    using SystemsEquationsSolver;
    using SystemsEquationsSolver.Methods;

    using ProblemSolver.Problems;

    public class EllipticSolver : ISolver
    {
        private readonly EllipticProblem ellipticProblem;

        private readonly ISystemSolver systemSolver = new DefaultSystemSolver();

        public EllipticSolver(EllipticProblem problem)
        {
            ellipticProblem = problem;
            I = (int)(ellipticProblem.L / ellipticProblem.H);
            J = (int)(ellipticProblem.M / ellipticProblem.H);
        }

        public int I { get; private set; }

        public int J { get; private set; }

        public double[] Solve()
        {
            var size = (I - 1) * (J - 1);

            var matrix = new double[size, size];
            var b = new double[size];

            var index = 0;
            var nodes = new Pair[size];
            for (int i = 1; i <= I - 1; i++)
            {
                for (int j = 1; j <= J - 1; j++)
                {
                    nodes[index++] = new Pair(i, j);
                }
            }

            var k = 1;
            var l = 1;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 4.0;
                    }
                    else if ((nodes[j].X == k + 1 && nodes[j].Y == l) || (nodes[j].X == k - 1 && nodes[j].Y == l)
                        || (nodes[j].X == k && nodes[j].Y == l + 1) || (nodes[j].X == k && nodes[j].Y == l - 1))
                    {
                        matrix[i, j] = -1.0;

                        // граничные точки в столбец свободных членов

                        // левая граница
                        if (k - 1 == 0)
                        {
                            b[i] = ellipticProblem.fi(0, l * ellipticProblem.H);
                        }

                        // правая граница
                        if (k + 1 == I)
                        {
                            b[i] = ellipticProblem.fi(ellipticProblem.L, l * ellipticProblem.H);
                        }

                        // нижняя граница
                        if (l - 1 == 0)
                        {
                            b[i] = ellipticProblem.fi(k * ellipticProblem.H, 0);
                        }

                        // верхняя граница
                        if (l + 1 == J)
                        {
                            b[i] = ellipticProblem.fi(k * ellipticProblem.H, ellipticProblem.M);
                        }
                    }
                }

                l++;
                if (l == J)
                {
                    k++;
                    l = 1;
                }
            }

/*            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write("{0:F3} ", matrix[i, j]);
                }

                Console.Write(" | {0:F3}", b[i]);

                Console.WriteLine();
            }*/

            var res = systemSolver.SolveSystem(new DefaultSystemEquations(matrix, b), IterativeMethod.Seidel);

            return res.X;
        }

        public Layer Solve(int needLayer)
        {
            throw new NotImplementedException();
        }

        public class Pair
        {
            public Pair(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; set; }

            public int Y { get; set; }
        }
    }
}
