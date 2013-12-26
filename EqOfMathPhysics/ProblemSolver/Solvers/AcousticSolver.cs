namespace ProblemSolver.Solvers
{
    using SystemsEquationsSolver;
    using SystemsEquationsSolver.Methods;

    using ProblemSolver.Problems;
    using ProblemSolver.Solvers.Results;

    public class AcousticSolver : ISolver<AcousticResult>
    {
        private const int Size = 4;

        private readonly double[,] a =
            {
                { 1.0, -1.0, 0.0, 0.0 },
                { 1.0, 1.0, 0.0, 0.0 },
                { 0.0, 0.0, 1.0, -1.0 },
                { 0.0, 0.0, 1.0, 1.0 }
            };

        private readonly AcousticProblem problem;

        private readonly ISystemSolver systemSolver = new DefaultSystemSolver();

        private readonly double h;

        public AcousticSolver(AcousticProblem acousticProblem)
        {
            problem = acousticProblem;
            h = problem.H;
            L = problem.L;
            I = (int)(L / h) + 1;
        }

        public double L { get; set; }

        public int I { get; set; }

        public AcousticResult Solve(int needLayer)
        {
            var result = new AcousticResult();
            var numberLayer = 0;

            Layer v;
            Layer p;
            Layer x;
            var t = new Layer(I);
            PrepareLayer(out v, out p, out x);

            while (I >= 1)
            {
                var newX = new Layer(I - 1);
                var newT = new Layer(I - 1);
                var newP = new Layer(I - 1);
                var newV = new Layer(I - 1);
                result.Add(new AcousticLayer { Number = numberLayer++, X = x.X, T = t.X, P = p.X, V = v.X});

                for (int i = 0; i < I - 1; i++)
                {
                    var b = new double[Size];
                    b[0] = -t[i] + x[i];
                    b[1] = t[i + 1] + x[i + 1];
                    b[2] = p[i] - v[i];
                    b[3] = p[i + 1] + v[i + 1];

                    var answer = systemSolver.SolveSystem(new DefaultSystemEquations(a, b), DirectMethod.Gauss);
                    newX[i] = answer[0];
                    newT[i] = answer[1];
                    newP[i] = answer[2];
                    newV[i] = answer[3];
                }

                x = new Layer(newX.X);
                t = new Layer(newT.X);
                p = new Layer(newP.X);
                v = new Layer(newV.X);
                I--;
            }

            return result;
        }

        public void PrepareLayer(out Layer v, out Layer p, out Layer x)
        {
            v = new Layer(I);
            p = new Layer(I);
            x = new Layer(I);

            for (int i = 0; i < I; i++)
            {
                v[i] = problem.Vx(i * h);
                p[i] = problem.Px(i * h);
                x[i] = i * h;
            }
        }
    }
}
