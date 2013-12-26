using System.Collections.Generic;
using System.Text;

namespace ProblemSolver.Solvers.Results
{
    public class AcousticResult
    {
        public List<AcousticLayer> Layers { get; set; }

        public AcousticResult()
        {
            Layers = new List<AcousticLayer>();
        }

        public void Add(AcousticLayer layer)
        {
            Layers.Add(layer);
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            var k = 0;
            foreach (var acousticLayer in Layers)
            {
                result.AppendFormat("Layer # {0}\n", k);
                result.Append(new string('-', 60) + "\n");
                result.Append("      x      |     t       |      p      |      v      |\n");
                for (int i = 0; i < acousticLayer.Count; i++)
                {
                    result.AppendFormat(" {0:F6}    |   {1:F6}  |   {2:F6}  |   {3:F6}  |\n", acousticLayer.X[i], acousticLayer.T[i], acousticLayer.P[i], acousticLayer.V[i]);
                }
                result.Append(new string('-', 60) + "\n");
                k++;
            }

            return result.ToString();
        }
    }

    public class AcousticLayer
    {
        public double[] X { get; set; }

        public double[] T { get; set; }

        public double[] P { get; set; }

        public double[] V { get; set; }

        public int Number;

        public int Count
        {
            get
            {
                return X.Length;
            }
        }
    }
}
