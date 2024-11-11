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

    // Op�rateur d'union : R�cup�re les voxels appartenant � au moins une des deux sph�res
    public static List<Bounds> Union(SphereManager sphere1, SphereManager sphere2)
    {
        HashSet<Bounds> unionVoxels = new HashSet<Bounds>();

        // Ajoute les voxels en contact de chaque sph�re dans l'ensemble (HashSet pour �viter les doublons)
        unionVoxels.UnionWith(sphere1.GetVoxelsInContact());
        unionVoxels.UnionWith(sphere2.GetVoxelsInContact());

        // Convertit l'ensemble en liste et retourne le r�sultat
        return new List<Bounds>(unionVoxels);
    }

    // Op�rateur d'intersection : R�cup�re les voxels appartenant aux deux sph�res
    public static List<Bounds> Intersection(SphereManager sphere1, SphereManager sphere2)
    {
        List<Bounds> intersectionVoxels = new List<Bounds>();

        // R�cup�re les voxels en contact pour chaque sph�re
        List<Bounds> voxelsSphere1 = sphere1.GetVoxelsInContact();
        List<Bounds> voxelsSphere2 = sphere2.GetVoxelsInContact();

        // Pour chaque voxel de la premi�re sph�re, v�rifie s'il est aussi pr�sent dans la deuxi�me
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
