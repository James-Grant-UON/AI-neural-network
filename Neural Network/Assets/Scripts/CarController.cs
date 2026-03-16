using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 10f;
    public float turnSpeed = 50f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Move(float accel, float steer)
    {
        Vector3 move = transform.forward * accel * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        float turn = steer * turnSpeed * Time.fixedDeltaTime;
        Quaternion rotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * rotation);
    }
}