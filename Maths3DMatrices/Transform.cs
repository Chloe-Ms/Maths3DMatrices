namespace Maths3DMatrices;

public class Transform
{
    private Transform _parent;

    private MatrixFloat _localTranslationMatrix;
    private MatrixFloat _localRotationMatrix;
    public Vector3 LocalPosition { get; set; }

    public Vector3 LocalRotation { get; set; }

    public Vector3 LocalScale { get; set; }

    public MatrixFloat LocalTranslationMatrix
    {
        get
        {
            MatrixFloat matrixPosition = MatrixFloat.Identity(4);
            matrixPosition[0, 3] = LocalPosition.x;
            matrixPosition[1, 3] = LocalPosition.y;
            matrixPosition[2, 3] = LocalPosition.z;
            return matrixPosition;
        }
    }

    public MatrixFloat LocalRotationMatrix => LocalRotationYMatrix * LocalRotationXMatrix * LocalRotationZMatrix;

    public MatrixFloat LocalRotationXMatrix
    {
        get
        {
            MatrixFloat localRotationXMatrix = MatrixFloat.Identity(4);
            float rotationXInRad = LocalRotation.x * MathF.PI / 180;
            localRotationXMatrix[1, 1] = MathF.Cos(rotationXInRad);
            localRotationXMatrix[2, 2] = MathF.Cos(rotationXInRad);
            localRotationXMatrix[1, 2] = - MathF.Sin(rotationXInRad);
            localRotationXMatrix[2, 1] = MathF.Sin(rotationXInRad);
            return localRotationXMatrix;
        } 
    }
    
    public MatrixFloat LocalRotationYMatrix
    {
        get
        {
            MatrixFloat localRotationYMatrix = MatrixFloat.Identity(4);
            float rotationYInRad = LocalRotation.y * MathF.PI / 180;
            localRotationYMatrix[0, 0] = MathF.Cos(rotationYInRad);
            localRotationYMatrix[2, 2] = MathF.Cos(rotationYInRad);
            localRotationYMatrix[2, 0] = - MathF.Sin(rotationYInRad);
            localRotationYMatrix[0, 2] = MathF.Sin(rotationYInRad);
            return localRotationYMatrix;
        }
    }
    
    public MatrixFloat LocalRotationZMatrix
    {
        get
        {
            MatrixFloat localRotationZMatrix = MatrixFloat.Identity(4);
            float rotationZInRad = LocalRotation.z * MathF.PI / 180;
            localRotationZMatrix[0, 0] = MathF.Cos(rotationZInRad);
            localRotationZMatrix[1, 1] = MathF.Cos(rotationZInRad);
            localRotationZMatrix[0, 1] = - MathF.Sin(rotationZInRad);
            localRotationZMatrix[1, 0] = MathF.Sin(rotationZInRad);
            return localRotationZMatrix;
        } 
    }

    public MatrixFloat LocalScaleMatrix
    {
        get
        {
            MatrixFloat localScaleMatrix = MatrixFloat.Identity(4);
            localScaleMatrix[0, 0] = LocalScale.x;
            localScaleMatrix[1, 1] = LocalScale.y;
            localScaleMatrix[2, 2] = LocalScale.z;
            return localScaleMatrix;
        } 
    }

    public MatrixFloat _localToWorldMatrix;

    public MatrixFloat LocalToWorldMatrix => _parent == null ?
        LocalTranslationMatrix * LocalRotationMatrix * LocalScaleMatrix:
        _parent.LocalToWorldMatrix * LocalTranslationMatrix * LocalRotationMatrix * LocalScaleMatrix;
    
    public MatrixFloat WorldToLocalMatrix => LocalToWorldMatrix.InvertByDeterminant();

    public Vector3 WorldPosition
    {
        set
        {
            MatrixFloat translationLocalMatrix = new MatrixFloat(3, 1);
            translationLocalMatrix[0, 0] = LocalToWorldMatrix[0, 3];
            translationLocalMatrix[1, 0] = LocalToWorldMatrix[1, 3];
            translationLocalMatrix[2, 0] = LocalToWorldMatrix[2, 3];
            MatrixFloat matrixUVW = MatrixFloat.SubMatrix(LocalToWorldMatrix, 3, 3);
            matrixUVW = matrixUVW.InvertByDeterminant();
            MatrixFloat mod = new MatrixFloat(3, 1);
            mod[0, 0] = value.x;
            mod[1, 0] = value.y;
            mod[2, 0] = value.z;
            MatrixFloat resultMatrix  = matrixUVW * (mod - translationLocalMatrix);
            LocalPosition = new Vector3(resultMatrix[0, 0], resultMatrix[1, 0], resultMatrix[2, 0]);
        }
        get
        {
            return _parent == null
                ? LocalPosition
                : new Vector3(
                    LocalToWorldMatrix[0, 3], 
                    LocalToWorldMatrix[1, 3], 
                    LocalToWorldMatrix[2, 3]);
        }
    }

    public Quaternion LocalRotationQuaternion
    {
        get
        {
            return Quaternion.Euler(LocalRotation.x, LocalRotation.y, LocalRotation.z);
        }
        set
        {
            LocalRotation = value.EulerAngles;
        }
    }

    public void SetParent(Transform parent) => _parent = parent;
    
    public Transform()
    {
        LocalPosition = new Vector3();
        LocalRotation = new Vector3();
        LocalScale = new Vector3(1f,1f,1f);
    }
}