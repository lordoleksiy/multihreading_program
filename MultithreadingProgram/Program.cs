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
    private static readonly int P = 24; // кількість потоків
    private static readonly int N = 1000;  // розмірність матриць і векторів
    static void Main(string[] args)
    {
        try
        {
            ThreadRunner runner = new ThreadRunner(P, N);
            runner.OnStarted += OnProgramStarted;
            runner.OnFinished += OnProgramFinished;
            runner.Run();
    }
        catch (Exception e) 
        {
            Console.WriteLine($"Some error occured!\n{e.Message}");
        }
    }

    public static void OnProgramFinished(object? sender, TimeSpan time)
    {
        Console.WriteLine("Program is finished!");
        Console.WriteLine($"Currunt Time: {DateTime.Now}");
        Console.WriteLine($"Code execution time: {time}");
    }

    public static void OnProgramStarted()
    {
        Console.WriteLine("Program is stared!");
        Console.WriteLine($"Currunt Time: {DateTime.Now}");
    }
}