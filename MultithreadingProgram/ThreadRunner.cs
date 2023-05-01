using System.Diagnostics;
using MultithreadingProgram.Models;

namespace MultithreadingProgram;

/**
 * Клас, який запускає і керує виконанням потоків
 */
public class ThreadRunner
{
    private readonly Stopwatch _stopwatch;
    private readonly Data _data;
    private readonly ThreadHelper _threadHelper;
    private readonly Thread[] _threads;

    public event EventHandler<StartModel> OnStarted;
    public event EventHandler<FinalModel> OnFinished;
    public ThreadRunner(int ThreadCount, int VectorSize)
    {
        ValidateData(ThreadCount, VectorSize);
        _data = new Data(ThreadCount, VectorSize);
        _stopwatch = new Stopwatch();
        _threadHelper = new ThreadHelper(ThreadCount);
        _threads = new Thread[ThreadCount];
    }
    private static void ValidateData(int threadCount, int vectorSize)
    {
        if (threadCount < 1)
        {
            throw new ArgumentException("The minimum thread count is 1.", nameof(threadCount));
        }
        if (vectorSize < threadCount)
        {
            throw new ArgumentException("The vector size must be equal to or greater than the thread count.", nameof(vectorSize));
        }
    }
    public void Run() 
    {
        var startModel = new StartModel()
        {
            P = _data.ThreadCount,
            N = _data.VectorSize
        };
        _stopwatch.Start();
        OnStarted?.Invoke(this, startModel);

        RunThreads();

        Array.ForEach(_threads, x => x.Join());
        _stopwatch.Stop();
        var finalModel = new FinalModel()
        {
            P = _data.ThreadCount,
            N = _data.VectorSize,
            time = _stopwatch.Elapsed
        };
        OnFinished?.Invoke(this, finalModel);
    }
    private void RunThreads()
    {
        for (int i = 0; i < _data.ThreadCount; i++)
        {
            _threads[i] = new(new ThreadStart(new MyThread(i + 1, _data, _threadHelper).Execute))
            {
                Name = $"Thread {i + 1}"
            };
            _threads[i].Start();
        }
    }
}
