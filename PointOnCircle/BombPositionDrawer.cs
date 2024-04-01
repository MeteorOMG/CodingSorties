using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BombPositionDrawer : MonoBehaviour
{
    [Header("Main params")]
    [SerializeField, Range(0f, 360f)] private float angleRange = 360f;
    [SerializeField] private float maxRadius = 1f;

    [Header("Hit Params")]
    [SerializeField] private float shotSize = 0.05f;
    [SerializeField] private float animSpeed = 15;

    [Header("Gizmos Colors")]
    [SerializeField] private Color arcColor = Color.green;
    [SerializeField] private Color arcRingColor = Color.white;
    [SerializeField] private Color shotColor = Color.red;

    [SerializeField] private List<ShotHandler> handlers = new List<ShotHandler>();

    [ContextMenu("Shoot")]
    public void ShootProjectile()
    {
        float radius = Random.Range(0f, maxRadius);
        float angle = Random.Range(0f, angleRange);
        Vector2 pos = GetPositionOnCircle(radius, angle);
        handlers.Add(new ShotHandler(radius, angle, shotSize, pos));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            ShootProjectile();

        if (Input.GetKeyDown(KeyCode.C))
            Clear();
    }

    #region Drawing
    private void OnDrawGizmos()
    {
        DrawArc();
        DrawCenter();
        DrawClosingLines();
        DrawShots();
    }

    private void DrawArc()
    {
        Handles.color = arcRingColor;
        Handles.DrawWireArc(Vector3.zero, Vector3.forward, Vector3.right, angleRange, maxRadius);
        Handles.color = arcColor;
        Handles.DrawSolidArc(Vector3.zero, Vector3.forward, Vector3.right, angleRange, maxRadius);
    }

    private void DrawCenter()
    {
        Gizmos.DrawSphere(Vector3.zero, 0.01f);
    }

    private void DrawClosingLines()
    {
        if (angleRange == 360f)
            return;

        Gizmos.color = arcRingColor;
        Gizmos.DrawLine(Vector3.zero, GetPositionOnCircle(maxRadius, 0f));
        Gizmos.DrawLine(Vector3.zero, GetPositionOnCircle(maxRadius, angleRange));
    }
    
    private void DrawShots()
    {
        Gizmos.color = shotColor;

        foreach(var shot in handlers)
        {
            shot.Update(animSpeed);
            Gizmos.DrawWireSphere(shot.hitPosition, shot.size);
            Gizmos.DrawSphere(shot.hitPosition, shot.size);
        }
    }
    #endregion

    private Vector2 GetPositionOnCircle(float radius, float angle)
    {
        return new Vector2(radius * Mathf.Cos(angle * Mathf.Deg2Rad), radius * Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    private void Clear()
    {
        handlers.Clear();
    }    
}

[System.Serializable]
public class ShotHandler
{
    public float radius;
    public float angle;
    public Vector3 hitPosition;

    public float size;
    public float targetSize;

    public ShotHandler(float radius, float angle, float targetSize, Vector3 hitPosition)
    {
        this.radius = radius;
        this.angle = angle;
        this.hitPosition = hitPosition;
        this.targetSize = targetSize;
    }

    public void Update(float speed)
    {
        size = Mathf.Lerp(size, targetSize, Time.deltaTime * speed);
    }
}