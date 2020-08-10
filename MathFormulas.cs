using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorApp
{
    public class MathFormulas : IEnumerable<object[]>
    {
        public bool IsEven(int num) => num % 2 == 0;

        public int Diff(int x, int y) => y - x;

        public int Sum(params int[] values) => values.Sum();

        public double Average(params int[] values) => values.Sum() / (double)values.Length;

        public int Add(int first, int second) => first + second;

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] {1, 2, 3},
            new object[] {-4, -6, -10},
            new object[] {-2, 2, 0},

        };

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 2, 3 };
            yield return new object[] { -4, -6, -10 };
            yield return new object[] { -2, 2, 0 };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
