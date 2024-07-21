using UnityEngine;

public interface IFenceElement
{
    float Area(float width);

    FenceResult Create(Vector3 origin, Quaternion rotation, float width);
    FenceResult Create(FenceResult result, float width) => Create(result.endPoint, result.rotation, width);

    void DrawGizmos(Vector3 origin, Quaternion rotation, float width);
    void DrawGizmos(FenceResult result, float width) => DrawGizmos(result.endPoint, result.rotation, width);
}