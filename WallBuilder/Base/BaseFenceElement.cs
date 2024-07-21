using UnityEngine;

public abstract class BaseFenceElement : IFenceElement
{
    public abstract float Area(float width);
    public abstract FenceResult Create(Vector3 origin, Quaternion rotation, float width);
    public abstract void DrawGizmos(Vector3 origin, Quaternion rotation, float width);
}