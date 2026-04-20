using System;

public class Layer
{
    public int inputs; // Number of input neurons
    public int neurons; // Number of neurons in this layer

    public float[,] weights; // Weight matrix (neurons x inputs)
    public float[] biases; // Bias values for each neuron
    public float[] outputs; // Output values after activation

    private Random rand = new Random();

    public Layer(int inputs, int neurons)
    {
        this.inputs = inputs;
        this.neurons = neurons;

        weights = new float[neurons, inputs];
        biases = new float[neurons];
        outputs = new float[neurons];

        InitWeights();
    }

    // Initialize weights randomly between -1 and 1
    void InitWeights()
    {
        for (int i = 0; i < neurons; i++)
            for (int j = 0; j < inputs; j++)
                weights[i, j] = (float)(rand.NextDouble() * 2 - 1);
    }
}