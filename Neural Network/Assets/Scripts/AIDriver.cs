using UnityEngine;

public class AIDriver : MonoBehaviour
{
    public NeuralNetwork net; // trained neural network
    public SensorSystem sensors; // sensor input system

    private float smoothedSteer = 0f; // selps reduce jittery steering
    public bool canDrive = false; // toggle AI control

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
        // get environment data from sensors
        float[] input = sensors.GetSensors();

        // feed into neural network
        float[] output = net.Forward(input);

        // output[0] = acceleration, output[1] = steering
        float targetSteer = Mathf.Clamp(output[1], -1f, 1f);

        // smooth steering to avoid sharp jitter
        smoothedSteer = Mathf.Lerp(smoothedSteer, targetSteer, 0.15f);

        // clamp acceleration to keep car moving forward
        float accel = Mathf.Clamp(output[0], 0.3f, 1f);

        // move the car
        car.Move(accel, smoothedSteer);
    }
}