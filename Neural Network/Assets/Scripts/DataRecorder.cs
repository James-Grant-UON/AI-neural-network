using UnityEngine;
using System.IO;

public class DataRecorder : MonoBehaviour
{
    public string fileName = "training_data.csv";

    [Header("Uniqueness Settings")]
    public float sensorThreshold = 0.05f; // Minimum sensor change required
    public float controlThreshold = 0.05f; // Minimum input change required

    private string filePath;

    // Last recorded values (used to prevent duplicate data)
    private float[] lastSensors;
    private float lastAccel;
    private float lastSteer;

    void Start()
    {
        filePath = Application.persistentDataPath + "/" + fileName;

        // Create file with headers if it doesn't exist
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath,
                "front,left30,right30,left60,right60,left,right,acceleration,steering\n");
        }
    }

    public void Record(float[] sensors, float accel, float steer)
    {
        // Skip if data is too similar to last entry
        if (!IsUnique(sensors, accel, steer)) return;

        // Format and append CSV line
        string line = string.Join(",", sensors) + "," + accel + "," + steer;
        File.AppendAllText(filePath, line + "\n");

        // Store last values for comparison
        lastSensors = (float[])sensors.Clone();
        lastAccel = accel;
        lastSteer = steer;
    }

    // Prevents recording nearly identical data (reduces dataset spam)
    bool IsUnique(float[] sensors, float accel, float steer)
    {
        if (lastSensors == null) return true;

        float sensorDiff = 0f;
        for (int i = 0; i < sensors.Length; i++)
            sensorDiff += Mathf.Abs(sensors[i] - lastSensors[i]);

        float controlDiff =
            Mathf.Abs(accel - lastAccel) +
            Mathf.Abs(steer - lastSteer);

        return !(sensorDiff < sensorThreshold &&
                 controlDiff < controlThreshold);
    }

    // Clears dataset file
    public void ResetData()
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        File.WriteAllText(filePath,
            "front,left30,right30,left60,right60,acceleration,steering\n");

        Debug.Log("Dataset reset");
    }
}