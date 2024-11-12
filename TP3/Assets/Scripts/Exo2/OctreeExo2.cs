using System.Collections.Generic;
using UnityEngine;

public class OctreeExo2
{
    // Classe repr�sentant un n�ud dans l'octree
    private class Node
    {
        public Bounds bounds; // Bo�te englobante du n�ud
        public bool isLeaf = true; // Indique si le n�ud est une feuille
        public float potential = 0.0f; // Potentiel actuel du n�ud
        public Node[] children = null; // Sous-n�uds
        public bool isVisible = false; // Indique si le voxel est d�j� visible

        public Node(Bounds bounds)
        {
            this.bounds = bounds;
        }

        // Subdivise le n�ud en 8 enfants
        public void Subdivide()
        {
            children = new Node[8];
            Vector3 size = bounds.size / 2.0f;
            Vector3 center = bounds.center;

            // Cr�e chaque sous-n�ud avec la position correcte
            for (int i = 0; i < 8; i++)
            {
                float xOffset = (i & 1) == 0 ? -size.x / 2 : size.x / 2;
                float yOffset = (i & 2) == 0 ? -size.y / 2 : size.y / 2;
                float zOffset = (i & 4) == 0 ? -size.z / 2 : size.z / 2;

                Vector3 offset = new Vector3(xOffset, yOffset, zOffset);
                children[i] = new Node(new Bounds(center + offset, size));
            }
            isLeaf = false;
        }
    }

    private Node root;
    private float visibilityThreshold;
    private int maxDepth;

    // Constructeur pour initialiser l'octree
    public OctreeExo2(Vector3 position, float size, float visibilityThreshold, int maxDepth)
    {
        root = new Node(new Bounds(position, new Vector3(size, size, size)));
        this.visibilityThreshold = visibilityThreshold;
        this.maxDepth = maxDepth;
    }

    // M�thode pour ajouter du potentiel autour d'une sph�re
    public void AddPotential(Vector3 sphereCenter, float sphereRadius, float amount)
    {
        AddPotentialRecursive(root, sphereCenter, sphereRadius, amount, 0);
    }

    private void AddPotentialRecursive(Node node, Vector3 sphereCenter, float sphereRadius, float amount, int depth)
    {
        if (!node.bounds.Intersects(new Bounds(sphereCenter, Vector3.one * sphereRadius * 2)))
            return;

        // Si profondeur max atteinte ou taille minimale, mise � jour du potentiel
        if (depth >= maxDepth || node.bounds.size.x <= 1.0f)
        {
            float distanceToCenter = Vector3.Distance(node.bounds.center, sphereCenter);
            if (distanceToCenter <= sphereRadius)
            {
                node.potential += amount;
                if (node.potential >= visibilityThreshold && !node.isVisible)
                {
                    CreateVisibleVoxel(node.bounds); // Cr�e un voxel visible
                    node.isVisible = true;
                }
            }
            return;
        }

        // Subdivise le n�ud s'il est une feuille
        if (node.isLeaf)
        {
            node.Subdivide();
        }

        // Appel r�cursif sur chaque enfant
        foreach (var child in node.children)
        {
            AddPotentialRecursive(child, sphereCenter, sphereRadius, amount, depth + 1);
        }
    }

    // Cr�e un voxel visible
    private void CreateVisibleVoxel(Bounds bounds)
    {
        GameObject voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        voxel.transform.position = bounds.center;
        voxel.transform.localScale = bounds.size;
        voxel.GetComponent<Renderer>().material.color = Color.red;
    }
}
