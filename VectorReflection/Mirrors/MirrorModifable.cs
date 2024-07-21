using UnityEngine;

public class MirrorModifable : MonoBehaviour, IReflective
{
    [SerializeField] private Vector3 extraRotation;

    public ReflectionHandler Decorate(ReflectionHandler reflectionHandler)
    {
        reflectionHandler.newDirection = Quaternion.Euler(extraRotation) * reflectionHandler.newDirection;
        return reflectionHandler;
    }
}
