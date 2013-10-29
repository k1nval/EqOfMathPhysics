using System;

namespace ProblemSolver
{
    using System.Collections.Generic;

    public struct Layer
    {
        public Layer(int nx) : this()
        {
            X = new List<double>(nx);
            for (var i = 0; i < nx; ++i)
            {
                X.Add(0);
            }
        }

        public IList<double> X { get; set; }

        public int Number { get; set; }

        public double this[int i]
        {
            get { return X[i]; }
            set { X[i] = value; }
        }
    }
}
