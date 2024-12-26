namespace Maths3DMatrices;

public class MatrixFloat
{
    float[,] _matrix;
    
    public int NbLines { get; private set; }
    public int NbColumns { get; private set; }
    public float[,] ToArray2D()
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
    
    public float this[int i, int j]
    {
        get { return _matrix[i, j]; }
        set => _matrix[i, j] = value;
    }
    
    public MatrixFloat(int numLines, int numCols)
    {
        NbLines = numLines;
        NbColumns = numCols;
        _matrix = new float[NbLines, NbColumns];
    }

    public MatrixFloat(float[,] matrix)
    {
        _matrix = matrix;
        NbLines = matrix.GetLength(0);
        NbColumns = matrix.GetLength(1);

    }

    public MatrixFloat(MatrixFloat matrixToCopy)
    {
        NbLines = matrixToCopy.NbLines;
        NbColumns = matrixToCopy.NbColumns;
        _matrix = new float[matrixToCopy.NbLines, matrixToCopy.NbColumns];
        for (int row = 0; row < matrixToCopy.NbLines; row++)
        {
            for (int col = 0; col < matrixToCopy.NbColumns; col++)
            {
                _matrix[row, col] = matrixToCopy[row, col];
            }
        }
    }

    public static MatrixFloat Identity(int dimension)
    {
        MatrixFloat matrix = new MatrixFloat(dimension, dimension);
        for (int i = 0; i < dimension; i++)
        {
           matrix[i, i] = 1; 
        }
        return matrix;
    }
    #region Multiply
    public void Multiply(float MatrixFloat)
    {
        for (int row = 0; row < NbLines; row++)
        {
            for (int col = 0; col < NbColumns; col++)
            {
                _matrix[row, col] *= MatrixFloat;
            }
        }
    }
    
    public static MatrixFloat Multiply(MatrixFloat matrixFloat, float multiplier)
    {
        MatrixFloat newMatrix = new MatrixFloat(matrixFloat);
        newMatrix.Multiply(multiplier);
        return newMatrix;
    }
    
    public MatrixFloat Multiply(MatrixFloat MatrixFloat)
    {
        if (NbColumns != MatrixFloat.NbLines)
        {
            throw new MatrixMultiplyException();
        }
        MatrixFloat newMatrix = new MatrixFloat(NbLines, MatrixFloat.NbColumns);
        float total;
        
        for (int row = 0; row < newMatrix.NbLines; row++)
        {
            for (int col = 0; col < newMatrix.NbColumns; col++)
            {
                total = 0;
                for (int k = 0; k < NbColumns; k++)
                {
                    total += _matrix[row, k] * MatrixFloat[k, col];
                }
                newMatrix[row, col] = total;
            }
        }
        return newMatrix;
    }

    public static MatrixFloat Multiply(MatrixFloat m1, MatrixFloat m2)
    {
        return m1.Multiply(m2);
    }
    #endregion
    
    public static MatrixFloat operator *(MatrixFloat m1, MatrixFloat m2)
    {
        return MatrixFloat.Multiply(m1, m2);
    }
    
    public static MatrixFloat operator *(MatrixFloat m, float multiplier)
    {
        return MatrixFloat.Multiply(m, multiplier);
    }
    
    public static Vector3 operator *(MatrixFloat m, Vector3 v)
    {
        MatrixFloat newMat = new MatrixFloat(3,1);
        newMat[0, 0] = v.x;
        newMat[1, 0] = v.y;
        newMat[2, 0] = v.z;
        MatrixFloat resMat = MatrixFloat.Multiply(m, newMat);
        return new Vector3(resMat[0, 0], resMat[1, 0], resMat[2, 0]);
    }
    
    public static MatrixFloat operator *(float multiplier,MatrixFloat m)
    {
        return m * multiplier;
    }
    
    public static MatrixFloat operator -(MatrixFloat m)
    {
        return m * -1;
    }

    public void Add(MatrixFloat m)
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

    public static MatrixFloat Add(MatrixFloat m1, MatrixFloat m2)
    {
        MatrixFloat newMatrix = new MatrixFloat(m1);
        newMatrix.Add(m2);
        return newMatrix;
    }
    
    public static MatrixFloat operator +(MatrixFloat m1, MatrixFloat m2)
    {
        return MatrixFloat.Add(m1, m2);
    }
    
    public static MatrixFloat operator -(MatrixFloat m1, MatrixFloat m2)
    {
        return MatrixFloat.Add(m1, -m2);
    }

    public MatrixFloat Transpose()
    {
        MatrixFloat newMatrix = new MatrixFloat( NbColumns,NbLines);
        for (int row = 0; row < NbLines; row++)
        {
            for (int col = 0; col < NbColumns; col++)
            {
                newMatrix[col, row] = _matrix[row, col];
            }
        }
        return newMatrix;
    }

    public static MatrixFloat Transpose(MatrixFloat matrix)
    {
        return matrix.Transpose();
    }

    public static MatrixFloat GenerateAugmentedMatrix(MatrixFloat m1, MatrixFloat m2)
    {
        MatrixFloat newMatrix = new MatrixFloat(m1.NbLines, m1.NbColumns + m2.NbColumns);
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

    public (MatrixFloat m1, MatrixFloat m2) Split(int i)
    {
        MatrixFloat transfoMatrix = new MatrixFloat(NbLines, i + 1);
        MatrixFloat resultMatrix = new MatrixFloat(NbLines,NbColumns - (i + 1));
        
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

    public static MatrixFloat InvertByRowReduction(MatrixFloat matrixFloat)
    {
        MatrixFloat identity = MatrixFloat.Identity(matrixFloat.NbLines);
        MatrixFloat m1;
        (_, m1) = MatrixRowReductionAlgorithm.Apply(matrixFloat,identity,true);
        return m1;
    }
    
    public MatrixFloat InvertByRowReduction()
    {
        return MatrixFloat.InvertByRowReduction(this);
    }

    #region SubMatrix
    public MatrixFloat SubMatrix(int row, int col)
    {
        MatrixFloat resultMatrix = new MatrixFloat(NbLines - 1, NbColumns - 1);
        int resultCol;
        int resultRow = 0;
        for (int i = 0; i < NbLines; i++)
        {
            resultCol = 0;
            for (int j = 0; j < NbColumns; j++)
            {
                if (i != row && j != col)
                {
                    resultMatrix[resultRow,resultCol] = _matrix[i,j];
                    resultCol++;
                }
            }

            if (i != row)
            {
                resultRow++;
            }
        }
        return resultMatrix;
    }

    public static MatrixFloat SubMatrix(MatrixFloat m, int row, int col) => m.SubMatrix(row, col);
    #endregion

    public static float Determinant(MatrixFloat matrix)
    {
        float result = 0;
        if (matrix.NbColumns == 1)
        {
            result = matrix[0, 0];
        }
        else if (matrix.NbColumns == 2)
        {
            result = matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
        }
        else
        {
            int row = 0;
            int sign = 1;
            for (int i = 0; i < matrix.NbColumns; i++)
            {
                MatrixFloat subMatrix = MatrixFloat.SubMatrix(matrix, row, i);
                sign = i % 2 == 0 ? 1 : -1;
                result += sign * matrix[row,i] * MatrixFloat.Determinant(subMatrix);
            }
        }
        
        return result;
    }

    public MatrixFloat Adjugate()
    {
        MatrixFloat transpose = MatrixFloat.Transpose(this);
        MatrixFloat result = new MatrixFloat(transpose.NbLines, transpose.NbColumns);
        for (int i = 0; i < NbLines; i++)
        {
            for (int j = 0; j < NbColumns; j++)
            { 
                MatrixFloat subMatrix = MatrixFloat.SubMatrix(transpose, i, j);
                int sign = (i + j) % 2 == 0 ? 1 : -1;
                result[i, j] = MatrixFloat.Determinant(subMatrix) * sign;
            }
        }
        return result;
    }
    
    public static MatrixFloat Adjugate(MatrixFloat matrix) => matrix.Adjugate();

    public MatrixFloat InvertByDeterminant()
    {
        float determinant = MatrixFloat.Determinant(this);
        if (determinant == 0)
        {
            throw new MatrixInvertException();
        }
        return 1 / determinant * Adjugate();
    }

    public static MatrixFloat InvertByDeterminant(MatrixFloat matrix) => matrix.InvertByDeterminant();
}