using System.Collections.Generic;
using UnityEngine;

public class OctreeExo2
{
    // Classe interne pour représenter un noeud dans l'octree
    private class Node
    {
        public Bounds bounds; // Limites du noeud dans l'espace
        public bool isLeaf = true; // Indique si ce noeud est une feuille (sans enfants)
        public float potential = 0.0f; // Potentiel actuel au niveau du noeud
        public Node[] children = null; // Tableau pour stocker les enfants si le noeud est subdivisé
        public bool isVisible = false; // Indique si le voxel correspondant est visible
        public GameObject voxelInstance = null; // Instance du voxel, visible ou non

        // Constructeur pour initialiser les limites du noeud
        public Node(Bounds bounds)
        {
            this.bounds = bounds;
        }

        // Méthode pour subdiviser un noeud en 8 enfants
        public void Subdivide()
        {
            children = new Node[8];
            Vector3 size = bounds.size / 2.0f; // Taille de chaque enfant
            Vector3 center = bounds.center;

            // Calcul des positions pour les 8 enfants
            for (int i = 0; i < 8; i++)
            {
                float xOffset = (i & 1) == 0 ? -size.x / 2 : size.x / 2;
                float yOffset = (i & 2) == 0 ? -size.y / 2 : size.y / 2;
                float zOffset = (i & 4) == 0 ? -size.z / 2 : size.z / 2;

                Vector3 offset = new Vector3(xOffset, yOffset, zOffset);
                children[i] = new Node(new Bounds(center + offset, size));
            }
            isLeaf = false; // Ce noeud n'est plus une feuille
        }
    }

    private Node root; // Racine de l'octree
    private float visibilityThreshold; // Seuil au-dessus duquel un voxel devient visible
    private int maxDepth; // Profondeur maximale de l'octree

    // Constructeur pour initialiser l'octree avec une position, taille et seuil de visibilité
    public OctreeExo2(Vector3 position, float size, float visibilityThreshold, int maxDepth)
    {
        root = new Node(new Bounds(position, new Vector3(size, size, size)));
        this.visibilityThreshold = visibilityThreshold;
        this.maxDepth = maxDepth;
    }

    // Méthode pour ajouter du potentiel dans l'octree à une position donnée
    public void AddPotential(Vector3 sphereCenter, float sphereRadius, float amount)
    {
        AddPotentialRecursive(root, sphereCenter, sphereRadius, amount, 0);
    }

    // Méthode pour retirer du potentiel dans l'octree
    public void RemovePotential(Vector3 sphereCenter, float sphereRadius)
    {
        RemovePotentialRecursive(root, sphereCenter, sphereRadius, 0);
    }

    // Méthode récursive pour ajouter du potentiel aux nœuds de l'octree
    private void AddPotentialRecursive(Node node, Vector3 sphereCenter, float sphereRadius, float amount, int depth)
    {
        if (!node.bounds.Intersects(new Bounds(sphereCenter, Vector3.one * sphereRadius * 2)))
            return;

        if (depth >= maxDepth || node.bounds.size.x <= 1.0f)
        {
            float distanceToCenter = Vector3.Distance(node.bounds.center, sphereCenter);
            if (distanceToCenter <= sphereRadius)
            {
                node.potential += amount;
                if (node.potential >= visibilityThreshold && !node.isVisible)
                {
                    node.voxelInstance = CreateVisibleVoxel(node.bounds);
                    node.isVisible = true;
                }
            }
            return;
        }

        if (node.isLeaf)
        {
            node.Subdivide();
        }

        foreach (var child in node.children)
        {
            AddPotentialRecursive(child, sphereCenter, sphereRadius, amount, depth + 1);
        }
    }

    // Méthode récursive pour retirer le potentiel des noeuds
    private void RemovePotentialRecursive(Node node, Vector3 sphereCenter, float sphereRadius, int depth)
    {
        if (!node.bounds.Intersects(new Bounds(sphereCenter, Vector3.one * sphereRadius * 2)))
            return;

        if (depth >= maxDepth || node.bounds.size.x <= 1.0f)
        {
            float distanceToCenter = Vector3.Distance(node.bounds.center, sphereCenter);
            if (distanceToCenter <= sphereRadius && node.isVisible)
            {
                GameObject.Destroy(node.voxelInstance);
                node.isVisible = false;
                node.potential = 0; // Réinitialiser le potentiel après suppression
            }
            return;
        }

        if (!node.isLeaf)
        {
            foreach (var child in node.children)
            {
                RemovePotentialRecursive(child, sphereCenter, sphereRadius, depth + 1);
            }
        }
    }

    // Création d'un voxel visible en tant que GameObject à une position donnée
    private GameObject CreateVisibleVoxel(Bounds bounds)
    {
        GameObject voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        voxel.transform.position = bounds.center;
        voxel.transform.localScale = bounds.size;
        voxel.GetComponent<Renderer>().material.color = Color.red;
        return voxel;
    }
}
