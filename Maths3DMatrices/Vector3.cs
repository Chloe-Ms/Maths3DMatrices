namespace Maths3DMatrices;

public class Vector3
{
    public float x;
    public float y;
    public float z;

    public Vector3() { }

    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public static Vector3 operator +(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }
    
    public static Vector3 operator -(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }
    
    public static Vector3 operator *(Vector3 v, float value)
    {
        return new Vector3(v.x * value, v.y * value, v.z * value);
    }
}