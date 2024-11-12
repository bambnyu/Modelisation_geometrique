using System.Collections.Generic;
using UnityEngine;

public class SphereManager
{
    public Vector3 Center { get; private set; } // Centre de la sphère
    public float Radius { get; private set; } // Rayon de la sphère
    public Octree Octree { get; private set; } // Octree associé pour gérer la voxelisation

    // Constructeur pour initialiser la sphère avec son centre, rayon, taille d'octree, et profondeur max
    public SphereManager(Vector3 center, float radius, float octreeSize, int maxDepth)
    {
        Center = center;
        Radius = radius;
        Octree = new Octree(center, octreeSize, maxDepth);
        Octree.VoxelizeSphere(center, radius); // Voxeliser la sphère dans l'octree
    }

    // Récupère les voxels en contact avec la sphère
    public List<Bounds> GetVoxelsInContact()
    {
        List<Bounds> voxels = Octree.GetVoxels(); // Tous les voxels de l'octree
        List<Bounds> inContact = new List<Bounds>();

        // Filtrer les voxels qui sont en contact avec la sphère
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

        // Ajoute les voxels en contact de chaque sphère dans l'ensemble (HashSet ---> éviter les doublons)
        unionVoxels.UnionWith(sphere1.GetVoxelsInContact());
        unionVoxels.UnionWith(sphere2.GetVoxelsInContact());

        // Convertit l'ensemble en liste et retourne le résultat
        return new List<Bounds>(unionVoxels);
    }

    // Opérateur d'intersection : Récupère les voxels appartenant aux deux sphères
    public static List<Bounds> Intersection(SphereManager sphere1, SphereManager sphere2)
    {
        List<Bounds> intersectionVoxels = new List<Bounds>();

        // Récupérer les voxels en contact pour chaque sphère
        List<Bounds> voxelsSphere1 = sphere1.GetVoxelsInContact();
        List<Bounds> voxelsSphere2 = sphere2.GetVoxelsInContact();

        // Vérifier les voxels d'intersection en comparant les limites
        foreach (var voxel1 in voxelsSphere1)
        {
            foreach (var voxel2 in voxelsSphere2)
            {
                if (voxel1.Intersects(voxel2))
                {
                    intersectionVoxels.Add(voxel1);
                    break; // Passer au voxel suivant après avoir trouvé une intersection
                }
            }
        }

        return intersectionVoxels;
    }

    // Récupère l'union des voxels en contact pour une liste de sphères
    public static List<Bounds> UnionAll(List<SphereManager> spheres)
    {
        HashSet<Bounds> unionVoxels = new HashSet<Bounds>();

        // Ajouter les voxels en contact de chaque sphère
        foreach (var sphere in spheres)
        {
            unionVoxels.UnionWith(sphere.GetVoxelsInContact());
        }

        return new List<Bounds>(unionVoxels);
    }

    // Récupère l'intersection des voxels en contact pour une liste de sphères
    public static List<Bounds> IntersectionAll(List<SphereManager> spheres)
    {
        if (spheres.Count == 0) return new List<Bounds>();

        // Commencer avec les voxels de la première sphère
        HashSet<Bounds> intersectionVoxels = new HashSet<Bounds>(spheres[0].GetVoxelsInContact());

        // Intersection avec les voxels de chaque sphère suivante
        for (int i = 1; i < spheres.Count; i++)
        {
            intersectionVoxels.IntersectWith(spheres[i].GetVoxelsInContact());
        }

        return new List<Bounds>(intersectionVoxels);
    }
}
