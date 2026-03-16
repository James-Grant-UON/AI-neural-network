using UnityEngine;

public class AIDriver : MonoBehaviour
{
    public NeuralNetwork net;
    public SensorSystem sensors;
    public CarController car;

    void Update()
    {
        if (net == null || sensors == null || car == null) return;

        float[] inputs = sensors.GetSensors();
        float[] output = net.Forward(inputs);

        car.Move(output[0], output[1]);
    }
}