<h1>Flocking Boids - Evolutionary Neural Network</h1>

<ins>How it works</ins>
1. Boids are always moving at a constant speed, and have the ability to rotate left or right.
2. Each boid has a neural network which determines if the boid should rotate left or right.
3. A neural network gains fitness depending on how close the boid is to the target.
4. After some time, half of the boids with the highest fitness survive - the rest are replaced by mutations of the survivors.
5. The target is moved, and the next generation begins.
6. This repeats generation by generation - until flocking behaviour towards the target emerges.

[Demo](media/flockingdemo.gif)
