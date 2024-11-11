using System.Collections.Generic;
using UnityEngine;

public class SphereManager
{
    // Propri�t�s pour le centre et le rayon de la sph�re, ainsi que l'Octree associ�
    public Vector3 Center { get; private set; }
    public float Radius { get; private set; }
    public Octree Octree { get; private set; }

    public SphereManager(Vector3 center, float radius, float octreeSize, int maxDepth)
    {
        // Initialise les propri�t�s du centre et du rayon de la sph�re
        Center = center;
        Radius = radius;

        // Cr�e un Octree avec les param�tres sp�cifi�s et l'associe � la sph�re
        Octree = new Octree(center, octreeSize, maxDepth);
        // Voxelise la sph�re dans l'octree en utilisant le centre et le rayon
        Octree.VoxelizeSphere(center, radius);
    }

    // M�thode pour r�cup�rer les voxels de l'octree en contact avec la surface de la sph�re
    public List<Bounds> GetVoxelsInContact()
    {
        // R�cup�re tous les voxels dans l'octree
        List<Bounds> voxels = Octree.GetVoxels();
        List<Bounds> inContact = new List<Bounds>();

        // Parcourt chaque voxel pour v�rifier s'il est en contact avec la sph�re
        foreach (var voxel in voxels)
        {
            // Si la distance entre le centre du voxel et le centre de la sph�re est inf�rieure ou �gale au rayon, il est en contact
            if (Vector3.Distance(voxel.center, Center) <= Radius)
            {
                inContact.Add(voxel); // Ajoute le voxel � la liste des voxels en contact
            }
        }
        return inContact; // Renvoie la liste des voxels en contact
    }
}
