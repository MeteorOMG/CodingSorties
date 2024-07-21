using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Fence : MonoBehaviour
{
    [Header("Main Params")]
    [SerializeField, Min(0f)] private float widht;
    [SerializeField] private float startRotation;
    [SerializeField] private Vector3 startPos;

    [SerializeReference, SubclassSelector] private List<IFenceElement> fences;
    [field: SerializeField] public float Area { get; private set; }

    private void OnDrawGizmos()
    {
        FenceResult data = new FenceResult(startPos, Quaternion.Euler(0f, startRotation, 0f));

        foreach (var fence in fences)
        {
            fence.DrawGizmos(data.endPoint, data.rotation, widht);
            data = fence.Create(data.endPoint, data.rotation, widht);
        }

        Area = fences.Sum(c => c.Area(widht));
    }
}
