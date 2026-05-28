using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NEURAL NETWORK
// Holds input, hidden, output layers, and weights of neural network
// feed forward function
// mutate function
// stores fitness of neural network

public class NeuralNetwork
{
    public float fitness = 0;

    public int numberOfInputs;
    public int numberOfOutputs;
    public int numberOfHiddenLayers;
    public int neuronsInEachLayer;


    public Neuron [][] neurons;

    public NeuralNetwork(int numberOfInputs, int numberOfOutputs, int numberOfHiddenLayers, int neuronsInEachLayer)
    {
        this.numberOfInputs = numberOfInputs;
        this.numberOfOutputs = numberOfOutputs;
        this.numberOfHiddenLayers = numberOfHiddenLayers;
        this.neuronsInEachLayer = neuronsInEachLayer;

        neurons = new Neuron [numberOfHiddenLayers + 1][]; // Jagged Array representing all of the neurons in the neural network
        // CreateNeurons();
    }

    public void CreateNeurons()
    {
        // Create first hidden layer
        CreateLayer(neurons, numberOfInputs, neuronsInEachLayer, 0); // Each neuron has a number of weights going into it

        // Create all other hidden layers
        for (int i = 1; i < numberOfHiddenLayers; i++)
        {
            CreateLayer(neurons, neuronsInEachLayer, neuronsInEachLayer, i);
        }

        // Create output layer
        CreateLayer(neurons, neuronsInEachLayer, numberOfOutputs, numberOfHiddenLayers);
    }

    List<float> CreateRandomWeights(float numberOfWeights)
    {
        List<float> weights = new List<float>();

        for (int i = 0; i < numberOfWeights; i++)
        {
            weights.Add(Random.Range(-0.50f, 0.50f));
        }

        return weights; // A list of values between 0 and 1
    }

    void CreateLayer(Neuron [][] Neurons, int nWeights, int nNeurons, int index, float manualWeight = -2) // index within the neural network array
    {
        List<Neuron> layer = new List<Neuron>();
        for (int i = 0; i < nNeurons; i++)
        {
            layer.Add(new Neuron(CreateRandomWeights(nWeights)));
        }
        Neurons[index] = layer.ToArray();
    }


    public List<float> FeedForward(float[] inputs)
    {

        // Iterate through every neuron in the first hidden layer
        for (int j = 0; j < neuronsInEachLayer; j++)
        {
            float value = 0.25f; // constant bias

            // Iterate through weights
            for (int k = 0; k < numberOfInputs; k++)
            {
                value += neurons[0][j].weights[k] * inputs[k]; // neutron value += weight * input value
            }
            neurons[0][j].value = (float)System.Math.Tanh(value);
            //neurons[0][j].value = value;
        }

        // Iterate through every layer
        for (int i = 1; i < neurons.Length; i++)
        {
            int nWeights = neurons[i][0].weights.Count;

            // Iterate through every neuron
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0.25f; // constant bias

                // Iterate through weights
                for (int k = 0; k < nWeights; k++)
                {
                    value += neurons[i][j].weights[k] * neurons[i - 1][k].value; // neutron value = weight * neutron value in previous layer
                }
                neurons[i][j].value = (float)System.Math.Tanh(value);
                //neurons[i][j].value = value;
            }
        }

        List<float> outputs = new List<float>();
        // Iterate through output layer
        for (int i = 0; i < numberOfOutputs; i++)
        {
            outputs.Add(neurons[neurons.Length - 1][i].value); // Adds value of each output neuron to 'outputs' list
        }
        return outputs;
    }

    public NeuralNetwork CreateMutatedNeuralNetwork()
    {
        Neuron [][] mutatedNeurons = new Neuron [numberOfHiddenLayers + 1][];

        // Create Deepcopy
        // in order to ensure the mutated neural network is not stored as a reference
        for (int i = 0; i < neurons.Length; i++)
        {
            List<Neuron> layer = new List<Neuron>();

            int nWeights = neurons[i][0].weights.Count;

            for (int j = 0; j < neurons[i].Length; j++)
            {
                List<float> weights = new List<float>();

                for (int k = 0; k < nWeights; k++)
                {
                    weights.Add(MutateWeight(neurons[i][j].weights[k])); // Change weight and add to a list of weights
                }
                layer.Add(new Neuron(weights));
            }
            mutatedNeurons[i] = layer.ToArray();
        }

        // Testing
        // Debug.Log("Comparing Mutated to Orginal:");
        // Debug.Log(mutatedNeurons[1][3].weights[2]);
        // Debug.Log(neurons[1][3].weights[2]);

        NeuralNetwork mutatedNeuralNetwork = new NeuralNetwork(numberOfInputs, numberOfOutputs, numberOfHiddenLayers, neuronsInEachLayer);
        mutatedNeuralNetwork.neurons = mutatedNeurons;

        return mutatedNeuralNetwork;
    }

    public float MutateWeight(float weight)
    {
        float probability = Random.Range(0.000f, 1.000f);

        if (probability <= 0.008f)
        {
            return Random.Range(-0.50f, 0.50f);
        }
        return weight;
    }
}

// NEURON CLASS
// holds value in neuron and list of incoming weights

public class Neuron
{
    public float value;
    public List<float> weights;

    public Neuron(List<float> weights)
    {
        this.weights = weights;
    }
}
