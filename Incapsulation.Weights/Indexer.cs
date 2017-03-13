using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
    public class Indexer
    {
        private readonly double[] array;
        private readonly int start;
        public int Length { get; }

        public Indexer(double[] array, int start, int length)
        {
            this.start = start;
            Length = length;
            if(start < 0 || length < 0 || start >= array.Length || length > array.Length) throw new ArgumentException();
            this.array = array;
        }

        public double this[int i]
        {
            get
            {
                if(i < 0 || i >= start + Length) throw new IndexOutOfRangeException();
                return array[i + start];
            }
            set
            {
                if(i < 0 || i >= start + Length) throw new IndexOutOfRangeException();
                array[i + start] = value;
            }
        }
    }
}
