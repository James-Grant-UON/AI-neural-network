using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DatasetLoader
{
    public List<float[]> inputs = new List<float[]>();
    public List<float[]> outputs = new List<float[]>();

    public void Load(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("Dataset not found: " + path);
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++) // skip header
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(',');
            if (values.Length < 7) continue;

            float[] inp = new float[5];
            float[] outp = new float[2];

            for (int j = 0; j < 5; j++)
                inp[j] = float.Parse(values[j]);

            outp[0] = float.Parse(values[5]);
            outp[1] = float.Parse(values[6]);

            inputs.Add(inp);
            outputs.Add(outp);
        }

        Debug.Log("Loaded " + inputs.Count + " samples.");
    }
}