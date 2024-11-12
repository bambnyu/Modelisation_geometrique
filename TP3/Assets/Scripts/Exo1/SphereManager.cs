using System.Collections.Generic;
using UnityEngine;

public class SphereManager
{
    public Vector3 Center { get; private set; } // Centre de la sph�re
    public float Radius { get; private set; } // Rayon de la sph�re
    public Octree Octree { get; private set; } // Octree associ� pour g�rer la voxelisation

    // Constructeur pour initialiser la sph�re avec son centre, rayon, taille d'octree, et profondeur max
    public SphereManager(Vector3 center, float radius, float octreeSize, int maxDepth)
    {
        Center = center;
        Radius = radius;
        Octree = new Octree(center, octreeSize, maxDepth);
        Octree.VoxelizeSphere(center, radius); // Voxeliser la sph�re dans l'octree
    }

    // R�cup�re les voxels en contact avec la sph�re
    public List<Bounds> GetVoxelsInContact()
    {
        List<Bounds> voxels = Octree.GetVoxels(); // Tous les voxels de l'octree
        List<Bounds> inContact = new List<Bounds>();

        // Filtrer les voxels qui sont en contact avec la sph�re
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

        // Ajoute les voxels en contact de chaque sph�re dans l'ensemble (HashSet ---> �viter les doublons)
        unionVoxels.UnionWith(sphere1.GetVoxelsInContact());
        unionVoxels.UnionWith(sphere2.GetVoxelsInContact());

        // Convertit l'ensemble en liste et retourne le r�sultat
        return new List<Bounds>(unionVoxels);
    }

    // Op�rateur d'intersection : R�cup�re les voxels appartenant aux deux sph�res
    public static List<Bounds> Intersection(SphereManager sphere1, SphereManager sphere2)
    {
        List<Bounds> intersectionVoxels = new List<Bounds>();

        // R�cup�rer les voxels en contact pour chaque sph�re
        List<Bounds> voxelsSphere1 = sphere1.GetVoxelsInContact();
        List<Bounds> voxelsSphere2 = sphere2.GetVoxelsInContact();

        // V�rifier les voxels d'intersection en comparant les limites
        foreach (var voxel1 in voxelsSphere1)
        {
            foreach (var voxel2 in voxelsSphere2)
            {
                if (voxel1.Intersects(voxel2))
                {
                    intersectionVoxels.Add(voxel1);
                    break; // Passer au voxel suivant apr�s avoir trouv� une intersection
                }
            }
        }

        return intersectionVoxels;
    }

    // R�cup�re l'union des voxels en contact pour une liste de sph�res
    public static List<Bounds> UnionAll(List<SphereManager> spheres)
    {
        HashSet<Bounds> unionVoxels = new HashSet<Bounds>();

        // Ajouter les voxels en contact de chaque sph�re
        foreach (var sphere in spheres)
        {
            unionVoxels.UnionWith(sphere.GetVoxelsInContact());
        }

        return new List<Bounds>(unionVoxels);
    }

    // R�cup�re l'intersection des voxels en contact pour une liste de sph�res
    public static List<Bounds> IntersectionAll(List<SphereManager> spheres)
    {
        if (spheres.Count == 0) return new List<Bounds>();

        // Commencer avec les voxels de la premi�re sph�re
        HashSet<Bounds> intersectionVoxels = new HashSet<Bounds>(spheres[0].GetVoxelsInContact());

        // Intersection avec les voxels de chaque sph�re suivante
        for (int i = 1; i < spheres.Count; i++)
        {
            intersectionVoxels.IntersectWith(spheres[i].GetVoxelsInContact());
        }

        return new List<Bounds>(intersectionVoxels);
    }
}
