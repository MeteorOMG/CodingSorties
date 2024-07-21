using System;
using UnityEngine;

[Serializable]
public class ScriptableHandler : BaseFenceElement
{
    [SerializeField] private ScriptableFenceElement targetScriptable;

    public override float Area(float width) => targetScriptable.Area(width);

    public override FenceResult Create(Vector3 origin, Quaternion rotation, float width) => targetScriptable.Create(origin, rotation, width);

    public override void DrawGizmos(Vector3 origin, Quaternion rotation, float width) => targetScriptable.DrawGizmos(origin, rotation, width);
}