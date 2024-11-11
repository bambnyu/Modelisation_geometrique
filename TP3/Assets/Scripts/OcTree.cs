using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    private Node root; // Racine de l'octree
    private int maxDepth; // Profondeur maximale de l'octree

    // Classe interne pour définir les nœuds de l'octree
    private class Node
    {
        public Bounds bounds; // Boîte englobante du nœud
        public bool isLeaf; // Indique si le nœud est une feuille
        public Node[] children; // Les sous-nœuds du nœud actuel

        // Constructeur pour initialiser un nœud avec une boîte englobante
        public Node(Bounds bounds)
        {
            this.bounds = bounds;
            this.isLeaf = true; // Un nœud est initialement une feuille
            this.children = null;
        }

        // Méthode pour subdiviser un nœud en 8 sous-nœuds
        public void Subdivide()
        {
            children = new Node[8]; // Créer un tableau pour les 8 enfants
            Vector3 size = bounds.size / 2.0f; // Taille de chaque sous-nœud
            Vector3 center = bounds.center; // Centre du nœud

            for (int i = 0; i < 8; i++)
            {
                float xOffset, yOffset, zOffset;

                // Déterminer les décalages sur x, y et z pour chaque sous-nœud
                xOffset = (i & 1) == 0 ? -size.x / 2 : size.x / 2;
                yOffset = (i & 2) == 0 ? -size.y / 2 : size.y / 2;
                zOffset = (i & 4) == 0 ? -size.z / 2 : size.z / 2;

                // Calculer la position de chaque enfant en fonction du décalage
                Vector3 offset = new Vector3(xOffset, yOffset, zOffset);

                // Créer un nouveau nœud enfant avec la position et la taille correspondantes
                children[i] = new Node(new Bounds(center + offset, size));
            }
            isLeaf = false; // Le nœud n'est plus une feuille car il a des enfants
        }
    }

    // Constructeur de l'octree avec une position, une taille et une profondeur maximale
    public Octree(Vector3 position, float size, int maxDepth)
    {
        root = new Node(new Bounds(position, new Vector3(size, size, size)));
        this.maxDepth = maxDepth;
    }

    // Vérifie si une boîte englobante (bounds) intersecte avec la sphère
    private bool IsIntersectingSphere(Bounds bounds, Vector3 sphereCenter, float sphereRadius)
    {
        float distance = Vector3.Distance(bounds.center, sphereCenter);
        return distance <= sphereRadius + bounds.extents.magnitude;
    }

    // Récupère la liste de tous les voxels (les nœuds feuilles) de l'octree
    public List<Bounds> GetVoxels()
    {
        List<Bounds> voxels = new List<Bounds>();
        CollectVoxels(root, voxels); // Collecte les voxels en partant de la racine
        return voxels;
    }

    // Méthode récursive pour collecter les voxels en parcourant l'octree
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
                CollectVoxels(node.children[i], voxels); // Parcourir les sous-nœuds
            }
        }
    }

    // Méthode pour voxeliser un nœud en fonction de la position et du rayon de la sphère
    private void VoxelizeNode(Node node, Vector3 sphereCenter, float sphereRadius, int depth)
    {
        // Condition d'arrêt si la profondeur max est atteinte ou si le voxel est trop petit
        if (depth >= maxDepth || node.bounds.size.x <= 1.0f)
        {
            return;
        }

        // Vérifier si le nœud intersecte la sphère
        if (IsIntersectingSphere(node.bounds, sphereCenter, sphereRadius))
        {
            if (depth < maxDepth)
            {
                node.Subdivide(); // Subdiviser le nœud
                for (int i = 0; i < 8; i++)
                {
                    VoxelizeNode(node.children[i], sphereCenter, sphereRadius, depth + 1); // Voxeliser les sous-nœuds
                }
            }
        }
    }

    // Méthode pour lancer la voxelisation d'une sphère dans l'octree
    public void VoxelizeSphere(Vector3 sphereCenter, float sphereRadius)
    {
        VoxelizeNode(root, sphereCenter, sphereRadius, 0);
    }
}
