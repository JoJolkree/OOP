using System;
using System.Drawing.Drawing2D;

namespace MyPhotoshop
{
    public class PixelFilter<T> : ParametrizedFilter<T>
        where T : IParameters, new()
    {
        private string name;
        private Func<Pixel, T, Pixel> processor;

        public PixelFilter(string name, Func<Pixel, T, Pixel> processor)
        {
            this.name = name;
            this.processor = processor;
        }

        public override Photo Process(Photo original, T values)
        {
            var result = new Photo(original.Width, original.Height);

            for (var x = 0; x < result.Width; x++)
            for (var y = 0; y < result.Height; y++)
            {
                result[x, y] = processor(original[x, y], values);
            }
            return result;
        }

        public override string ToString()
        {
            return name;
        }
    }
}