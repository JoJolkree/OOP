using System;
using System.Runtime.InteropServices;

namespace MyPhotoshop
{
	public class Photo
	{
		public readonly int Width;
		public readonly int Height;
		public readonly Pixel[,] Data;

	    public Photo(int width, int height)
	    {
	        this.Width = width;
	        this.Height = height;
	        Data = new Pixel[width, height];
	    }

	    public Pixel this[int x, int y] {
	        get { return Data[x, y]; }
	        set { Data[x, y] = value; }
	    }

	}
}
