using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controller
// Manages the boids and each generation of the neural network

public class Controller : MonoBehaviour
{
    public List<NeuralNetwork> neuralNetworks;
    public List<GameObject> spawnedObjects;

    public float timePerGeneration; // (seconds)

    public int numberToSpawn;
    public Vector3 positionToSpawn;
    public GameObject objectPrefab;

    public Transform target;

    void Start()
    {
        InitializeNeuralNetworks(numberToSpawn); // Initializes n amount of neural networks and adds them to 'neuralNetworks' list

        StartCoroutine(newGeneration()); // newGeneration() is called every 15 seconds
    }

    IEnumerator newGeneration()
    {
        // Destroys every gameobject of the previous generation
        foreach (GameObject go in spawnedObjects)
        {
            GameObject.Destroy(go);
        }

        spawnedObjects = new List<GameObject>();

        target.transform.position = new Vector3(Random.Range(-7.0f, 7.0f), Random.Range(-4.0f, 4.0f));

        // Instantiates new generation of objects and assigns neural networks
        for (int i = 0; i < neuralNetworks.Count; i++)
        {
            GameObject go = Instantiate(objectPrefab);
            go.GetComponent<AIScript>().neuralNetwork = neuralNetworks[i];

            go.transform.position = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);

            spawnedObjects.Add(go);
        }

        yield return new WaitForSeconds(timePerGeneration);

        SortAscending();

        Debug.Log(neuralNetworks.Count/2);

        for (int i = 0; i < (int)(numberToSpawn/2); i++) // Removes the worst 50% of neural networks by fitness
        {
            neuralNetworks.RemoveAt(0);
        }        

        List<NeuralNetwork> newNeuralNetworks = new List<NeuralNetwork>();

        Debug.Log(neuralNetworks.Count);

        for (int i = 0; i < neuralNetworks.Count; i++) // Iterates through top 50% of neural networks with the greatest fitness
        {
            NeuralNetwork mutatedNeuralNetwork = neuralNetworks[i].CreateMutatedNeuralNetwork(); // Creates a mutated version of a neural network to add to 'neuralNetworks' list
            newNeuralNetworks.Add(mutatedNeuralNetwork);
        }
        neuralNetworks.AddRange(newNeuralNetworks); // Combines the list of the top 50% of neural networks with the list of the new mutated neural networks

        StartCoroutine(newGeneration()); // Repeats function
    }

    void SortAscending()
    {
        List<NeuralNetwork> sortingList = new List<NeuralNetwork>(); // List to insert values
        // ---
        // Insertion Sort
        // Neural networks sorted in ascending order by fitness
        // ---
        while (neuralNetworks.Count != 0)
        {
            int index = 0;
            for (int i = 0; i < sortingList.Count; i++)
            {
                index = i;
                if (neuralNetworks[0].fitness <= sortingList[i].fitness)
                {
                    break;
                }
            }
            sortingList.Insert(index, neuralNetworks[0]);
            neuralNetworks.RemoveAt(0);
        }

        neuralNetworks = sortingList;
    }

    void InitializeNeuralNetworks(int numberOfNeuralNetworks)
    {
        neuralNetworks = new List<NeuralNetwork>();

        for (int i = 0; i < numberOfNeuralNetworks; i++)
        {
            NeuralNetwork neuralNetwork = new NeuralNetwork(1, 1, 2, 4);
            neuralNetwork.CreateNeurons();

            neuralNetworks.Add(neuralNetwork);
        }
    }
}
