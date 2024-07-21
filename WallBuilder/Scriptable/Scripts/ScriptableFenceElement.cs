using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable, CreateAssetMenu]
public class ScriptableFenceElement : ScriptableObject, IFenceElement
{
    [field: SerializeReference, SubclassSelector] public List<IFenceElement> NestedElements { get; private set; }

    public float Area(float width) => NestedElements.Sum(c => c.Area(width));

    public FenceResult Create(Vector3 origin, Quaternion rotation, float width)
    {
        FenceResult data = new FenceResult(origin, rotation);
        foreach (var element in NestedElements)
            data = element.Create(data, width);

        return data;
    }

    public void DrawGizmos(Vector3 origin, Quaternion rotation, float width)
    {
        FenceResult data = new FenceResult(origin, rotation);
        foreach (var element in NestedElements)
        {
            element.DrawGizmos(data, width);
            data = element.Create(data, width);
        }
    }
}