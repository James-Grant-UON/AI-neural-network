using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DatasetLoader
{
    public List<float[]> inputs = new List<float[]>(); // Sensor inputs
    public List<float[]> outputs = new List<float[]>(); // Expected outputs (accel, steer)

    public void Load(string path)
    {
        // Ensure file exists before attempting to read
        if (!File.Exists(path))
        {
            Debug.LogWarning("Dataset not found: " + path);
            return;
        }

        string[] lines = File.ReadAllLines(path);

        // Skip first line (header row)
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] v = lines[i].Split(',');

            // 7 sensor inputs (raycast distances)
            float[] inp = new float[7];

            // 2 outputs: acceleration + steering
            float[] outp = new float[2];

            // 7 inputs
            for (int j = 0; j < 7; j++)
                inp[j] = float.Parse(v[j]);

            // outputs shift to index 7 and 8
            outp[0] = float.Parse(v[7]); // accel
            outp[1] = float.Parse(v[8]); // steer

            inputs.Add(inp);
            outputs.Add(outp);
        }

        Debug.Log("Loaded " + inputs.Count + " samples.");
    }
}