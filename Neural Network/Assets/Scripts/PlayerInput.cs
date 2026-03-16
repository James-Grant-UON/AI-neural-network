using UnityEngine;

[RequireComponent(typeof(CarController))]
public class PlayerInput : MonoBehaviour
{
    private CarController car;

    void Awake()
    {
        car = GetComponent<CarController>();
    }

    void Update()
    {
        float accel = Input.GetAxis("Vertical");   // W/S or Up/Down
        float steer = Input.GetAxis("Horizontal"); // A/D or Left/Right
        car.Move(accel, steer);
    }
}