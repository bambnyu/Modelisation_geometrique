using System.Collections.Generic;
using UnityEngine;

public class Gestion : MonoBehaviour
{
    public int numSpheres = 5; // Number of spheres to generate, configurable in Inspector
    public float octreeSize = 10.0f; // Taille de l'octree -> la resolution de la grille taille interessante pour les tests entre 5 et 10
    public int maxDepth = 20; // Max depth for octree resolution
    public float sphereRadius = 5.0f; // Radius for each sphere

    private List<SphereManager> spheres = new List<SphereManager>();

    void Start()
    {
        // Initialize n spheres with random positions
        for (int i = 0; i < numSpheres; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-octreeSize / 2, octreeSize / 2),
                Random.Range(-octreeSize / 2, octreeSize / 2),
                Random.Range(-octreeSize / 2, octreeSize / 2)
            );

            SphereManager sphere = new SphereManager(randomPosition, sphereRadius, octreeSize, maxDepth);
            spheres.Add(sphere);
        }

        // Display voxels in contact for each sphere
        foreach (var sphere in spheres)
        {
            DisplayVoxelsInContactWithSphere(sphere, Color.green);
        }

        // Display union and intersection if there are at least two spheres on the first two spheres in the list 
        if (spheres.Count >= 2)
        {
            // Display voxels in union (blue)
            //DisplayVoxels(SphereManager.Union(spheres[0], spheres[1]), Color.blue);

            // Display voxels in intersection (red)
            //DisplayVoxels(SphereManager.Intersection(spheres[0], spheres[1]), Color.red);
        }
    }

    // Display voxels in contact with each sphere
    void DisplayVoxelsInContactWithSphere(SphereManager sphere, Color color)
    {
        List<Bounds> voxels = sphere.GetVoxelsInContact();
        foreach (var voxel in voxels)
        {
            CreateCube(voxel, color);
        }
    }

    // Display a list of voxels with a specific color
    void DisplayVoxels(List<Bounds> voxels, Color color)
    {
        foreach (var voxel in voxels)
        {
            CreateCube(voxel, color);
        }
    }

    // Method to create a cube based on the bounds of a voxel
    void CreateCube(Bounds bounds, Color color)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = bounds.center;
        cube.transform.localScale = bounds.size;
        cube.GetComponent<Renderer>().material.color = color;
    }
}
