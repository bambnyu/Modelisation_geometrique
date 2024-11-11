using System.Collections.Generic;
using UnityEngine;

public class Gestion : MonoBehaviour
{
    private GameObject sphere; // Utilisation de la sphère de unity (plus simple pour la creation, ma methode pour le tp1 etait pas la plus optimale comme base)
    private Octree octree;

    private Vector3 sphereCenter;
    private float sphereRadius;

    
    void Start()
    {
        // Créer une sphère Unity et définir ses propriétés
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = Vector3.zero; // Placer la sphère au centre de la scène
        sphereRadius = 5.0f; // Définir le rayon de la sphère  (taille a bouger selon les questions)
        sphere.transform.localScale = Vector3.one * sphereRadius * 2; // Ajuster l'échelle pour correspondre au rayon

        // Initialiser l'octree
        octree = new Octree(Vector3.zero, 10.0f, 20); // Définir la taille et la profondeur de l'octree

        // Définir le centre de la sphère pour l'affichage des voxels
        sphereCenter = sphere.transform.position;

        // Voxeliser la sphère dans l'octree
        octree.VoxelizeSphere(sphereCenter, sphereRadius);

        // Afficher les voxels en contact avec la sphère
        DisplayVoxelsInContactWithSphere(sphereCenter, sphereRadius);
    }

    // Méthode pour afficher les voxels en contact avec la sphère
    void DisplayVoxelsInContactWithSphere(Vector3 sphereCenter, float sphereRadius)
    {
        List<Bounds> voxels = octree.GetVoxels(); // Récupérer tous les voxels de l'octree
        foreach (var voxel in voxels)
        {
            Vector3 voxelCenter = voxel.center;
            float distanceToSphere = Vector3.Distance(voxelCenter, sphereCenter);

            // Vérifier si le voxel est à l'intérieur du rayon de la sphère
            if (distanceToSphere <= sphereRadius)
            {
                CreateCube(voxel); // Créer un cube pour chaque voxel en contact avec la sphère
            }
        }
    }

    // Méthode pour créer un cube basé sur les limites (Bounds) d'un voxel
    void CreateCube(Bounds bounds)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Positionner et dimensionner le cube pour qu'il corresponde aux limites du voxel
        cube.transform.position = bounds.center;
        cube.transform.localScale = bounds.size;

        // Mettre une couleur  verte pour le cube pour que ce soit plus visuel
        cube.GetComponent<Renderer>().material.color = Color.green;
    }
}
