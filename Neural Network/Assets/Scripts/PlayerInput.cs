using UnityEngine;

[RequireComponent(typeof(CarController))]
public class PlayerInput : MonoBehaviour
{
    public SensorSystem sensors; //ref fir raycast sensor system
    public DataRecorder recorder; //saves training data

    public bool isActive = true;
    public bool recordData = true; // NEW toggle

    private CarController car;

    void Awake()
    {
        car = GetComponent<CarController>();
    }

    void FixedUpdate()
    {
        //no input if player  control isnt active
        if (!isActive) return;


        //keyboard input for moving
        float accel = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");


        //accel for Z axis and steer for x axis
        car.Move(accel, steer);

        //control the training data
        if (recordData)
        {
            float[] sensorValues = sensors.GetSensors();
            recorder.Record(sensorValues, accel, steer);
        }
    }
}