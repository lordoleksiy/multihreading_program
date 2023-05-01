using System.IO.Enumeration;
using System.Text;
using MultithreadingProgram.Models;

namespace MultithreadingProgram;

/**
* Архітектура комп'ютерів 3
* Курсова робота, частина 2
* Варіант: 10
* Завдання: a= ((B*C)+ max (MX*MR)
* Кількість потоків: 24
* ПВВ1 - a,B; ПВВP - MX, C, MR 
* Ковтун Олексій ІО-03
* Дата виконання: 15/04/2023
*/


/**
 * Main - Точка входу програми
 * OnProgramFinished - метод, який робить вивід на завершення виконання програми
 * OnPRogramStrated - метод, який робить вивід, коли програма почала сво
 */
internal class Program
{
    private static readonly int[] P = { 1, 2, 4, 8, 10, 12, 16 }; // кількість потоків
    private static readonly int[] N = { 1200, 2400, 3600 };  // розмірність матриць і векторів
    private static readonly string FilePath = "Results.txt";
    private static readonly ICollection<string> results = new List<string>();
    
    static void Main(string[] args)
    {
        try
        {
            RunProgram();
            WriteToFile(results);
            ChartBuilder chartBuilder = new(FilePath, N.Last());
            chartBuilder.Run();
        }
        catch (Exception e) 
        {
            Console.WriteLine($"Some error occured!\n{e.Message}");
        }
    }

    public static void RunProgram()
    {
        foreach (var p in P.AsEnumerable())
        {
            foreach (var n in N.AsEnumerable())
            {
                ThreadRunner runner = new ThreadRunner(p, n);
                runner.OnStarted += OnProgramStarted;
                runner.OnFinished += OnProgramFinished;
                runner.Run();
            }
        }
        if (!File.Exists(FilePath))
        {
            File.Create(FilePath);
        }
    }

    public static void OnProgramFinished(object? sender, FinalModel model)
    {
        double seconds = Math.Round(model.time.TotalSeconds, 3);
        Console.WriteLine($"Program is finished! N: {model.N}; P: {model.P}");
        Console.WriteLine($"Code execution time: {seconds}");
        results.Add($"P: {model.P}; N: {model.N} = {seconds}");
    }

    public static void OnProgramStarted(object? sender, StartModel model)
    {
        Console.WriteLine($"Program is stared! N: {model.N}; P: {model.P}");
    }

    private static void WriteToFile(IEnumerable<string> lines)
    {

        using StreamWriter writer = new(FilePath, false);
        foreach (var line in lines)
        {
            writer.WriteLine(line);
        }
    }
}