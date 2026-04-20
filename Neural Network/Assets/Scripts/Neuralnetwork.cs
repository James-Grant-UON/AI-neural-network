using System;
//feedforward with backpropagation

public class NeuralNetwork
{
    public Layer l1, l2, l3;   // 3-layer network
    public float learningRate = 0.01f;

    public NeuralNetwork()
    {
        // Architecture: 7 inputs -> 10 -> 8 -> 2 outputs
        l1 = new Layer(7, 10);
        l2 = new Layer(10, 8);
        l3 = new Layer(8, 2);
    }

    // Activation function (tanh gives output range -1 to 1)
    private float Activate(float x) => (float)Math.Tanh(x);

    // Derivative of tanh (used in backpropagation)
    private float ActivateDerivative(float x) => 1 - x * x;

    // Forward pass input - output
    public float[] Forward(float[] input)
    {
        // ---- Layer 1 ----
        for (int i = 0; i < l1.neurons; i++)
        {
            float sum = l1.biases[i];

            for (int j = 0; j < l1.inputs; j++)
                sum += l1.weights[i, j] * input[j];

            l1.outputs[i] = Activate(sum);
        }

        // ---- Layer 2 ----
        for (int i = 0; i < l2.neurons; i++)
        {
            float sum = l2.biases[i];

            for (int j = 0; j < l2.inputs; j++)
                sum += l2.weights[i, j] * l1.outputs[j];

            l2.outputs[i] = Activate(sum);
        }

        // ---- Output Layer ----
        for (int i = 0; i < l3.neurons; i++)
        {
            float sum = l3.biases[i];

            for (int j = 0; j < l3.inputs; j++)
                sum += l3.weights[i, j] * l2.outputs[j];

            l3.outputs[i] = Activate(sum);
        }

        return l3.outputs;
    }

    // Backpropagation adjusts weights based on error
    public void Backward(float[] input, float[] expected)
    {
        // ---- Output layer error ----
        float[] delta3 = new float[l3.neurons];
        for (int i = 0; i < l3.neurons; i++)
        {
            delta3[i] =
                (l3.outputs[i] - expected[i]) *
                ActivateDerivative(l3.outputs[i]);
        }

        // ---- Hidden layer 2 error ----
        float[] delta2 = new float[l2.neurons];
        for (int i = 0; i < l2.neurons; i++)
        {
            float sum = 0;

            for (int j = 0; j < l3.neurons; j++)
                sum += delta3[j] * l3.weights[j, i];

            delta2[i] = sum * ActivateDerivative(l2.outputs[i]);
        }

        // ---- Hidden layer 1 error ----
        float[] delta1 = new float[l1.neurons];
        for (int i = 0; i < l1.neurons; i++)
        {
            float sum = 0;

            for (int j = 0; j < l2.neurons; j++)
                sum += delta2[j] * l2.weights[j, i];

            delta1[i] = sum * ActivateDerivative(l1.outputs[i]);
        }

        // ---- Update weights and biases ----

        // Output layer
        for (int i = 0; i < l3.neurons; i++)
        {
            for (int j = 0; j < l3.inputs; j++)
                l3.weights[i, j] -= learningRate * delta3[i] * l2.outputs[j];

            l3.biases[i] -= learningRate * delta3[i];
        }

        // Hidden layer 2
        for (int i = 0; i < l2.neurons; i++)
        {
            for (int j = 0; j < l2.inputs; j++)
                l2.weights[i, j] -= learningRate * delta2[i] * l1.outputs[j];

            l2.biases[i] -= learningRate * delta2[i];
        }

        // Hidden layer 1
        for (int i = 0; i < l1.neurons; i++)
        {
            for (int j = 0; j < l1.inputs; j++)
                l1.weights[i, j] -= learningRate * delta1[i] * input[j];

            l1.biases[i] -= learningRate * delta1[i];
        }
    }
}