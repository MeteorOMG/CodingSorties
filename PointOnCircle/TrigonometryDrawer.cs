using System;
using System.Collections.Generic;
using UnityEngine;

public class TrigonometryDrawer : MonoBehaviour
{
    [SerializeField] private GraphSetup graphSetup = GraphSetup.Sine | GraphSetup.Cosine | GraphSetup.CoordinateSystem;
    [SerializeField, Range(-360f, 360f)] private float angleInDegrees;
    [SerializeField] private bool dynamicGraph = true;
    [SerializeField] private float animationSpeed = 70;

    [Header("Gizmos Settings")]
    [SerializeField] private Color coordinateSystemColor = Color.white;
    [SerializeField] private Color circleColor = Color.white;
    [SerializeField] private Color sineGraphColor = Color.green;
    [SerializeField] private Color cosineGraphColor = Color.red;

    [SerializeField] private Color pointOnCircleColor = Color.white;
    [SerializeField] private Color pointOnCircleSinConnect = Color.green;
    [SerializeField] private Color pointOnCircleCosineConnect = Color.red;

    private float FunctionPointSize => 0.05f;
    private float Range => Mathf.PI * 4f;
    private float GraphPointPrecision => 0.1f;
    private Vector3 CosineRotation => new Vector3(0f, 0f, 270f);

    //Be carefull! Dictionaries are not made to be constantly iterated through. I used dictionary here for sake of presentation. Consider using List in your implementation
    private Dictionary<GraphSetup, Action> graphSetupCallbacks = new Dictionary<GraphSetup, Action>();

    private void Update()
    {
        if (angleInDegrees > 360f)
            angleInDegrees = -360f;

        angleInDegrees += animationSpeed * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        foreach(var set in graphSetupCallbacks)
        {
            if ((set.Key & graphSetup) > 0)
                set.Value?.Invoke();
        }
    }

    #region Drawing
    private void DrawFunction(Func<float, float> TrigFunction, Color color, Vector3 rotation)
    {
        if (GraphPointPrecision == 0)
            return;

        float val = Mathf.Deg2Rad * angleInDegrees;
        float xPosition = dynamicGraph ? 0f : val;

        Matrix4x4 initialMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(rotation), Vector3.one);
        Gizmos.color = color;
        Gizmos.DrawWireSphere(new Vector3(xPosition, TrigFunction.Invoke(val), 0f), FunctionPointSize);
        Gizmos.DrawSphere(new Vector3(xPosition, TrigFunction.Invoke(val), 0f), FunctionPointSize);

        float subtractValue = dynamicGraph ? val : 0f;
        Vector2 prevPoint = new Vector2(-Range - subtractValue, TrigFunction.Invoke(-Range));
        
        for (float i = -Range; i <= Range; i += GraphPointPrecision)
        {
            Vector2 thisPoint = new Vector2(i - subtractValue, TrigFunction.Invoke(i));
            Gizmos.DrawLine(prevPoint, thisPoint);
            prevPoint = thisPoint;
        }

        Gizmos.matrix = initialMatrix;
    }

    private void DrawCircle()
    {
        Gizmos.color = circleColor;
        Gizmos.DrawWireSphere(Vector3.zero, 1f);
    }

    private void DrawPointOnCircle()
    {
        float val = Mathf.Deg2Rad * angleInDegrees;
        Vector2 point = new Vector2(Mathf.Cos(val), Mathf.Sin(val));

        Gizmos.color = pointOnCircleColor;
        Gizmos.DrawSphere(point, 0.1f);
        Gizmos.DrawWireSphere(point, 0.1f);
    }

    private void DrawSineLineConnection()
    {
        float val = Mathf.Deg2Rad * angleInDegrees;
        Vector2 point = new Vector2(Mathf.Cos(val), Mathf.Sin(val));

        Gizmos.color = pointOnCircleSinConnect;
        Gizmos.DrawLine(point, new Vector3(0f, Mathf.Sin(val)));
    }

    private void DrawCosineLineConnection()
    {
        float val = Mathf.Deg2Rad * angleInDegrees;
        Vector2 point = new Vector2(Mathf.Cos(val), Mathf.Sin(val));

        Gizmos.color = pointOnCircleCosineConnect;
        Vector3 point2 = new Vector3(0f, Mathf.Cos(val));
        Gizmos.DrawLine(point, Quaternion.Euler(CosineRotation) * point2);
    }

    private void DrawCoordinateSystem()
    {
        Gizmos.color = coordinateSystemColor;
        Gizmos.DrawLine(new Vector3(-Range, 0f), new Vector3(Range, 0));
        Gizmos.DrawLine(new Vector3(0, -Range), new Vector3(0, Range));
    }
    #endregion

    private void OnValidate()
    {
        CreateDrawingActions();
    }

    private void CreateDrawingActions()
    {
        graphSetupCallbacks = new Dictionary<GraphSetup, Action>()
        {
            {GraphSetup.Sine, () => DrawFunction(Mathf.Sin, sineGraphColor, Vector3.zero) },
            {GraphSetup.Cosine, () => DrawFunction(Mathf.Cos, cosineGraphColor, Vector3.zero) },
            {GraphSetup.CosineRotated, () => DrawFunction(Mathf.Cos, cosineGraphColor, CosineRotation) },
            {GraphSetup.SineLine, () => DrawSineLineConnection() },
            {GraphSetup.CosineLine, () => DrawCosineLineConnection() },
            {GraphSetup.Circle, () => DrawCircle() },
            {GraphSetup.PointOnCircle, () => DrawPointOnCircle() },
            {GraphSetup.CoordinateSystem, () => DrawCoordinateSystem() },
        };
    }

    [Flags]
    public enum GraphSetup
    {
        Sine = 1 << 1,
        Cosine = 1 << 2,
        CosineRotated = 1 << 3,
        SineLine = 1 << 4,
        CosineLine = 1 << 5,
        Circle = 1 << 6,
        PointOnCircle = 1 << 7,
        CoordinateSystem = 1 << 8
    }
}
