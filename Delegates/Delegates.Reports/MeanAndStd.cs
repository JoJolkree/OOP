namespace Delegates.Reports
{
    public class MeanAndStd
    {
        public double Mean { get; set; }
        public double Std { get; set; }

        public override string ToString()
        {
            return Mean + "±" + Std;
        }
    }
}