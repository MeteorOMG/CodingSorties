using UnityEngine;

public class WeaponSpreadExample : MonoBehaviour
{
    [SerializeField] private Transform launchSpot;
    [SerializeField] private float timeBetweenShots = .1f;
    [SerializeField] private float rayDuration = 2f;

    [SerializeField, Range(0f, 180f)] private float upAngleExtents = 15f;
    [SerializeField, Range(0f, 180f)] private float forwardAngleExtents = 15f;
    [SerializeField] private SphereCoordinatesType coordsType;

    private float currentTime = 0f;
    private Vector3 lastRotation;

    private void Update()
    {
        if(currentTime >= timeBetweenShots)
        {
            lastRotation = GetLaunchRotationEuler();
            currentTime = 0f;
            Debug.DrawRay(launchSpot.transform.position, lastRotation, Color.red, rayDuration);
        }

        currentTime += Time.deltaTime;
    }

    public Vector3 GetLaunchRotationEuler()
    {
        float upAngleValue = Random.Range(-upAngleExtents, upAngleExtents);
        float forwardAngleValue = Random.Range(-forwardAngleExtents, forwardAngleExtents);

        Vector3 position = MathfCalculator.PositionOnSphere(1f, upAngleValue, forwardAngleValue, coordsType);
        return Quaternion.LookRotation(position) * launchSpot.forward;
    }

    public Quaternion GetLaunchRotationQuat() => Quaternion.Euler(GetLaunchRotationEuler());

    [ContextMenu("SetNewRotation")]
    public void SetNewRotation() => lastRotation = GetLaunchRotationEuler();
}
