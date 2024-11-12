using System.Collections.Generic;
using UnityEngine;

public class Gestion : MonoBehaviour
{
    public int numSpheres = 5; // Nombre de sphères à générer
    public float octreeSize = 10.0f; // Taille de l'octree a faire varier entre 5 et 10 pour la résolution
    public int maxDepth = 20; // Profondeur maximale pour la résolution de l'octree
    public float sphereRadius = 5.0f; // Rayon de chaque sphère

    private List<SphereManager> spheres = new List<SphereManager>();

    void Start()
    {
        // Initialiser n sphères avec des positions aléatoires
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

        // Afficher les voxels en contact pour chaque sphère
        foreach (var sphere in spheres)
        {
            DisplayVoxelsInContactWithSphere(sphere, Color.green);
        }

        // Afficher l'union et l'intersection pour toutes les sphères
        if (spheres.Count > 1)
        {
            // Union de toutes les sphères (bleu)
            //DisplayVoxels(SphereManager.UnionAll(spheres), Color.blue);

            // Intersection de toutes les sphères (rouge) très peu probable que toutes les sphères se superposent au meme endroit
            //DisplayVoxels(SphereManager.IntersectionAll(spheres), Color.red);
            // Intersection des 2 premières  sphères (rouge)
            //DisplayVoxels(SphereManager.Intersection(spheres[0], spheres[1]), Color.red); // faut que les voxels soient en contact avec les deux spheres ce qui est plus commun
        }
    }

    // Afficher les voxels en contact avec chaque sphère
    void DisplayVoxelsInContactWithSphere(SphereManager sphere, Color color)
    {
        List<Bounds> voxels = sphere.GetVoxelsInContact();
        foreach (var voxel in voxels)
        {
            CreateCube(voxel, color);
        }
    }

    // Afficher une liste de voxels avec une couleur spécifique
    void DisplayVoxels(List<Bounds> voxels, Color color)
    {
        foreach (var voxel in voxels)
        {
            CreateCube(voxel, color);
        }
    }

    // Méthode pour créer un cube basé sur les limites d'un voxel
    void CreateCube(Bounds bounds, Color color)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = bounds.center;
        cube.transform.localScale = bounds.size;
        cube.GetComponent<Renderer>().material.color = color;
    }
}
