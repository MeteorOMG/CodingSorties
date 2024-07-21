using UnityEngine;

[System.Serializable]
public class ReflectionHandler
{
    public Vector3 origin;
    public Vector3 direction;

    public Vector3 hit;
    public Vector3 newDirection;

    public float range;
    public int reflectionsLeft;
    public bool hitFound;

    public ReflectionHandler(Vector3 origin, Vector3 direction, Vector3 hit, Vector3 newDirection, bool hitFound, float range, int reflectionsLeft)
    {
        this.origin = origin;
        this.direction = direction;
        this.hit = hit;
        this.newDirection = newDirection;
        this.hitFound = hitFound;
        this.range = range;
        this.reflectionsLeft = reflectionsLeft;
    }
}