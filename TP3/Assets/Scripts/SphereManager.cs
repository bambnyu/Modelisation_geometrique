using System.Collections.Generic;
using UnityEngine;

public class SphereManager
{
    public Vector3 Center { get; private set; }
    public float Radius { get; private set; }
    public Octree Octree { get; private set; }

    public SphereManager(Vector3 center, float radius, float octreeSize, int maxDepth)
    {
        Center = center;
        Radius = radius;
        Octree = new Octree(center, octreeSize, maxDepth);
        Octree.VoxelizeSphere(center, radius);
    }

    public List<Bounds> GetVoxelsInContact()
    {
        List<Bounds> voxels = Octree.GetVoxels();
        List<Bounds> inContact = new List<Bounds>();

        foreach (var voxel in voxels)
        {
            if (Vector3.Distance(voxel.center, Center) <= Radius)
            {
                inContact.Add(voxel);
            }
        }
        return inContact;
    }

    // Opérateur d'union : Récupère les voxels appartenant à au moins une des deux sphères
    public static List<Bounds> Union(SphereManager sphere1, SphereManager sphere2)
    {
        HashSet<Bounds> unionVoxels = new HashSet<Bounds>();

        // Ajoute les voxels en contact de chaque sphère dans l'ensemble (HashSet pour éviter les doublons)
        unionVoxels.UnionWith(sphere1.GetVoxelsInContact());
        unionVoxels.UnionWith(sphere2.GetVoxelsInContact());

        // Convertit l'ensemble en liste et retourne le résultat
        return new List<Bounds>(unionVoxels);
    }

    // Opérateur d'intersection : Récupère les voxels appartenant aux deux sphères
    public static List<Bounds> Intersection(SphereManager sphere1, SphereManager sphere2)
    {
        List<Bounds> intersectionVoxels = new List<Bounds>();

        // Récupère les voxels en contact pour chaque sphère
        List<Bounds> voxelsSphere1 = sphere1.GetVoxelsInContact();
        List<Bounds> voxelsSphere2 = sphere2.GetVoxelsInContact();

        // Pour chaque voxel de la première sphère, vérifie s'il est aussi présent dans la deuxième
        foreach (var voxel in voxelsSphere1)
        {
            if (voxelsSphere2.Contains(voxel))
            {
                intersectionVoxels.Add(voxel);
            }
        }

        return intersectionVoxels;
    }
}
