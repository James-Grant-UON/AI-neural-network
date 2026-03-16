using UnityEngine;

public class Trainer : MonoBehaviour
{
    public string fileName = "training_data.csv";
    public int epochs = 2;
    public float learningRate = 0.01f;

    [HideInInspector] public NeuralNetwork net;
    [HideInInspector] public DatasetLoader data;
    [HideInInspector] public bool IsTrainingDone = false;

    private int currentEpoch = 0;
    private int sampleIndex = 0;

    void Awake()
    {
        string path = Application.persistentDataPath + "/" + fileName;
        data = new DatasetLoader();
        data.Load(path);

        net = new NeuralNetwork();
        net.learningRate = learningRate;

        IsTrainingDone = false;
        currentEpoch = 0;
        sampleIndex = 0;

        this.enabled = false; // controlled by GameManager
    }

    public void StartTraining()
    {
        if (data.inputs.Count == 0)
        {
            Debug.LogError("No dataset found! Record data first.");
            return;
        }

        IsTrainingDone = false;
        currentEpoch = 0;
        sampleIndex = 0;
        this.enabled = true;
    }

    void Update()
    {
        if (IsTrainingDone || data.inputs.Count == 0) return;

        float[] input = data.inputs[sampleIndex];
        float[] expected = data.outputs[sampleIndex];

        net.Forward(input);
        net.Backward(input, expected);

        sampleIndex++;
        if (sampleIndex >= data.inputs.Count)
        {
            sampleIndex = 0;
            currentEpoch++;

            // Optional MSE calculation per epoch
            float mse = 0;
            for (int i = 0; i < data.inputs.Count; i++)
            {
                float[] pred = net.Forward(data.inputs[i]);
                for (int j = 0; j < pred.Length; j++)
                    mse += Mathf.Pow(pred[j] - data.outputs[i][j], 2);
            }
            mse /= data.inputs.Count;

            Debug.Log("Epoch " + currentEpoch + " completed. MSE = " + mse);

            if (currentEpoch >= epochs)
            {
                IsTrainingDone = true;
                this.enabled = false;
                Debug.Log("Training complete!");
            }
        }
    }

    public NeuralNetwork GetTrainedNetwork()
    {
        return net;
    }
}