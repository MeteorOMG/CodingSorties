using UnityEngine;

[ExecuteAlways]
public class LaserOrigin : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float laserRange = 100;
    [SerializeField] private int maxReflections = 5;
    [SerializeField] private bool debugMode = true;

    [Header("Path")]
    [SerializeField] private LaserPath path;

    private void Update()
    {
        if (debugMode)
            SendStartLaser();
    }

    [ContextMenu("SendStart")]
    public void SendStartLaser() => path = new LaserPath(transform.position, transform.forward, laserRange, maxReflections);
    
    private void OnDrawGizmos()
    {
        foreach (var hitInfo in path.reflections)
        {
            Gizmos.color = hitInfo.hitFound ? Color.green : Color.red;
            Vector3 laserEnd = hitInfo.hitFound ? hitInfo.hit : hitInfo.origin + (hitInfo.direction.normalized * laserRange);
            Gizmos.DrawLine(hitInfo.origin, laserEnd);
        }
    }
}