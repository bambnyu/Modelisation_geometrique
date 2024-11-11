using System.Collections.Generic;
using UnityEngine;

public class Gestion : MonoBehaviour
{
    private List<SphereManager> spheres = new List<SphereManager>();

    public float octreeSize = 10; // Taille de l'octree -> la resolution de la grille taille interessante pour les tests entre 5 et 10

    void Start()
    {
        // Initialisation de deux sphères

        SphereManager sphere1 = new SphereManager(Vector3.zero, 5.0f, octreeSize, 20);
        SphereManager sphere2 = new SphereManager(new Vector3(6.0f, 0.0f, 0.0f), 5.0f, octreeSize, 20);

        // Affiche les voxels en union et en intersection

        spheres.Add(sphere1);
        spheres.Add(sphere2);

        // Affichage des voxels en contact pour chaque sphère individuellement
        foreach (var sphere in spheres)
        {
            DisplayVoxelsInContactWithSphere(sphere, Color.green);
        }

        // Ne  pas les afficher en meme temps ca a pas de sens
        //// Affichage des voxels en union (couleur bleue)
        ////DisplayVoxels(SphereManager.Union(sphere1, sphere2), Color.blue);

        //// Affichage des voxels en intersection (couleur rouge)
        //DisplayVoxels(SphereManager.Intersection(sphere1, sphere2), Color.red);
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

    // Méthode pour créer un cube basé sur les limites (Bounds) d'un voxel
    void CreateCube(Bounds bounds, Color color)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = bounds.center;
        cube.transform.localScale = bounds.size;
        cube.GetComponent<Renderer>().material.color = color;
    }
}
