using System;

public class Layer
{
    public int inputs, neurons;
    public float[,] weights;
    public float[] biases;
    public float[] outputs;
    public float[] zValues; // weighted sums for backprop

    private Random rand = new Random();

    public Layer(int inputs, int neurons)
    {
        this.inputs = inputs;
        this.neurons = neurons;
        weights = new float[neurons, inputs];
        biases = new float[neurons];
        outputs = new float[neurons];
        zValues = new float[neurons];
        InitWeights();
    }

    void InitWeights()
    {
        for (int i = 0; i < neurons; i++)
            for (int j = 0; j < inputs; j++)
                weights[i, j] = (float)(rand.NextDouble() * 2 - 1);
    }
}