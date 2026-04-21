using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour
{
    public NeuralNetwork net;

    public float learningRate = 0.01f;
    public int currentEpoch = 0;

    public bool isTraining = false;

    public List<float> errorHistory = new List<float>();

    //stored dataset
    private List<float[]> inputs = new List<float[]>();
    private List<float[]> outputs = new List<float[]>();

    void Awake()
    {
        // create neural network and assign learning rate
        net = new NeuralNetwork();
        net.learningRate = learningRate;
    }

    // load dataset into trainer
    public void LoadDataset(DatasetLoader loader)
    {
        inputs = loader.inputs;
        outputs = loader.outputs;

        Debug.Log("dataset loaded: " + inputs.Count + " samples");
    }

    // train one full pass over dataset (1 epoch)
    public float TrainEpoch()
    {
        if (inputs.Count == 0) return 0f;

        float totalError = 0f;

        for (int i = 0; i < inputs.Count; i++)
        {
            // forward pass
            float[] prediction = net.Forward(inputs[i]);

            // compute Mean Squared Error (MSE)
            for (int j = 0; j < prediction.Length; j++)
            {
                float diff = prediction[j] - outputs[i][j];
                totalError += diff * diff;
            }

            // backpropagation step
            net.Backward(inputs[i], outputs[i]);
        }

        currentEpoch++;

        float mse = totalError / inputs.Count;
        return mse;
    }

    // train multiple epochs in sequence
    public void TrainMultipleEpochs(int epochs)
    {
        isTraining = true;
        errorHistory.Clear();

        for (int e = 0; e < epochs; e++)
        {
            float error = TrainEpoch();
            errorHistory.Add(error);

            Debug.Log("epoch " + currentEpoch + " MSE: " + error);
        }

        isTraining = false;

        Debug.Log("training complete. Epochs: " + currentEpoch);
    }
}