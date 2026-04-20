using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public float speed = 10f; // Forward movement speed
    public float turnSpeed = 50f; // Turning speed

    private Rigidbody rb;

    // Stored spawn position (used for resetting)
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Prevent car from tipping over
        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationZ;
    }

    void Start()
    {
        // Save initial position and rotation
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Moves the car using Rigidbody for physics consistency
    public void Move(float accel, float steer)
    {
        // Forward movement
        Vector3 move =
            transform.forward * accel * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + move);

        // Rotation (steering)
        float turn = steer * turnSpeed * Time.fixedDeltaTime;

        Quaternion rotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * rotation);
    }

    // Resets car to starting position after collision
    public void ResetCar()
    {
        // Stop all motion
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Reset transform
        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    // Detect collisions with walls and reset
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ResetCar();
        }
    }
}