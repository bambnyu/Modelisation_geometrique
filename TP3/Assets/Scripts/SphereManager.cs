using System.Collections.Generic;
using UnityEngine;

public class SphereManager
{
    // Propriétés pour le centre et le rayon de la sphère, ainsi que l'Octree associé
    public Vector3 Center { get; private set; }
    public float Radius { get; private set; }
    public Octree Octree { get; private set; }

    public SphereManager(Vector3 center, float radius, float octreeSize, int maxDepth)
    {
        // Initialise les propriétés du centre et du rayon de la sphère
        Center = center;
        Radius = radius;

        // Crée un Octree avec les paramètres spécifiés et l'associe à la sphère
        Octree = new Octree(center, octreeSize, maxDepth);
        // Voxelise la sphère dans l'octree en utilisant le centre et le rayon
        Octree.VoxelizeSphere(center, radius);
    }

    // Méthode pour récupérer les voxels de l'octree en contact avec la surface de la sphère
    public List<Bounds> GetVoxelsInContact()
    {
        // Récupère tous les voxels dans l'octree
        List<Bounds> voxels = Octree.GetVoxels();
        List<Bounds> inContact = new List<Bounds>();

        // Parcourt chaque voxel pour vérifier s'il est en contact avec la sphère
        foreach (var voxel in voxels)
        {
            // Si la distance entre le centre du voxel et le centre de la sphère est inférieure ou égale au rayon, il est en contact
            if (Vector3.Distance(voxel.center, Center) <= Radius)
            {
                inContact.Add(voxel); // Ajoute le voxel à la liste des voxels en contact
            }
        }
        return inContact; // Renvoie la liste des voxels en contact
    }
}
