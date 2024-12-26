namespace Maths3DMatrices;

public static class MatrixElementaryOperations
{
    public static void SwapLines(MatrixInt matrix, int linePos, int linePos2)
    {
        int temp;
        for (int i = 0; i < matrix.NbColumns; i++)
        {
            temp = matrix[linePos,i];
            matrix[linePos,i] = matrix[linePos2,i];
            matrix[linePos2,i] = temp;
        }
    }

    public static void SwapLines(MatrixFloat matrix, int linePos, int linePos2)
    {
        float temp;
        for (int i = 0; i < matrix.NbColumns; i++)
        {
            temp = matrix[linePos,i];
            matrix[linePos,i] = matrix[linePos2,i];
            matrix[linePos2,i] = temp;
        }
    }
    
    public static void SwapColumns(MatrixInt matrix, int colPos, int colPos2)
    {
        int temp;
        for (int i = 0; i < matrix.NbLines; i++)
        {
            temp = matrix[i, colPos];
            matrix[i, colPos] = matrix[i, colPos2];
            matrix[i, colPos2] = temp;
        }
    }

    public static void MultiplyLine(MatrixInt matrix, int linePos, int multiplier)
    {
        if (multiplier == 0)
            throw new MatrixScalarZeroException();
        for (int i = 0; i < matrix.NbColumns; i++)
        {
            matrix[linePos,i] *= multiplier;
        }
    }
    
    public static void MultiplyLine(MatrixFloat matrix, int linePos, float multiplier)
    {
        if (multiplier == 0)
            throw new MatrixScalarZeroException();
        for (int i = 0; i < matrix.NbColumns; i++)
        {
            matrix[linePos,i] *= multiplier;
        }
    }

    public static void MultiplyColumn(MatrixInt matrix, int colPos, int multiplier)
    {
        if (multiplier == 0)
            throw new MatrixScalarZeroException();
        for (int i = 0; i < matrix.NbLines; i++)
        {
            matrix[i,colPos] *= multiplier;
        }
    }

    public static void AddLineToAnother(MatrixInt matrixInt, int line, int line2, int factor)
    {
        for (int i = 0; i < matrixInt.NbColumns; i++)
        {
            matrixInt[line2,i] += matrixInt[line,i] * factor;
        }
    }

    public static void AddLineToAnother(MatrixFloat matrixInt, int line, int line2, float factor)
    {
        for (int i = 0; i < matrixInt.NbColumns; i++)
        {
            matrixInt[line2,i] += matrixInt[line,i] * factor;
        }
    }
    
    public static void AddColumnToAnother(MatrixInt matrixInt, int col, int col2, int factor)
    {
        for (int i = 0; i < matrixInt.NbLines; i++)
        {
            matrixInt[i,col2] += matrixInt[i,col] * factor;
        }    
    }
}