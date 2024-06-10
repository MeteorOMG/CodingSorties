using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Unity.VisualScripting;

public class SpherePositionDrawer : MonoBehaviour
{
    [Header("MainSettings")]
    [SerializeField] private SphereCoordinatesType type;
    [SerializeField] private float radius = 1f;
    [SerializeField] private float forwardAngle = 90f;
    [SerializeField] private float upAngle = 90f;

    [Header("Gizmos Settings")]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] private bool showFullCircles = true;

    [SerializeField] private Color upPlaneColor = new Color(0, .85f, .81f, .5f);
    [SerializeField] private Color forwardPlaneColor = new Color(.22f, .57f, .86f, .5f);
    [SerializeField] private Color sphereColor = Color.green;
    [SerializeField] private Color textColor = Color.white;

    [SerializeField] private float sphereSize = .05f;
    [SerializeField] private int fontSize = 32;

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        DrawGizmos(type)?.Invoke();
    }

    private UnityAction DrawGizmos(SphereCoordinatesType type) => type switch
    {
        SphereCoordinatesType.Center => DrawAsCircle,
        SphereCoordinatesType.Ring => DrawAsRing,
    };

    #region Center Sphere Position Drawing
    private void DrawAsCircle()
    {
        Gizmos.color = sphereColor;
        Vector3 targetPosition = MathfCalculator.PositionOnSphereCenter(radius, upAngle, forwardAngle, transform.position, transform.rotation);
        Gizmos.DrawSphere(targetPosition, sphereSize);
        Gizmos.DrawWireSphere(targetPosition, sphereSize);

        Quaternion forwardRotation = transform.rotation * Quaternion.Euler(0f, upAngle, 0f);                        // Calculate new forward rotation. We need to take into considiration up angle and game object rotation
        Vector3 forwardDirection = (forwardRotation * Vector3.forward).normalized;                                  // Find direction based on rotation
        Vector3 normalDirection = Vector3.Cross(forwardDirection, transform.up);                                    // Find perpendicular Vector to establish Normal direction. 

        DrawArcs(transform.position, transform.up, transform.forward, upAngle, radius, upPlaneColor);               // UpAngle arc. Rotation starts always from transform.forward
        DrawArcs(transform.position, normalDirection, forwardDirection, forwardAngle, radius, forwardPlaneColor);   // Forward angle arc. Arc needs to point towards new forward (rotation and upangle)

        DrawLabel($"{(int)forwardAngle}°", transform.position + transform.up * radius / 2f, textColor);             // Drawing label in the halfway of sphere height
        DrawLabel($"{(int)upAngle}°", transform.position, textColor);                                               // Drawing label in the center

        DrawEdgeLines(forwardDirection);                                                            // Drawing remaining edge lines
    }

    private void DrawArcs(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius, Color color)
    {
        Handles.color = Color.white;
        if (showFullCircles)
            Handles.DrawWireArc(center, normal, from, 360f, radius);

        Handles.DrawWireArc(center, normal, from, Mathf.Clamp(angle, -360f, 360f), radius);                         // Draw outline of full arc to visualize angle.
        Handles.DrawWireArc(center, normal, from, Mathf.Clamp(angle, -360f, 360f), radius / 5f);                    // Draw smaller arc inside to help visualize angle placement.

        Handles.color = color;
        Handles.DrawSolidArc(center, normal, from, Mathf.Clamp(angle, -360f, 360f), radius);                        // Draw fill of full arc
    }

    private void DrawLabel(string text, Vector3 position, Color col)
    {
        text = $"<color=#{col.ToHexString()}>{text}</color>";
        Handles.Label(position, text, new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = fontSize
        });
    }

    private void DrawEdgeLines(Vector3 correctedForward)
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + correctedForward * radius);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * radius);
    }
    #endregion

    #region Ring Sphere Position Drawing
    private void DrawAsRing()
    {
        DrawAnglePlanes();
        DrawUpPlaneLines();
        DrawForwardPlaneLines();
    }

    private void DrawAnglePlanes()
    {
        Vector3 pos = MathfCalculator.PositionOnSphereRing(radius, upAngle, forwardAngle, transform.position, transform.rotation);

        Gizmos.color = sphereColor;
        Gizmos.DrawSphere(pos, sphereSize);
        Gizmos.DrawWireSphere(pos, sphereSize);

        float localZ = radius * Mathf.Cos(upAngle * Mathf.Deg2Rad);
        float localX = radius * Mathf.Sin(upAngle * Mathf.Deg2Rad);
        Vector3 inForwardPosition = TransformInto(new Vector3(0f, 0f, localZ));

        DrawArcs(transform.position, transform.up, transform.forward, upAngle, radius, upPlaneColor);
        DrawArcs(inForwardPosition, transform.forward, transform.right, forwardAngle, localX, forwardPlaneColor);
     
        DrawLabel($"{(int)forwardAngle}°", inForwardPosition, textColor);
        DrawLabel($"{(int)upAngle}°", transform.position, textColor);

        Gizmos.DrawLine(transform.position, TransformInto(new Vector3(localX, 0f, localZ)));
        Gizmos.DrawLine(inForwardPosition, pos);
    }

    private void DrawUpPlaneLines()
    {
        Gizmos.color = Color.white;
        float localZ = radius * Mathf.Cos(upAngle * Mathf.Deg2Rad);
        float localX = radius * Mathf.Sin(upAngle * Mathf.Deg2Rad);

        Vector3 posOnEdge = TransformInto(new Vector3(localX, 0f, localZ));
        Vector3 height = TransformInto(new Vector3(0f, 0f, localZ));

        //From center to circle edge
        Gizmos.DrawLine(transform.position, posOnEdge);
        Gizmos.DrawLine(transform.position, height);
        Gizmos.DrawLine(posOnEdge, height);
    }

    private void DrawForwardPlaneLines()
    {
        float localZ = radius * Mathf.Cos(upAngle * Mathf.Deg2Rad);

        Vector3 pos = MathfCalculator.PositionOnSphereRing(radius, upAngle, forwardAngle, transform.position, transform.rotation);
        Vector3 height = TransformInto(new Vector3(0f, 0f, localZ));

        Gizmos.DrawLine(height, pos);
        Gizmos.DrawLine(height, transform.position);
        Gizmos.DrawLine(pos, transform.position);
    }

    //Method to make that call more compact, easier to read
    private Vector3 TransformInto(Vector3 additional) => transform.TransformPoint(additional);
    #endregion
}