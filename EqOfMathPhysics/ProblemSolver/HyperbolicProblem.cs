namespace ProblemSolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class HyperbolicProblem : IProblem
    {
        public double H { get; set; }

        public double L { get; set; }

        public int Layer { get; set; }
    }
}
