using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NestedFenceElement : BaseFenceElement
{
    [SerializeReference, SubclassSelector] private List<IFenceElement> nestedElements;

    public override float Area(float width) => nestedElements.Sum(c => c.Area(width));

    public override FenceResult Create(Vector3 origin, Quaternion rotation, float width)
    {
        FenceResult result = new FenceResult(origin, rotation);
        //(Vector3 startOrigin, Quaternion startRotation) data = (origin, rotation);
        foreach (var element in nestedElements)
            result = element.Create(result, width);

        return result;
    }

    public override void DrawGizmos(Vector3 origin, Quaternion rotation, float width)
    {
        FenceResult result = new FenceResult(origin, rotation);
        foreach (var element in nestedElements)
        {
            element.DrawGizmos(result, width);
            result = element.Create(result, width);
        }
    }
}
