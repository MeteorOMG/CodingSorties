using System;
using UnityEngine;

public static class MathfCalculator
{
    #region Sphere
    public static Vector3 PositionOnSphere(float radius, float upAngle, float forwardAngle, SphereCoordinatesType type) => type switch
    {
        SphereCoordinatesType.Center => PositionOnSphereCenter(radius, upAngle, forwardAngle),
        SphereCoordinatesType.Ring => PositionOnSphereRing(radius, upAngle, forwardAngle),
    };

    public static Vector3 PositionOnSphere(float radius, float upAngle, float forwardAngle, Vector3 origin, Quaternion rotation, SphereCoordinatesType type) => type switch
    {
        SphereCoordinatesType.Center => PositionOnSphereCenter(radius, upAngle, forwardAngle, origin, rotation),
        SphereCoordinatesType.Ring => PositionOnSphereRing(radius, upAngle, forwardAngle, origin, rotation)
    };

    public static Vector3 PositionOnSphereRing(float radius, float tAngle, float sAngle)
    {
        float x = radius * Mathf.Cos(sAngle * Mathf.Deg2Rad) * Mathf.Sin(tAngle * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(sAngle * Mathf.Deg2Rad) * Mathf.Sin(tAngle * Mathf.Deg2Rad);
        float z = radius * Mathf.Cos(tAngle * Mathf.Deg2Rad);
        return new Vector3(x, y, z);
    }

    public static Vector3 PositionOnSphereRing(float radius, float tAngle, float sAngle, Vector3 origin, Quaternion rotation)
    {
        Vector3 position = PositionOnSphereRing(radius, tAngle, sAngle);
        return (rotation * position) + origin;
    }

    public static Vector3 PositionOnSphereCenter(float radius, float upAngle, float forwardAngle)
    {
        float x = radius * Mathf.Cos(forwardAngle * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(forwardAngle * Mathf.Deg2Rad);
        return Quaternion.Euler(0f, upAngle - 90f, 0f) * new Vector3(x, y, 0); //-90 to make begining of axis at forward
    }

    public static Vector3 PositionOnSphereCenter(float radius, float upAngle, float forwardAngle, Vector3 origin, Quaternion rotation)
    {
        Vector3 position = PositionOnSphereCenter(radius, upAngle, forwardAngle);
        return (rotation * position) + origin;
    }  
    #endregion
}

public enum SphereCoordinatesType
{
    Center,
    Ring
}