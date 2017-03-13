using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.Geometry
{
    public abstract class Body
    {
        public double Volume { get; set; }
        public abstract void Accept(IVisitor visitor);
    }

    public static class BodyExtensions
    {
        public static double GetVolume(this Body body)
        {
            IVisitor visitor = new Volume();
            body.Accept(visitor);
            return body.Volume;
        }
    }

public interface IVisitor
    {
        void Visit(Ball ball);
        void Visit(Cube cube);
        void Visit(Cyllinder cyllinder);
    }

    public class Ball : Body
    {
        public double Radius { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Cube : Body
    {
        public double Size { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Cyllinder : Body
    {
        public double Height { get; set; }
        public double Radius { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class Volume : IVisitor
    {
        public void Visit(Ball ball)
        {
            ball.Volume =  4.0 * Math.PI * Math.Pow(ball.Radius, 3) / 3;
        }

        public void Visit(Cube cube)
        {
            cube.Volume = Math.Pow(cube.Size, 3);
        }

        public void Visit(Cyllinder cyllinder)
        {
            cyllinder.Volume = Math.PI * Math.Pow(cyllinder.Radius, 2) * cyllinder.Height;
        }
    }
}
