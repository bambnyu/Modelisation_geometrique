using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    private Node root; // Racine de l'arbre
    private int maxDepth; // Profondeur maximale de l'arbre
    private class Node
    {
        public Bounds bounds; // Bounding box du nœud
        public bool isLeaf; // Indique si le nœud est une feuille
        public Node[] children; // Sous-nœuds

        public Node(Bounds bounds)
        {
            this.bounds = bounds;
            this.isLeaf = true;
            this.children = null;
        }
        public void Subdivide()
        {
            children = new Node[8];
            Vector3 size = bounds.size / 2.0f; // Taille des sous-nœuds
            Vector3 center = bounds.center; // Centre du nœud


            for (int i = 0; i < 8; i++)
            {
                float xOffset, yOffset, zOffset;
                if ((i & 1) == 0)  // Si le bit le plus à droite de i est 0
                {
                    xOffset = -size.x / 2;
                }
                else  // Si le bit le plus à droite de i est 1
                {
                    xOffset = size.x / 2;
                }
                if ((i & 2) == 0)  // Si le deuxième bit de i est 0
                {
                    yOffset = -size.y / 2;
                }
                else  // Si le deuxième bit de i est 1
                {
                    yOffset = size.y / 2;
                }
                if ((i & 4) == 0)  // Si le troisième bit de i est 0
                {
                    zOffset = -size.z / 2;
                }
                else  // Si le troisième bit de i est 1
                {
                    zOffset = size.z / 2;
                }
                // Calculer la position de chaque enfant en fonction du décalage sur x, y et z
                Vector3 offset = new Vector3(xOffset, yOffset, zOffset);

                // Créer un nouveau nœud enfant avec la position et la taille correspondantes
                children[i] = new Node(new Bounds(center + offset, size));
            }
            isLeaf = false; // Le nœud n'est plus une feuille car on vient de lui faire des gosses
        }


    }

    public Octree(Vector3 position, float size, int maxDepth) // notre constructeur j'aurais du le mettre au dessus
    {
        root = new Node(new Bounds(position, new Vector3(size, size, size)));
        this.maxDepth = maxDepth;
    }

    private bool IsIntersectingSphere(Bounds bounds, Mesh_Sphere sphere)
    {
        // On utilise la distance au centre de la sphère
        float distance = Vector3.Distance(bounds.center, sphere.center);
        return distance <= sphere.rayon + bounds.extents.magnitude;
    }

    public List<Bounds> GetVoxels()
    {
        List<Bounds> voxels = new List<Bounds>();
        CollectVoxels(root, voxels);
        return voxels;
    }
    private void CollectVoxels(Node node, List<Bounds> voxels)
    {
        if (node.isLeaf)
        {
            voxels.Add(node.bounds);
        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                CollectVoxels(node.children[i], voxels);
            }
        }
    }

    private void VoxelizeNode(Node node, Mesh_Sphere sphere, int depth)
    {
        if (depth >= maxDepth || node.bounds.size.x <= 1.0f)
        {
            return;
        }

        if (IsIntersectingSphere(node.bounds, sphere))
        {
            if (depth < maxDepth)
            {
                node.Subdivide();
                for (int i = 0; i < 8; i++)
                {
                    VoxelizeNode(node.children[i], sphere, depth + 1);
                }
            }
        }
    }


    public void VoxelizeSphere(Mesh_Sphere sphere)
    {
        VoxelizeNode(root, sphere, 0);
    }

}
