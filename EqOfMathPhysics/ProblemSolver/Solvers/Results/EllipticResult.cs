namespace ProblemSolver.Solvers.Results
{
    public class EllipticResult
    {
        public double[] SimpleIterationsX { get; set; }

        public double[] SeidelX { get; set; }

        public int Count { get; set; }

        public int CountSimpleIterations { get; set; }

        public int CountSeidelIterations { get; set; }

        public double Procent
        {
            get
            {
                return ((double)this.CountSeidelIterations / this.CountSimpleIterations) * 100.0;
            }
        }

        public string Conclusion
        {
            get
            {
                if (this.Procent < 100)
                {
                    return "Метод Зейделя быстрей на " + (100 - (int)this.Procent) + "%";
                }

                return "Метод простой итерации быстрей на " + (int)this.Procent + "%";
            }
        }
    }
}
