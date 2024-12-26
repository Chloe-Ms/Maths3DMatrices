namespace Maths3DMatrices;

public class Vector4
{
    public float x;
    public float y;
    public float z;
    public float w;

    public Vector4(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static Vector4 operator *(MatrixFloat m, Vector4 v)
    {
        MatrixFloat vectorMatrix = new MatrixFloat(4,1);
        vectorMatrix[0, 0] = v.x;
        vectorMatrix[1, 0] = v.y;
        vectorMatrix[2, 0] = v.z;
        vectorMatrix[3, 0] = v.w;
        
        vectorMatrix = m * vectorMatrix;

        return new Vector4(vectorMatrix[0, 0], vectorMatrix[1, 0],vectorMatrix[2, 0],vectorMatrix[3, 0]);
    }
}