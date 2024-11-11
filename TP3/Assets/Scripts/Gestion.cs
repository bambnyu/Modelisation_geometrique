using System.Collections.Generic;
using UnityEngine;

public class Gestion : MonoBehaviour
{
    private GameObject sphere; // Utilisation de la sph�re de unity (plus simple pour la creation, ma methode pour le tp1 etait pas la plus optimale comme base)
    private Octree octree;

    private Vector3 sphereCenter;
    private float sphereRadius;

    
    void Start()
    {
        // Cr�er une sph�re Unity et d�finir ses propri�t�s
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = Vector3.zero; // Placer la sph�re au centre de la sc�ne
        sphereRadius = 5.0f; // D�finir le rayon de la sph�re  (taille a bouger selon les questions)
        sphere.transform.localScale = Vector3.one * sphereRadius * 2; // Ajuster l'�chelle pour correspondre au rayon

        // Initialiser l'octree
        octree = new Octree(Vector3.zero, 10.0f, 20); // D�finir la taille et la profondeur de l'octree

        // D�finir le centre de la sph�re pour l'affichage des voxels
        sphereCenter = sphere.transform.position;

        // Voxeliser la sph�re dans l'octree
        octree.VoxelizeSphere(sphereCenter, sphereRadius);

        // Afficher les voxels en contact avec la sph�re
        DisplayVoxelsInContactWithSphere(sphereCenter, sphereRadius);
    }

    // M�thode pour afficher les voxels en contact avec la sph�re
    void DisplayVoxelsInContactWithSphere(Vector3 sphereCenter, float sphereRadius)
    {
        List<Bounds> voxels = octree.GetVoxels(); // R�cup�rer tous les voxels de l'octree
        foreach (var voxel in voxels)
        {
            Vector3 voxelCenter = voxel.center;
            float distanceToSphere = Vector3.Distance(voxelCenter, sphereCenter);

            // V�rifier si le voxel est � l'int�rieur du rayon de la sph�re
            if (distanceToSphere <= sphereRadius)
            {
                CreateCube(voxel); // Cr�er un cube pour chaque voxel en contact avec la sph�re
            }
        }
    }

    // M�thode pour cr�er un cube bas� sur les limites (Bounds) d'un voxel
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
