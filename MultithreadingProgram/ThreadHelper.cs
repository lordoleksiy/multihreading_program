namespace MultithreadingProgram;

/**
 * Клас, який містить засоби синхронізації потоків
 **/
public class ThreadHelper
{
    public readonly Barrier InputBarrier; // barrier for synchronizing all threads after input
    public readonly Barrier CalcBBarrier; // barrier for synchronizing all threads after b calculation
    public readonly object locker; // object for thread-safety change of general variables
    public readonly ManualResetEvent finishEvent; // event for thread-safety otput for result
    private int threadCounter; // variable counter for MAnualResetEvent
    public ThreadHelper(int threadCount) 
    {
        InputBarrier = new Barrier(threadCount);
        CalcBBarrier = new Barrier(threadCount);
        locker = new object();
        finishEvent = new ManualResetEvent(false);
        threadCounter = threadCount;
    }

    public void SignalEndCalc()
    {
        if (Interlocked.Decrement(ref threadCounter) == 0)
        {
            finishEvent.Set();
        }
    }
}
