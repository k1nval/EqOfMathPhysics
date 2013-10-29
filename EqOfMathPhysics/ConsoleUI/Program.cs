using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProblemSolver;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var problem = new ParabolicProblem(new InputArguments {h = 0.1D, L = 1});
            problem.m0 = (x) => 4*x*(1 - x);
            problem.m1 = (t) => 0;
            problem.m2 = (t) => 0;
            problem.K = 1;
            problem.f = (x, t) => 0;
            var solver = new ParabolicSolver(problem);
            var l = solver.Solve(6);
            for (int i = 0; i < l.X.Count; i++)
            {
                Console.WriteLine(l[i]);
            }
        }
    }
}
