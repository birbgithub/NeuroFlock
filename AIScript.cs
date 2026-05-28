using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AISCRIPT
// assigned to each boid, to control a single boid's movement
// rewards boid's neural network based on proximity to target

public class AIScript : MonoBehaviour
{
    public NeuralNetwork neuralNetwork;

    public float speed = 1;  // speed of boid
    public float rotationSpeed = 20; // rotation speed of boid

    public Transform target;

    void Start()
    {
        neuralNetwork.fitness = 0;  // base fitness of neural network
        target = GameObject.FindGameObjectWithTag("Target").transform;
    }

    void FixedUpdate()
    {
        float[] inputs = {Vector3.Angle(transform.up, transform.position - target.position)/360};
        List<float> outputs = neuralNetwork.FeedForward(inputs);

        if (outputs[0] >= 0)
        {
            transform.eulerAngles += new Vector3(0, 0, 1) * Time.deltaTime * rotationSpeed;
        }
        else
        {
            transform.eulerAngles += new Vector3(0, 0, -1) * Time.deltaTime * rotationSpeed;
        }

        transform.position += transform.up * Time.deltaTime * speed;

        neuralNetwork.fitness += 1/(Vector3.Distance(transform.position, target.position));
        
        // The closer a boid is to the target, the greater fitness it recieves
        // This means that the neural network is rewarded for moving toward the target
    }
}
