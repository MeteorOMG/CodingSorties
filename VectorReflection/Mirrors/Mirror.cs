using UnityEngine;

public class Mirror : MonoBehaviour, IReflective
{
    public ReflectionHandler Decorate(ReflectionHandler reflectionHandler) => reflectionHandler;
}
