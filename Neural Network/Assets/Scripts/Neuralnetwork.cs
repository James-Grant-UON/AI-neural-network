using System;
//feedforward with backpropagation

public class NeuralNetwork
{
    public Layer layer1, layer2, layer3;   // 3-layer network
    public float learningRate = 0.01f;

    public NeuralNetwork()
    {
        // architecture: 7 inputs -> 10 -> 8 -> 2 outputs
        layer1 = new Layer(7, 10);
        layer2 = new Layer(10, 8);
        layer3 = new Layer(8, 2);
    }

    // activation function (tanh gives output range -1 to 1)
    private float Activate(float x) => (float)Math.Tanh(x);

    // derivative of tanh (used in backpropagation)
    private float ActivateDerivative(float x) => 1 - x * x;

    // forward pass input - output
    public float[] Forward(float[] input)
    {
        // ---- Layer 1 ----
        for (int i = 0; i < layer1.neurons; i++)
        {
            float sum = layer1.biases[i];

            for (int j = 0; j < layer1.inputs; j++)
                sum += layer1.weights[i, j] * input[j];

            layer1.outputs[i] = Activate(sum);
        }

        // ---- Layer 2 ----
        for (int i = 0; i < layer2.neurons; i++)
        {
            float sum = layer2.biases[i];

            for (int j = 0; j < layer2.inputs; j++)
                sum += layer2.weights[i, j] * layer1.outputs[j];

            layer2.outputs[i] = Activate(sum);
        }

        // ---- Output Layer ----
        for (int i = 0; i < layer3.neurons; i++)
        {
            float sum = layer3.biases[i];

            for (int j = 0; j < layer3.inputs; j++)
                sum += layer3.weights[i, j] * layer2.outputs[j];

            layer3.outputs[i] = Activate(sum);
        }

        return layer3.outputs;
    }

    // backpropagation adjusts weights based on error
    public void Backward(float[] input, float[] expected)
    {
        // -- output layer error ---
        float[] delta3 = new float[layer3.neurons];
        for (int i = 0; i < layer3.neurons; i++)
        {
            delta3[i] =
                (layer3.outputs[i] - expected[i]) *
                ActivateDerivative(layer3.outputs[i]);
        }

        // --- hidden layer 2 error ---
        float[] delta2 = new float[layer2.neurons];
        for (int i = 0; i < layer2.neurons; i++)
        {
            float sum = 0;

            for (int j = 0; j < layer3.neurons; j++)
                sum += delta3[j] * layer3.weights[j, i];

            delta2[i] = sum * ActivateDerivative(layer2.outputs[i]);
        }

        // -- hidden layer 1 error --
        float[] delta1 = new float[layer1.neurons];
        for (int i = 0; i < layer1.neurons; i++)
        {
            float sum = 0;

            for (int j = 0; j < layer2.neurons; j++)
                sum += delta2[j] * layer2.weights[j, i];

            delta1[i] = sum * ActivateDerivative(layer1.outputs[i]);
        }

        // -- update weights and biases --

        // output layer
        for (int i = 0; i < layer3.neurons; i++)
        {
            for (int j = 0; j < layer3.inputs; j++)
                layer3.weights[i, j] -= learningRate * delta3[i] * layer2.outputs[j];

            layer3.biases[i] -= learningRate * delta3[i];
        }

        // hidden layer 2
        for (int i = 0; i < layer2.neurons; i++)
        {
            for (int j = 0; j < layer2.inputs; j++)
                layer2.weights[i, j] -= learningRate * delta2[i] * layer1.outputs[j];

            layer2.biases[i] -= learningRate * delta2[i];
        }

        // hidden layer 1
        for (int i = 0; i < layer1.neurons; i++)
        {
            for (int j = 0; j < layer1.inputs; j++)
                layer1.weights[i, j] -= learningRate * delta1[i] * input[j];

            layer1.biases[i] -= learningRate * delta1[i];
        }
    }
}