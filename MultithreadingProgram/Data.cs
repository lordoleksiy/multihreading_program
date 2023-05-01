namespace MultithreadingProgram;

/**
 * Клас, який містить усі необхідні дані для програми
 **/
public class Data
{
    /**
    * Задаємо початкові дані
    * P - кількість процесів
    * N - розмірність векторів
    **/
    public readonly int ThreadCount;
    public readonly int VectorSize;
    public readonly int H;
    public int[] B { get; set; }
    public int[] C { get; set; }
    public int[,] MX { get; set; }
    public int[,] MR { get; set; }

    // спільні ресурси x, b та результат a:
    public int GeneralX { get; set; }
    public int GeneralB { get; set; }
    public int ResA { get; set; }
    public Data(int threadCount, int vectorSize)
    {
        VectorSize = vectorSize;
        ThreadCount = threadCount;
        this.H = vectorSize / threadCount;

        B = new int[vectorSize];
        C = new int[vectorSize];
        MX = new int[vectorSize, vectorSize];
        MR = new int[vectorSize, vectorSize];
    }

    public void SetX(int X)
    {
        GeneralX = Math.Max(X, GeneralX);
    }

    public int[,] FillMatrix(int num)
    {
        int[,] matrix = new int[VectorSize, VectorSize];
        Enumerable.Range(0, VectorSize).ToList().ForEach(k =>
            Enumerable.Range(0, VectorSize).ToList().ForEach(j => matrix[k, j] = num));
        return matrix;
    }

    public static int VectorMultiply(int[] vector1, int[] vector2)
    {
        return vector1.Zip(vector2, (x1, x2) => x1 * x2).Sum();
    }

    public static int[,] MatrixMultiply(int[,] a, int[,] b)
    {
        int rowsA = a.GetLength(0);
        int colsA = a.GetLength(1);
        int rowsB = b.GetLength(0);
        int colsB = b.GetLength(1); 

        if (colsA != rowsB)
            throw new ArgumentException("The number of columns of the first matrix must be equal to the number of rows of the second matrix.");

        int[,] result = new int[rowsA, colsB];

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                int sum = 0;
                for (int k = 0; k < colsA; k++)
                {
                    sum += a[i, k] * b[k, j];
                }
                result[i, j] = sum;
            }
        }

        return result;
    }

    public void GetMRH(out int[,] MRh, int startPos, int endPos)
    {
        int count = endPos - startPos;
        MRh = new int[count, VectorSize];
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < VectorSize; j++)
            {
                MRh[i, j] = MR[i+startPos, j];
            }
        }  
    }

    public static int MaxInMatrix(int[,] matrix)
    {
        return matrix.Cast<int>().Max();
    }
}
