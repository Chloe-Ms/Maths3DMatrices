namespace Maths3DMatrices;

public struct Quaternion
{
    public float x { get; set; } 
    public float y { get; set; } 
    public float z { get; set; } 
    public float w { get; set; }
    
    public static Quaternion Identity = new Quaternion(0f, 0f, 0f, 1f);

    public MatrixFloat Matrix
    {
        get
        {
            MatrixFloat matrix = new MatrixFloat(new[,]
            {
                { 1 - 2*y*y - 2*z*z, 2*x*y - 2*w*z, 2*x*z + 2*w*y, 0 },
                { 2*x*y + 2*w*z, 1 - 2*x*x - 2*z*z, 2*y*z - 2*w*x, 0 },
                { 2*x*z - 2*w*y, 2*y*z + 2*w*x, 1 - 2*x*x - 2*y*y, 0 },
                { 0, 0, 0, 1 }
            });
            
            return matrix;
        }
    }

    public Vector3 EulerAngles
    {
        get
        {
            MatrixFloat matrixQuaternion = Matrix;
            float p = MathF.Asin(-matrixQuaternion[1,2]);
            float h,b;
            if (MathF.Cos(p) != 0)
            {
                h = MathF.Atan2(matrixQuaternion[0,2],matrixQuaternion[2,2]);
                b = MathF.Atan2(matrixQuaternion[1,0],matrixQuaternion[1,1]);
            }
            else //gimbal lock
            {
                h = MathF.Atan2(-matrixQuaternion[2,0],matrixQuaternion[0,0]);
                b = 0;
            }
            Vector3 eulerAngles = new Vector3(p,h,b) * (180 / MathF.PI);
            return eulerAngles;
        }
    }

    public Quaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static float GetLengthSquared(Quaternion q)
    {
        return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
    }

    public static float GetLength(Quaternion q)
    {
        return (float)Math.Sqrt(GetLengthSquared(q));
    }
    
    public static Quaternion AngleAxis(float angle, Vector3 axis)
    {
        float magnitude = MathF.Sqrt(axis.x * axis.x + axis.y * axis.y + axis.z * axis.z);
        Vector3 unitVector = new Vector3(axis.x / magnitude, axis.y / magnitude, axis.z / magnitude);

        float radiansAngle = (float)((Math.PI / 180f) * angle);
        return new Quaternion(unitVector.x * MathF.Sin(radiansAngle / 2f), 
            unitVector.y * MathF.Sin(radiansAngle / 2f), 
            unitVector.z * MathF.Sin(radiansAngle / 2f), 
            MathF.Cos(radiansAngle / 2f));
    }

    public static Quaternion operator *(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(
            q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y,
            q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x,
            q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w,
            q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
            );
    }
    
    public static Quaternion operator *(Quaternion q, float value)
    {
        return new Quaternion(
            q.x * value,
            q.y * value,
            q.z * value,
            q.w * value
        );
    }
    
    private Quaternion GetConjugate()
    {
        return new Quaternion(-x, -y, -z, w);
    }

    private static Quaternion GetConjugate(Quaternion q)
    {
        return q.GetConjugate();
    }

    public static Quaternion GetUnitQuaternion(Quaternion q)
    {
        float resultLength = GetLength(q);
        return new Quaternion(q.x / resultLength, q.y / resultLength, q.z / resultLength, q.w / resultLength);
    }
    
    public static Vector3 operator *(Quaternion q, Vector3 point)
    {
        Quaternion pointQuaternion = new Quaternion(point.x, point.y, point.z, 0);
        Quaternion inverseQ = q.GetConjugate() * (1 / GetLengthSquared(q));
        Quaternion result = q * pointQuaternion * inverseQ;
        return new Vector3(result.x, result.y, result.z); 
    }

    public static Quaternion Euler(float roll, float pitch, float yaw)
    {
        //X = roll, Y = pitch, Z = yaw
        Quaternion qRX = AngleAxis(roll, new Vector3(1, 0, 0));
        Quaternion qRY = AngleAxis(pitch, new Vector3(0, 1, 0));
        Quaternion qRZ = AngleAxis(yaw, new Vector3(0, 0, 1));
        
        return qRY * qRX * qRZ;
    }
}