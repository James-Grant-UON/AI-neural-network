using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public float speed = 10f; // forward movement speed
    public float turnSpeed = 50f; // turning speed

    private Rigidbody rb;

    // stored spawn position (used for resetting)
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // prevent car from tipping over
        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
    }

    void Start()
    {
        // save initial position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // moves the car using Rigidbody for physics consistency
    public void Move(float accel, float steer)
    {
        // forward movement
        Vector3 move =
            transform.forward * accel * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + move);

        // rotation (steering)
        float turn = steer * turnSpeed * Time.fixedDeltaTime;

        Quaternion rotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * rotation);
    }

    // resets car to starting position after collision
    public void ResetCar()
    {
        // stop all motion
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // reset transform
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    // detect collisions with walls and reset
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ResetCar();
        }
    }
}