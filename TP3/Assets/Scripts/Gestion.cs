using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gestion : MonoBehaviour
{
    private Mesh_Sphere sphere;

    private Octree octree;
    // Start is called before the first frame update
    void Start()
    {
        sphere = new Mesh_Sphere();
        // Initialiser l'octree
        octree = new Octree(Vector3.zero, 10.0f, 20); // Vous pouvez ajuster la taille et la profondeur

        // Voxeliser la sphère
        octree.VoxelizeSphere(sphere);

        // Récupérer et afficher les voxels sous forme de cubes
        List<Bounds> voxels = octree.GetVoxels();
        foreach (var voxel in voxels)
        {
            CreateCube(voxel);
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