using UnityEngine;

public class SensorSystem : MonoBehaviour
{
    public float maxDistance = 20f;

    public float[] GetSensors()
    {
        float[] sensors = new float[5];
        sensors[0] = CastRay(transform.forward); // front
        sensors[1] = CastRay(Quaternion.Euler(0, -30, 0) * transform.forward); // front-left
        sensors[2] = CastRay(Quaternion.Euler(0, 30, 0) * transform.forward);  // front-right
        sensors[3] = CastRay(Quaternion.Euler(0, -60, 0) * transform.forward); // left
        sensors[4] = CastRay(Quaternion.Euler(0, 60, 0) * transform.forward);  // right
        return sensors;
    }

    float CastRay(Vector3 dir)
    {
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, maxDistance))
            return hit.distance / maxDistance; // normalized
        return 1f;
    }

    void OnDrawGizmos()
    {
        // Draw rays for debugging
        Vector3[] directions = {
            transform.forward,
            Quaternion.Euler(0,-30,0)*transform.forward,
            Quaternion.Euler(0,30,0)*transform.forward,
            Quaternion.Euler(0,-60,0)*transform.forward,
            Quaternion.Euler(0,60,0)*transform.forward
        };
        Gizmos.color = Color.red;
        foreach (var d in directions)
            Gizmos.DrawRay(transform.position, d * maxDistance);
    }
}