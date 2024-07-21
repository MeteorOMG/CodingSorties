using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class RoundCorner : BaseFenceElement
{    
    [SerializeField] private bool leftTurn;

    public override float Area(float width) => (Mathf.PI * Mathf.Pow(width, 2f)) / 4f;

    public override FenceResult Create(Vector3 origin, Quaternion rotation, float width)
    {
        Vector3 centerPoint = (origin + (rotation * Vector3.forward * (width / 2f)));
        Quaternion newRotation = ModifyDirection(leftTurn, rotation);
        return new FenceResult(centerPoint + (newRotation * Vector3.forward * (width / 2f)), newRotation);
    }

    public Quaternion ModifyDirection(bool left, Quaternion initialRotation)
    {
        Quaternion multiplier = left ? Quaternion.Euler(0f, -90f, 0f) : Quaternion.Euler(0f, 90f, 0f);
        return multiplier * initialRotation;
    }

    public override void DrawGizmos(Vector3 origin, Quaternion rotation, float width)
    {
        Vector3 movement = leftTurn ? Vector3.left : Vector3.right;
        Vector3 from = leftTurn ? Vector3.right : Vector3.left;
        Vector3 center = (movement * width) / 2f;
        float angle = leftTurn ? -90f : 90f;

        Handles.matrix = Matrix4x4.TRS(origin, rotation, Vector3.one);

        Handles.color = Color.white;
        Handles.DrawWireArc(center, Vector3.up, from, angle, width);
        Handles.DrawLine(movement * (width / 2f), (movement * (width / 2f)) + Vector3.forward * width);
        Handles.DrawLine(new Vector3(-width / 2f, 0f, 0f), new Vector3(width / 2f, 0f, 0f));

        Handles.color = new Color(0.450f, 0.572f, 0.717f);
        Handles.DrawSolidArc(center, Vector3.up, from, angle, width);
    }
}
