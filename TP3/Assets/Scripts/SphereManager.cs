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

        // Retrieve the voxels in contact for each sphere
        List<Bounds> voxelsSphere1 = sphere1.GetVoxelsInContact();
        List<Bounds> voxelsSphere2 = sphere2.GetVoxelsInContact();

        // Check for intersecting voxels by comparing bounds using Intersects method
        foreach (var voxel1 in voxelsSphere1)
        {
            foreach (var voxel2 in voxelsSphere2)
            {
                if (voxel1.Intersects(voxel2))
                {
                    intersectionVoxels.Add(voxel1);
                    break; // Move to the next voxel1 after finding an intersection
                }
            }
        }

        return intersectionVoxels;
    }

    public static List<Bounds> UnionAll(List<SphereManager> spheres)
    {
        HashSet<Bounds> unionVoxels = new HashSet<Bounds>();

        // Add voxels in contact from each sphere
        foreach (var sphere in spheres)
        {
            unionVoxels.UnionWith(sphere.GetVoxelsInContact());
        }

        return new List<Bounds>(unionVoxels);
    }

    public static List<Bounds> IntersectionAll(List<SphereManager> spheres)
    {
        if (spheres.Count == 0) return new List<Bounds>();

        // Start with the voxels of the first sphere
        HashSet<Bounds> intersectionVoxels = new HashSet<Bounds>(spheres[0].GetVoxelsInContact());

        // Intersect with each subsequent sphere's voxels
        for (int i = 1; i < spheres.Count; i++)
        {
            intersectionVoxels.IntersectWith(spheres[i].GetVoxelsInContact());
        }

        return new List<Bounds>(intersectionVoxels);
    }
}
