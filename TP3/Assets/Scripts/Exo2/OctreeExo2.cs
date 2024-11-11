using System.Collections.Generic;
using UnityEngine;

public class OctreeExo2
{
    private class Node
    {
        public Bounds bounds;
        public bool isLeaf = true;
        public float potential = 0.0f;
        public Node[] children = null;

        public Node(Bounds bounds)
        {
            this.bounds = bounds;
        }

        // Subdivide the node into 8 children
        public void Subdivide()
        {
            children = new Node[8];
            Vector3 size = bounds.size / 2.0f;
            Vector3 center = bounds.center;

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
    private float maxDepth;

    public OctreeExo2(Vector3 position, float size, float visibilityThreshold, int maxDepth)
    {
        root = new Node(new Bounds(position, new Vector3(size, size, size)));
        this.visibilityThreshold = visibilityThreshold;
        this.maxDepth = maxDepth;
    }

    // Add potential to the node at the specified position
    public void AddPotential(Vector3 position, float amount)
    {
        AddPotentialRecursive(root, position, amount, 0);
    }

    private void AddPotentialRecursive(Node node, Vector3 position, float amount, int depth)
    {
        if (depth >= maxDepth || node.bounds.size.x <= 1.0f)
        {
            node.potential += amount;
            if (node.potential >= visibilityThreshold)
            {
                CreateVisibleVoxel(node.bounds);
            }
            return;
        }

        if (!node.bounds.Contains(position))
            return;

        if (node.isLeaf)
        {
            node.Subdivide();
        }

        foreach (var child in node.children)
        {
            AddPotentialRecursive(child, position, amount, depth + 1);
        }
    }

    // Create a visible voxel in the scene
    private void CreateVisibleVoxel(Bounds bounds)
    {
        GameObject voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        voxel.transform.position = bounds.center;
        voxel.transform.localScale = bounds.size;
    }
}
