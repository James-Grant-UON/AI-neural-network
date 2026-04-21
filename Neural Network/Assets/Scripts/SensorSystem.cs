using UnityEngine;

public class SensorSystem : MonoBehaviour
{
    public float maxDistance = 20f; // maximum ray length

    // returns normalized distances (0 = very close, 1 = no obstacle)
    public float[] GetSensors()
    {
        return new float[]
        {
            CastRay(transform.forward), // Front side
            CastRay(Quaternion.Euler(0, -30, 0) * transform.forward), // Front-left
            CastRay(Quaternion.Euler(0, 30, 0) * transform.forward),  // Front-right
            CastRay(Quaternion.Euler(0, -60, 0) * transform.forward), // Far left
            CastRay(Quaternion.Euler(0, 60, 0) * transform.forward),  // Far right
            CastRay(-transform.right), // Left side
            CastRay(transform.right) // Right side
        };
    }

    // casts a ray and returns normalized distance
    float CastRay(Vector3 dir)
    {
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, maxDistance))
            return hit.distance / maxDistance;

        // no hit = max distance
        return 1f;
    }

    // draw rays in editor for debugging
    void OnDrawGizmos()
    {
        Vector3[] directions =
        {
            transform.forward,
            Quaternion.Euler(0,-30,0)*transform.forward,
            Quaternion.Euler(0,30,0)*transform.forward,
            Quaternion.Euler(0,-60,0)*transform.forward,
            Quaternion.Euler(0,60,0)*transform.forward,
            -transform.right,
            transform.right
        };

        for (int i = 0; i < directions.Length; i++)
        {
            if (Physics.Raycast(transform.position, directions[i], out RaycastHit hit, maxDistance))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, hit.point);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, directions[i] * maxDistance);
            }
        }
    }
}