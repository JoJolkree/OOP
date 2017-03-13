using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delegates.Reports
{
    public interface IReportGenerator
    {
        string Caption { get; }
        string MakeReport(IEnumerable<Measurement> measurements);
        string MakeItem(string valueType, string entry);

        StatMaker StatMaker { get; }
    }

    public class HtmlGenerator : IReportGenerator
    {
        public HtmlGenerator(StatMaker statMaker)
        {
            StatMaker = statMaker;
            Caption = $"<h1>{StatMaker.Caption}</h1>";
        }

        public string MakeReport(IEnumerable<Measurement> measurements)
        {
            var result = new StringBuilder();
            result.Append(Caption);
            result.Append("<ul>");
            result.Append(MakeItem("Temperature", 
                StatMaker.MakeStat(measurements.Select(z => z.Temperature)).ToString()));
            result.Append(MakeItem("Humidity", 
                StatMaker.MakeStat(measurements.Select(z => z.Humidity)).ToString()));
            result.Append("</ul>");
            return result.ToString();
        }

        public string Caption { get; }
        public string MakeItem(string valueType, string entry)
        {
            return $"<li><b>{valueType}</b>: {entry}";
        }

        public StatMaker StatMaker { get; }
    }

    public class MarkdownGenerator : IReportGenerator
    {
        public MarkdownGenerator(StatMaker statMaker)
        {
            StatMaker = statMaker;
            Caption = $"## {StatMaker.Caption}\n\n";
        }

        public string Caption { get; }
        public string MakeReport(IEnumerable<Measurement> measurements)
        {
            var result = new StringBuilder();
            result.Append(Caption);
            result.Append(MakeItem("Temperature", 
                StatMaker.MakeStat(measurements.Select(z => z.Temperature)).ToString()));
            result.Append(MakeItem("Humidity", 
                StatMaker.MakeStat(measurements.Select(z => z.Humidity)).ToString()));
            return result.ToString();
        }

        public string MakeItem(string valueType, string entry)
        {
            return $" * **{valueType}**: {entry}\n\n";
        }

        public StatMaker StatMaker { get; }
    }

    public abstract class StatMaker
    {
        public abstract string Caption { get; }
        public abstract object MakeStat(IEnumerable<double> data);
    }

    public class MeanAndStdStatMaker : StatMaker
    {
        public override string Caption => "Mean and Std";

        public override object MakeStat(IEnumerable<double> data)
        {
            var newData = data.ToList();
            var mean = newData.Average();
            var std = Math.Sqrt(newData.Select(z => Math.Pow(z - mean, 2)).Sum() / (newData.Count - 1));

            return new MeanAndStd
            {
                Mean = mean,
                Std = std
            };
        }
    }

    public class MedianStatMaker : StatMaker
    {
        public override string Caption => "Median";
        public override object MakeStat(IEnumerable<double> data)
        {
            var list = data.OrderBy(z => z).ToList();
            if (list.Count % 2 == 0)
                return (list[list.Count / 2] + list[list.Count / 2 + 1]) / 2;
            return list[list.Count / 2];
        }
    }


    public static class ReportMakerHelper
    {
        public static string MakeFirstReport(IEnumerable<Measurement> data)
        {
            return new HtmlGenerator(new MeanAndStdStatMaker()).MakeReport(data);
        }

        public static string MakeSecondReport(IEnumerable<Measurement> data)
        {
            return new MarkdownGenerator(new MedianStatMaker()).MakeReport(data);
        }
    }
}
