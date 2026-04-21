using UnityEngine;
using System.IO;

public class DataRecorder : MonoBehaviour
{
    public string fileName = "training_data.csv";

    [Header("Settings")]
    public float sensorThreshold = 0.05f; // minimum sensor change required
    public float controlThreshold = 0.05f; // minimum input change required

    private string filePath;

    // last recorded values (used to prevent duplicate data)
    private float[] lastSensors;
    private float lastAccel;
    private float lastSteer;

    void Start()
    {
        filePath = Application.persistentDataPath + "/" + fileName;

        // create file with headers if it doesn't exist -- should always exist with current build but is a failsafe
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath,
                "front,left30,right30,left60,right60,left,right,acceleration,steering\n");
        }
    }

    public void Record(float[] sensors, float accel, float steer)
    {
        // skip if data is too similar to last entry
        if (!IsUnique(sensors, accel, steer)) return;

        // format and append CSV line
        string line = string.Join(",", sensors) + "," + accel + "," + steer;
        File.AppendAllText(filePath, line + "\n");

        // store last values for comparison
        lastSensors = (float[])sensors.Clone();
        lastAccel = accel;
        lastSteer = steer;
    }

    // prevents recording nearly identical data (reduces dataset spam) -- thinking about whether to change parameters or only do data thats exactly the same
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

    // clears dataset file
    public void ResetData()
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        File.WriteAllText(filePath,
            "front,left30,right30,left60,right60,left,right,acceleration,steering\n");

        Debug.Log("dataset reset");
    }
}