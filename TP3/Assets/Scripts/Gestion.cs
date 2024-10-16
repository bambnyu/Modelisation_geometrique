using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gestion : MonoBehaviour
{
    private Mesh_Sphere sphere;

    private Octree octree;

    private Vector3 sphereCenter;
    private float sphereRadius;

    // Start is called before the first frame update
    void Start()
    {
        sphere = new Mesh_Sphere();
        // Initialiser l'octree
        octree = new Octree(Vector3.zero, 10.0f, 20); // Vous pouvez ajuster la taille et la profondeur

        sphereCenter = Vector3.zero;  // Assuming the sphere is centered at origin
        sphereRadius = sphere.rayon;


        // Voxeliser la sphère
        octree.VoxelizeSphere(sphere);

        DisplayVoxelsInContactWithSphere(sphereCenter, sphereRadius);


        //// Récupérer et afficher les voxels sous forme de cubes
        //List<Bounds> voxels = octree.GetVoxels();
        //foreach (var voxel in voxels)
        //{
        //    CreateCube(voxel);
        //}
    }

    void DisplayVoxelsInContactWithSphere(Vector3 sphereCenter, float sphereRadius)
    {
        List<Bounds> voxels = octree.GetVoxels();
        foreach (var voxel in voxels)
        {
            // Get the center of the voxel
            Vector3 voxelCenter = voxel.center;

            // Calculate the distance from the voxel to the center of the sphere
            float distanceToSphere = Vector3.Distance(voxelCenter, sphereCenter);

            // Check if the voxel is within the sphere's radius
            if (distanceToSphere <= sphereRadius)
            {
                // Create and display the cube if in contact with the sphere
                CreateCube(voxel);
            }
        }
    }

    // Méthode pour créer un cube basé sur les bounds d'un voxel
    void CreateCube(Bounds bounds)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Positionner le cube au centre des bounds du voxel
        cube.transform.position = bounds.center;

        // Ajuster la taille du cube selon les dimensions du voxel
        cube.transform.localScale = bounds.size;

        // Optionnel: Appliquer une couleur ou un matériau au cube
        cube.GetComponent<Renderer>().material.color = Color.green; // Exemple de couleur
    }
}