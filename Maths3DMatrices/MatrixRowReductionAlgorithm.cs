namespace Maths3DMatrices;

public class MatrixRowReductionAlgorithm
{
    public static (MatrixFloat m1, MatrixFloat m2) Apply(MatrixFloat m1, MatrixFloat m2, bool throwException = false)
    {
        MatrixFloat augmentedMatrix = MatrixFloat.GenerateAugmentedMatrix(m1, m2);
        for (int col = 0; col < m1.NbColumns; col++)
        {
            int indexMaxValue = GetIndexHighestNumberInColumn(augmentedMatrix, col);
            if (indexMaxValue != -1)
            {
                //Swap lines with max index
                if (indexMaxValue != col)
                {
                    MatrixElementaryOperations.SwapLines(augmentedMatrix,indexMaxValue, col);
                }

                var test = augmentedMatrix[col, col];
                //Multiply by inverse to get 1
                MatrixElementaryOperations.MultiplyLine(augmentedMatrix,col,1 / augmentedMatrix[col, col]);

                //Add to other lines
                for (int i = 0; i < augmentedMatrix.NbLines; i++)
                {
                    if (col != i)
                    {
                        MatrixElementaryOperations.AddLineToAnother(augmentedMatrix,col,i,-augmentedMatrix[i,col]);
                    }
                }
            }
            else if (throwException)
            {
                throw new MatrixInvertException();
            }
        }
        
        return augmentedMatrix.Split(m1.NbColumns - 1);
    }

    static int GetIndexHighestNumberInColumn(MatrixFloat matrix, int column)
    {
        int maxIndex = -1;
        float maxValue = 0;
        float columnValue;
        for (int i = column; i < matrix.NbLines; i++)
        {
            columnValue = matrix[i, column];
            if ((maxIndex == -1 && columnValue != 0) ||
                (maxValue != -1 && columnValue > maxValue))
            {
                maxIndex = i;
                maxValue = columnValue;
            }
        }
        return maxIndex;
    }
}