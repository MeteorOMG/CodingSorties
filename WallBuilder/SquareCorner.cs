using System;
using UnityEngine;

[Serializable]
public class SquareCorner : BaseFenceElement
{
    [SerializeField] private DirectionModifier directionModifier;

    public override float Area(float width) => width * width;

    public override FenceResult Create(Vector3 origin, Quaternion rotation, float width)
    {
        Vector3 centerPoint = (origin + (rotation * Vector3.forward * (width / 2f)));
        Quaternion newRotation = ModifyDirection(directionModifier, rotation);
        return new FenceResult(centerPoint + (newRotation * Vector3.forward * (width / 2f)), newRotation);
    }

    public override void DrawGizmos(Vector3 origin, Quaternion rotation, float width)
    {
        Gizmos.matrix = Matrix4x4.TRS(origin, rotation, Vector3.one);

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector3(0f, 0f, width / 2f), new Vector3(width, 0f, width));

        Gizmos.color = new Color(0.458f, 0.619f, 0.721f);
        Gizmos.DrawCube(new Vector3(0f, 0f, width / 2f), new Vector3(width, 0f, width));
    }

    public Quaternion ModifyDirection(DirectionModifier modifier, Quaternion initialRotation) => modifier switch
    {
        DirectionModifier.Forward => initialRotation,
        DirectionModifier.Left => Quaternion.Euler(0f, -90f, 0) * initialRotation,
        DirectionModifier.Right => Quaternion.Euler(0f, 90f, 0f) * initialRotation,
    };

    public enum DirectionModifier
    {
        Forward,
        Left,
        Right,
    }
}
