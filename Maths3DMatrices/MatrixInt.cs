namespace Maths3DMatrices;

public class MatrixInt
{
    int[,] _matrix;
    
    public int NbLines { get; private set; }
    public int NbColumns { get; private set; }
    public int[,] ToArray2D()
    {
        return _matrix;
    }
    public bool IsIdentity()
    {
        if (NbLines != NbColumns)
        {
            return false;
        }
        bool isIdentity = true;
        int i = 0;
        int j;
        
        while (isIdentity && i < NbLines)
        {
            j = 0;
            while (isIdentity && j < NbColumns)
            {
                if ((i == j && _matrix[i, j] != 1) || (i != j && _matrix[i, j] != 0))
                {
                    isIdentity = false;
                }
                j++;
            }
            i++;
        }
        return isIdentity;
    }
    
    public int this[int i, int j]
    {
        get { return _matrix[i, j]; }
        set => _matrix[i, j] = value;
    }
    
    public MatrixInt(int numLines, int numCols)
    {
        NbLines = numLines;
        NbColumns = numCols;
        _matrix = new int[NbLines, NbColumns];
    }

    public MatrixInt(int[,] matrix)
    {
        _matrix = matrix;
        NbLines = matrix.GetLength(0);
        NbColumns = matrix.GetLength(1);

    }

    public MatrixInt(MatrixInt matrixToCopy)
    {
        NbLines = matrixToCopy.NbLines;
        NbColumns = matrixToCopy.NbColumns;
        _matrix = new int[matrixToCopy.NbLines, matrixToCopy.NbColumns];
        for (int row = 0; row < matrixToCopy.NbLines; row++)
        {
            for (int col = 0; col < matrixToCopy.NbColumns; col++)
            {
                _matrix[row, col] = matrixToCopy[row, col];
            }
        }
    }

    public static MatrixInt Identity(int dimension)
    {
        MatrixInt matrix = new MatrixInt(dimension, dimension);
        for (int i = 0; i < dimension; i++)
        {
           matrix[i, i] = 1; 
        }
        return matrix;
    }
    
    public void Multiply(int matrixInt)
    {
        for (int row = 0; row < NbLines; row++)
        {
            for (int col = 0; col < NbColumns; col++)
            {
                _matrix[row, col] *= matrixInt;
            }
        }
    }
    
    public static MatrixInt Multiply(MatrixInt matrixInt, int multiplier)
    {
        MatrixInt newMatrix = new MatrixInt(matrixInt);
        newMatrix.Multiply(multiplier);
        return newMatrix;
    }
    
    public MatrixInt Multiply(MatrixInt matrixInt)
    {
        if (NbColumns != matrixInt.NbLines)
        {
            throw new MatrixMultiplyException();
        }
        MatrixInt newMatrix = new MatrixInt(NbLines, matrixInt.NbColumns);
        int total;
        
        for (int row = 0; row < newMatrix.NbLines; row++)
        {
            for (int col = 0; col < newMatrix.NbColumns; col++)
            {
                total = 0;
                for (int k = 0; k < NbColumns; k++)
                {
                    total += _matrix[row, k] * matrixInt[k, col];
                }
                newMatrix[row, col] = total;
            }
        }
        return newMatrix;
    }

    public static MatrixInt Multiply(MatrixInt m1, MatrixInt m2)
    {
        return m1.Multiply(m2);
    }

    public static MatrixInt operator *(MatrixInt m1, MatrixInt m2)
    {
        return MatrixInt.Multiply(m1, m2);
    }
    
    public static MatrixInt operator *(MatrixInt m, int multiplier)
    {
        return MatrixInt.Multiply(m, multiplier);
    }
    
    public static MatrixInt operator *(int multiplier,MatrixInt m)
    {
        return m * multiplier;
    }
    
    public static MatrixInt operator -(MatrixInt m)
    {
        return m * -1;
    }

    public void Add(MatrixInt m)
    {
        if (NbLines != m.NbLines || NbColumns != m.NbColumns)
        {
            throw new MatrixSumException();
        }
        for (int row = 0; row < NbLines; row++)
        {
            for (int col = 0; col < NbColumns; col++)
            {
                _matrix[row, col] += m[row, col];
            }
        }
    }

    public static MatrixInt Add(MatrixInt m1, MatrixInt m2)
    {
        MatrixInt newMatrix = new MatrixInt(m1);
        newMatrix.Add(m2);
        return newMatrix;
    }
    
    public static MatrixInt operator +(MatrixInt m1, MatrixInt m2)
    {
        return MatrixInt.Add(m1, m2);
    }
    
    public static MatrixInt operator -(MatrixInt m1, MatrixInt m2)
    {
        return MatrixInt.Add(m1, -m2);
    }

    public MatrixInt Transpose()
    {
        MatrixInt newMatrix = new MatrixInt( NbColumns,NbLines);
        for (int row = 0; row < NbLines; row++)
        {
            for (int col = 0; col < NbColumns; col++)
            {
                newMatrix[col, row] = _matrix[row, col];
            }
        }
        return newMatrix;
    }

    public static MatrixInt Transpose(MatrixInt matrix)
    {
        return matrix.Transpose();
    }

    public static MatrixInt GenerateAugmentedMatrix(MatrixInt m1, MatrixInt m2)
    {
        MatrixInt newMatrix = new MatrixInt(m1.NbLines, m1.NbColumns + m2.NbColumns);
        for (int row = 0; row < newMatrix.NbLines; row++)
        {
            for (int col = 0; col < newMatrix.NbColumns; col++)
            {
                if (col < m1.NbColumns)
                {
                    newMatrix[row, col] = m1[row, col];
                }
                else
                {
                    newMatrix[row, col] = m2[row, col - m1.NbColumns];
                }
            }
        }
        return newMatrix;
    }

    public (MatrixInt m1, MatrixInt m2) Split(int i)
    {
        MatrixInt transfoMatrix = new MatrixInt(NbLines, i + 1);
        MatrixInt resultMatrix = new MatrixInt(NbLines,NbColumns - (i + 1));
        
        for (int row = 0; row < NbLines; row++)
        {
            for (int col = 0; col < NbColumns; col++)
            {
                if (col <= i)
                {
                    transfoMatrix[row, col] = _matrix[row, col];
                }
                else
                {
                    resultMatrix[row,col - (i + 1)] = _matrix[row, col];
                }
            }
        }
        
        return (transfoMatrix,resultMatrix);
    }
}