namespace MultithreadingProgram;


/**
 * Клас потоку
 **/
public class MyThread
{
    private readonly int index;
    private readonly Data _data;
    private readonly ThreadHelper threadHelper;
    private readonly int startPos;
    private readonly int endPos;
    public MyThread(int index, Data data, ThreadHelper threadHelper)
    {
        this.index = index;
        _data = data;
        this.threadHelper = threadHelper;
        startPos = (index - 1) * _data.H;
        endPos = startPos + _data.H;

        // перевірка на випадок того, якщо к-сть матриць/к-сть потоків не дорівнює цілому числу
        if (index == _data.ThreadCount && endPos != _data.VectorSize)
        {
            endPos = _data.VectorSize;
        }
    }
    public void Execute()
    {
        if (index == 1)
        {
            // Введення даних в потоці 1:
            Array.Fill(_data.B, 1);
        }
        if (index == _data.ThreadCount)
        {
            // Введення даних в останньому потоці:
            Array.Fill(_data.C, 1);
            _data.MX = _data.FillMatrix(1);
            _data.MR = _data.FillMatrix(1);
        }
        // Очікування на завершення введення даних:
        threadHelper.InputBarrier.SignalAndWait();

        // Обчислення1 bi:
        int[] Bh = _data.B.Skip(startPos).Take(endPos - startPos).ToArray();
        int[] Ch = _data.C.Skip(startPos).Take(endPos - startPos).ToArray();
        int bi = Data.VectorMultiply(Bh, Ch);

        // Обчислення2 b:
        lock (threadHelper.locker)
        {
            _data.GeneralB += bi; 
        }

        // Очікування на кінець обчислення b:
        threadHelper.CalcBBarrier.SignalAndWait();

        // Обчислення3 〖MB〗_H  = MX*MR_H
        _data.GetMRH(out int[,] MRh, startPos, endPos); // отримали MR_H
        int[,] MBh = Data.MatrixMultiply(MRh, _data.MX);

        // Обчислення4 
        int xi = Data.MaxInMatrix(MBh);

        // Обчислення5 x:
        lock (threadHelper.locker)
        {
            _data.SetX(xi);
        }

        // Сигнал про завершення обчислень x:
        threadHelper.SignalEndCalc();
        if (index == 1)
        {
            // Очікування на кінець обчислень x:
            threadHelper.finishEvent.WaitOne();

            // копіювання x та b:
            int b1 = _data.GeneralB;
            int x1 = _data.GeneralX;

            // Обчислення6 a:
            _data.ResA = b1 + x1;

            // Вивід результату:
            Console.WriteLine($"Потік {index} завершив своє виконання");
            Console.WriteLine($"Результат: {_data.ResA}");
        }
    }
}
