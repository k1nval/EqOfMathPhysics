namespace ProblemSolver
{
    using System.Collections.Generic;

    public class ProblemResult
    {
        public ProblemResult(IEnumerable<double> result)
        {
            Result = result;
        }

        public IEnumerable<double> Result { get; set; }
    }
}
