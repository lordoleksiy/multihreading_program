using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.ImageSharp;
using OxyPlot.Series;

namespace MultithreadingProgram;

public class ChartBuilder
{
    private readonly string _filePath;
    private readonly int N;
    private readonly IDictionary<int, double> timeLines;
    private readonly IList<DataPoint> dataSet1;
    private readonly IList<DataPoint> dataSet2;
    private readonly IList<DataPoint> dataSet3;
    public ChartBuilder(string filePath, int n)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File doesn't exist on path you provided");
        }

        _filePath = filePath;
        N = n;
        timeLines = new Dictionary<int, double>();
        dataSet1 = new List<DataPoint>();
        dataSet2 = new List<DataPoint>();
        dataSet3 = new List<DataPoint>();
    }
    public void Run()
    {
        ParseData();
        GeneratePlots();
        BuildCharts();
    }

    private void ParseData()
    {
        string[] lines = File.ReadAllLines(_filePath);
        foreach (string line in lines)
        {
            if (line.Length == 0)
            {
                continue;
            }
            string[] parts = line.Split(new char[]{ ';', '=' }, StringSplitOptions.RemoveEmptyEntries);
            int p = int.Parse(parts[0][3..]);
            int n = int.Parse(parts[1][4..]);
            double time = double.Parse(parts[2][1..]);
            if (n == N)
            {
                timeLines.Add(p, time);
            }
        }
    }
    private void BuildCharts()
    {
        
        PngExporter.Export(GeneratePlot("Час виконання програми ПРГКБА", dataSet1), $"plot1_{N}.png", 1280, 720);
        PngExporter.Export(GeneratePlot("Графік поведінки КП", dataSet2), $"plot2_{N}.png", 1280, 720);
        PngExporter.Export(GeneratePlot("Графік поведінки КЕ", dataSet3), $"plot3_{N}.png", 1280, 720);
    }
    private static PlotModel GeneratePlot(string name, IList<DataPoint> list)
    {
        var model = new PlotModel { Title = name, Background = OxyColor.FromRgb(255, 255, 255) };
        var series = new LineSeries { ItemsSource = list, Color = OxyColor.FromRgb(0, 0, 255) };
        model.Series.Add(series);
        return model;
    }

    private void GeneratePlots()
    {
        var T1 = timeLines[timeLines.Keys.Min()];
        foreach (var pair in timeLines)
        {
            var kp = T1 / pair.Value;
            dataSet1.Add(new DataPoint(pair.Key, pair.Value));
            dataSet2.Add(new DataPoint(pair.Key, kp));
            dataSet3.Add(new DataPoint(pair.Key, kp / pair.Key * 100));
        }

    }
}
