using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LaserPath
{
    public List<ReflectionHandler> reflections = new List<ReflectionHandler>();

    public LaserPath(Vector3 startPosition, Vector3 direction, float range, int maxReflections)
    {
        reflections = new List<ReflectionHandler>();
        CreatePath(startPosition, direction, range, maxReflections);
    }

    public void CreatePath(Vector3 startPosition, Vector3 direction, float range, int maxReflections)
    {
        reflections.Clear();

        var laser = SendLaser(startPosition, direction, range, maxReflections);
        reflections.Add(laser);

        while (laser.hitFound)
        {
            laser = SendLaser(laser);
            reflections.Add(laser);
        }
    }

    public ReflectionHandler SendLaser(Vector3 from, Vector3 to, float range, int reflectionsLeft)
    {
        var hitRegistered = Physics.Raycast(new Ray(from + (to.normalized * Mathf.Epsilon), to), out RaycastHit hitInfo, range);

        if (hitRegistered && Vector3.Distance(hitInfo.point, from) != 0)
        {
            IReflective reflective = hitInfo.transform.GetComponent<IReflective>();
            if (reflective is not null)
            {
                Vector3 direction = Vector3.Reflect((hitInfo.point - from).normalized, hitInfo.normal);
                ReflectionHandler handler = new ReflectionHandler(from, to, hitInfo.point, direction.normalized, true, range, reflectionsLeft - 1);
                return reflective.Decorate(handler);
            }
        }

        return new ReflectionHandler(from, to, Vector3.zero, Vector3.zero, false, 0f, 0);
    }

    public ReflectionHandler SendLaser(ReflectionHandler previousResult) => SendLaser(previousResult.hit, previousResult.newDirection, previousResult.range, previousResult.reflectionsLeft);
}
