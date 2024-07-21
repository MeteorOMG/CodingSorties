using UnityEngine;

[System.Serializable]
public struct FenceResult
{
    public Vector3 endPoint;
    public Quaternion rotation;

    public FenceResult(Vector3 endPoint, Quaternion rotation)
    {
        this.endPoint = endPoint;
        this.rotation = rotation;
    }
}