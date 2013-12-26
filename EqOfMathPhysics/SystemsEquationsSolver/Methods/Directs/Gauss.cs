using System;
using System.Linq;

namespace SystemsEquationsSolver.Methods.Directs
{
    using SystemsEquationsSolver.Methods.Abstract;
    using SystemsEquationsSolver.Results.Concrete;

public class Gauss : IDirectMethods
{
        public uint RowCount;

        public uint ColumCount;

        public double[,] A { get; set; }

        public double[] B { get; set; }

        public double[] Answer { get; set; }

        public Gauss(DefaultSystemEquations systemEquations)
        {
            var size = systemEquations.Count;
            A = new double[size, size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    A[i, j] = systemEquations.Matrix[i, j];
                }
            }

            B = systemEquations.B.ToArray();
            Answer = new double[B.Count()];
            RowCount = (uint)A.GetLength(0);
            ColumCount = (uint)A.GetLength(0);
        }
 
        private void SortRows(int SortIndex)
        {
 
          double MaxElement = this.A[SortIndex, SortIndex];
          int MaxElementIndex = SortIndex;
          for (int i = SortIndex + 1; i < RowCount; i++)
          {
            if (this.A[i, SortIndex] > MaxElement)
            {
              MaxElement = this.A[i, SortIndex];
              MaxElementIndex = i;
            }
          }
 
          //теперь найден максимальный элемент ставим его на верхнее место
          if (MaxElementIndex > SortIndex)//если это не первый элемент
          {
            double Temp;
 
            Temp = this.B[MaxElementIndex];
            this.B[MaxElementIndex] = this.B[SortIndex];
            this.B[SortIndex] = Temp;
 
            for (int i = 0; i < ColumCount; i++)
            {
              Temp = this.A[MaxElementIndex, i];
              this.A[MaxElementIndex, i] = this.A[SortIndex, i];
              this.A[SortIndex, i] = Temp;
            }
          }
        }
 
        public int SolveMatrix()
        {
          if (RowCount != ColumCount)
            return 1; //нет решения
 
          for (int i = 0; i < RowCount - 1; i++)
          {
            SortRows(i);
            for (int j = i + 1; j < RowCount; j++)
            {
              if (A[i, i] != 0) //если главный элемент не 0, то производим вычисления
              {
                double MultElement = A[j, i] / A[i, i];
                for (int k = i; k < ColumCount; k++)
                  A[j, k] -= A[i, k] * MultElement;
                B[j] -= B[i] * MultElement;
              }
              //для нулевого главного элемента просто пропускаем данный шаг
            }
          }
 
          //ищем решение
          for (int i = (int)(RowCount - 1); i >= 0; i--)
          {
            Answer[i] = this.B[i];
 
            for (int j = (int)(RowCount - 1); j > i; j--)
              Answer[i] -= A[i, j] * Answer[j];
 
            if (A[i, i] == 0)
              if (B[i] == 0)
                return 2; //множество решений
              else
                return 1; //нет решения
 
            Answer[i] /= A[i, i];
 
          }
          return 0;
        }

        public override String ToString()
        {
          String S = "";
          for (int i = 0; i < RowCount; i++)
          {
            S += "\r\n";
            for (int j = 0; j < ColumCount; j++)
            {
              S += this.A[i, j].ToString("F04") + "\t";
            }
 
            S += "\t" + Answer[i].ToString("F08");
            S += "\t" + B[i].ToString("F04");
          }
          return S;
        }

        public DirectResult Solve()
        {
            SolveMatrix();
            var result = new DirectResult { X = this.Answer };

            return result;
        }
    }
}
