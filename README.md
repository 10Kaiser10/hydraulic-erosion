# Hydraulic Erosion on Procedurally Generated Terrain

Procedural Terrain Generation in Unity. Simulating erosion due to water to make the terrain look more natural. Using compute shader to run the simulation of GPU for massive improvements in simulation speed (Simulating a million water drops in 10 seconds).


- Generating Height Map image using perlin noise. Using a compute shader to run the code on GPU.
- Applying hydraulic erosion on the height map. Using compute shader to simulate 1,000,000 drops in parallel on GPU. 
- Using the eroded heightmap to generate the mesh.
- Generating textures based on the slope of the terrain and applying it on the mesh.

### Final Output:
![](https://github.com/10Kaiser10/hydraulic-erosion/blob/main/images/Annotation%202021-01-29%20212812.png)
![](https://github.com/10Kaiser10/hydraulic-erosion/blob/main/images/Annotation%202021-01-29%20213256.png)
![](https://github.com/10Kaiser10/hydraulic-erosion/blob/main/images/Annotation%202021-01-29%20213730.png)
