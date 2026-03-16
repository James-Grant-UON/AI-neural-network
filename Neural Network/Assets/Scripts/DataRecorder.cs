using UnityEngine;
using System.IO;

public class DataRecorder : MonoBehaviour
{
    public SensorSystem sensors;
    public string fileName = "training_data.csv";

    private string filePath;

    void Start()
    {
        filePath = Application.persistentDataPath + "/" + fileName;
        if (sensors == null) sensors = GetComponent<SensorSystem>();
    }

    void Update()
    {
        if (sensors == null) return;

        float[] sensorValues = sensors.GetSensors();
        float accel = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");

        string line = string.Join(",", sensorValues) + "," + accel + "," + steer;
        File.AppendAllText(filePath, line + "\n");
    }

    public void ResetData()
    {
        filePath = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(filePath)) File.Delete(filePath);
        File.WriteAllText(filePath,
            "front,left30,right30,left60,right60,acceleration,steering\n");
        Debug.Log("Dataset reset: " + filePath);
    }
}