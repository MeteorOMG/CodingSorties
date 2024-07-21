using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class CircularWall : BaseFenceElement
{
    [SerializeField, Min(0f)] private float arcRadius = 2f;
    [SerializeField, Min(0f)] private float angle = 45f;
    [SerializeField] private bool leftTurn;

    public override float Area(float width)
    {     
        float radius = Mathf.Clamp(arcRadius, width, float.MaxValue);
        float areaBiggerCircle = (Mathf.PI * Mathf.Pow(radius, 2f)) * (angle / 360f);
        float areaSmallerCircle = (Mathf.PI * Mathf.Pow(radius - width, 2f)) * (angle / 360f);
        return areaBiggerCircle - areaSmallerCircle;
    }

    public override FenceResult Create(Vector3 origin, Quaternion rotation, float width)
    {
        float radius = Mathf.Clamp(arcRadius, width, float.MaxValue);
        float centerOffset = (radius - (width / 2f)) * (leftTurn ? -1f : 1f);
        Vector3 circleCenter = origin + (rotation * new Vector3(centerOffset, 0f, 0f));

        float angleDirectionCorrection = leftTurn ? angle : -angle + 180f;
        float xEnd = (radius - (width / 2f)) * Mathf.Cos(angleDirectionCorrection * Mathf.Deg2Rad);
        float zEnd = (radius - (width / 2f)) * Mathf.Sin(angleDirectionCorrection * Mathf.Deg2Rad);

        Vector3 endingPoint = circleCenter + (rotation * new Vector3(xEnd, 0f, zEnd));
        Vector3 newForwardDirection = Vector3.Cross((circleCenter - endingPoint).normalized, leftTurn ? Vector3.down : Vector3.up);

        return new FenceResult(endingPoint, Quaternion.LookRotation(newForwardDirection));      
    }

    public override void DrawGizmos(Vector3 origin, Quaternion rotation, float width)
    {
        Handles.matrix = Gizmos.matrix = Matrix4x4.TRS(origin, rotation, Vector3.one);

        float radius = Mathf.Clamp(arcRadius, width, float.MaxValue);
        float centerOffset = (radius - (width / 2f)) * (leftTurn ? -1f : 1f);

        Vector3 from = leftTurn ? Vector3.right : Vector3.left;
        Vector3 normal = leftTurn ? Vector3.down : Vector3.up;

        Handles.color = Gizmos.color = Color.white;
        Handles.DrawWireArc(new Vector3(centerOffset, 0f, 0f), normal, from, angle, radius);
        Handles.DrawWireArc(new Vector3(centerOffset, 0f, 0f), normal, from, angle, radius - (width));
        Gizmos.DrawLine(new Vector3(-width / 2f, 0f, 0f), new Vector3(width / 2f, 0f, 0f));

        var data = Create(origin, rotation, width);
        Handles.matrix = Gizmos.matrix = Matrix4x4.TRS(data.endPoint, data.rotation, Vector3.one);
        Gizmos.DrawLine(new Vector3(-width / 2f, 0f, 0f), new Vector3(width / 2f, 0f, 0f));
    }
}
