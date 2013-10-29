namespace ProblemSolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IProblem
    {
        double H { get; set; }

        double L { get; set; }

        int Layer { get; set; }
    }
}
