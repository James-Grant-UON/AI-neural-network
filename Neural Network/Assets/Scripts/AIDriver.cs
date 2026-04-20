using UnityEngine;

[RequireComponent(typeof(CarController))]
public class AIDriver : MonoBehaviour
{
    public NeuralNetwork net; // Trained neural network
    public SensorSystem sensors; // Sensor input system

    private float smoothedSteer = 0f; // Helps reduce jittery steering
    public bool canDrive = false; // Toggle AI control

    private CarController car;

    void Awake()
    {
        car = GetComponent<CarController>();
    }

    void FixedUpdate()
    {
        if (!canDrive)
        {
            return;
        }
        // Get environment data from sensors
        float[] input = sensors.GetSensors();

        // Feed into neural network
        float[] output = net.Forward(input);

        // Output[0] = acceleration, Output[1] = steering
        float targetSteer = Mathf.Clamp(output[1], -1f, 1f);

        // Smooth steering to avoid sharp jitter
        smoothedSteer = Mathf.Lerp(smoothedSteer, targetSteer, 0.15f);

        // Clamp acceleration to keep car moving forward
        float accel = Mathf.Clamp(output[0], 0.3f, 1f);

        // Move the car
        car.Move(accel, smoothedSteer);
    }
}