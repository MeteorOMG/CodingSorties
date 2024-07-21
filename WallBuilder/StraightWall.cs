using System;
using UnityEngine;

[Serializable]
public class StraightWall : BaseFenceElement
{
    [SerializeField, Min(0.1f)] private float length;

    public override float Area(float width) => length * width;

    public override FenceResult Create(Vector3 origin, Quaternion rotation, float width)
    {
        return new FenceResult(origin + (rotation * Vector3.forward * length), rotation);
    }

    public override void DrawGizmos(Vector3 origin, Quaternion rotation, float width)
    {
        Gizmos.matrix = Matrix4x4.TRS(origin, rotation, Vector3.one);
        Vector3 center = Vector3.forward * (length / 2f);
        Vector3 size = new Vector3(width, 0f, length);

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(center, size);

        Gizmos.color = new Color(0.701f, 0.772f, 0.843f);
        Gizmos.DrawCube(center, size);
    }
}