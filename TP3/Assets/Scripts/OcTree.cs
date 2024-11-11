using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    private Node root; // Racine de l'octree
    private int maxDepth; // Profondeur maximale de l'octree

    // Classe interne pour d�finir les n�uds de l'octree
    private class Node
    {
        public Bounds bounds; // Bo�te englobante du n�ud
        public bool isLeaf; // Indique si le n�ud est une feuille
        public Node[] children; // Les sous-n�uds du n�ud actuel

        // Constructeur pour initialiser un n�ud avec une bo�te englobante
        public Node(Bounds bounds)
        {
            this.bounds = bounds;
            this.isLeaf = true; // Un n�ud est initialement une feuille
            this.children = null;
        }

        // M�thode pour subdiviser un n�ud en 8 sous-n�uds
        public void Subdivide()
        {
            children = new Node[8]; // Cr�er un tableau pour les 8 enfants
            Vector3 size = bounds.size / 2.0f; // Taille de chaque sous-n�ud
            Vector3 center = bounds.center; // Centre du n�ud

            for (int i = 0; i < 8; i++)
            {
                float xOffset, yOffset, zOffset;

                // D�terminer les d�calages sur x, y et z pour chaque sous-n�ud
                xOffset = (i & 1) == 0 ? -size.x / 2 : size.x / 2;
                yOffset = (i & 2) == 0 ? -size.y / 2 : size.y / 2;
                zOffset = (i & 4) == 0 ? -size.z / 2 : size.z / 2;

                // Calculer la position de chaque enfant en fonction du d�calage
                Vector3 offset = new Vector3(xOffset, yOffset, zOffset);

                // Cr�er un nouveau n�ud enfant avec la position et la taille correspondantes
                children[i] = new Node(new Bounds(center + offset, size));
            }
            isLeaf = false; // Le n�ud n'est plus une feuille car il a des enfants
        }
    }

    // Constructeur de l'octree avec une position, une taille et une profondeur maximale
    public Octree(Vector3 position, float size, int maxDepth)
    {
        root = new Node(new Bounds(position, new Vector3(size, size, size)));
        this.maxDepth = maxDepth;
    }

    // V�rifie si une bo�te englobante (bounds) intersecte avec la sph�re
    private bool IsIntersectingSphere(Bounds bounds, Vector3 sphereCenter, float sphereRadius)
    {
        float distance = Vector3.Distance(bounds.center, sphereCenter);
        return distance <= sphereRadius + bounds.extents.magnitude;
    }

    // R�cup�re la liste de tous les voxels (les n�uds feuilles) de l'octree
    public List<Bounds> GetVoxels()
    {
        List<Bounds> voxels = new List<Bounds>();
        CollectVoxels(root, voxels); // Collecte les voxels en partant de la racine
        return voxels;
    }

    // M�thode r�cursive pour collecter les voxels en parcourant l'octree
    private void CollectVoxels(Node node, List<Bounds> voxels)
    {
        if (node.isLeaf)
        {
            voxels.Add(node.bounds); // Ajouter les limites du voxel si c'est une feuille
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                CollectVoxels(node.children[i], voxels); // Parcourir les sous-n�uds
            }
        }
    }

    // M�thode pour voxeliser un n�ud en fonction de la position et du rayon de la sph�re
    private void VoxelizeNode(Node node, Vector3 sphereCenter, float sphereRadius, int depth)
    {
        // Condition d'arr�t si la profondeur max est atteinte ou si le voxel est trop petit
        if (depth >= maxDepth || node.bounds.size.x <= 1.0f)
        {
            return;
        }

        // V�rifier si le n�ud intersecte la sph�re
        if (IsIntersectingSphere(node.bounds, sphereCenter, sphereRadius))
        {
            if (depth < maxDepth)
            {
                node.Subdivide(); // Subdiviser le n�ud
                for (int i = 0; i < 8; i++)
                {
                    VoxelizeNode(node.children[i], sphereCenter, sphereRadius, depth + 1); // Voxeliser les sous-n�uds
                }
            }
        }
    }

    // M�thode pour lancer la voxelisation d'une sph�re dans l'octree
    public void VoxelizeSphere(Vector3 sphereCenter, float sphereRadius)
    {
        VoxelizeNode(root, sphereCenter, sphereRadius, 0);
    }
}
