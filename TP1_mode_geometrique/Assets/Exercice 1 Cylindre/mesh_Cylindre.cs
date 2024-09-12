using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_Cylindre : MonoBehaviour
{
    public float rayon = 1.0f; // Le rayon du cylindre
    public float hauteur = 2.0f; // La hauteur du cylindre
    public int nombre_Meridiens = 20; // Le nombre de méridiens (divisions autour du cylindre) je sais pas combien en mettre de base

    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    private GameObject meshObject;
    private Mesh mesh;

    void Start()
    {
        GenerateCylinderData();

        mesh = new Mesh();
        mesh.name = "Cylindre";

        meshObject = new GameObject("Mesh Object Cylindre", typeof(MeshRenderer), typeof(MeshFilter));

        meshObject.GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices; // Assigner les sommets
        mesh.uv = uv; // Assigner les coordonnées UV
        mesh.triangles = triangles; // Assigner les triangles
        mesh.RecalculateNormals(); // Recalculer les normales pour l'éclairage
    }

    private void GenerateCylinderData()
    {
        int verticesCount = (nombre_Meridiens + 1) * 2 + (nombre_Meridiens * 2); // Sommets pour le corps + bords des couvercles
        vertices = new Vector3[verticesCount];
        uv = new Vector2[verticesCount];
        triangles = new int[nombre_Meridiens * 12]; // Triangles pour le corps + couvercles

        float angleStep = 2 * Mathf.PI / nombre_Meridiens;

        int vertIndex = 0;
        int triIndex = 0;

        // on va faire ca en 2 parties la partie du cylindre donc le contour puis on fera les couvercles pour le fermer des deux cotés

        // Générer les sommets pour le corps du cylindre (contour)
        for (int i = 0; i <= nombre_Meridiens; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(angle) * rayon;
            float z = Mathf.Sin(angle) * rayon;

            // Sommets bas du cylindre (contour)
            vertices[vertIndex] = new Vector3(x, 0, z);
            uv[vertIndex] = new Vector2(i / (float)nombre_Meridiens, 0);
            vertIndex++;

            // Sommets haut du cylindre (contour)
            vertices[vertIndex] = new Vector3(x, hauteur, z);
            uv[vertIndex] = new Vector2(i / (float)nombre_Meridiens, 1);
            vertIndex++;

            if (i < nombre_Meridiens)
            {
                // Triangles du corps (contour uniquement)
                int nextIndex = (i + 1) * 2;

                triangles[triIndex++] = i * 2;
                triangles[triIndex++] = i * 2 + 1;
                triangles[triIndex++] = nextIndex + 1;

                triangles[triIndex++] = i * 2;
                triangles[triIndex++] = nextIndex + 1;
                triangles[triIndex++] = nextIndex;
            }
        }

        // Ajouter les couvercles (supérieur et inférieur) 
        int bottomStartIndex = 0;
        int topStartIndex = 1;

        for (int i = 0; i < nombre_Meridiens; i++)
        {
            int nextIndex = (i + 1) % nombre_Meridiens;

            // Couvercle inférieur (en bas) toujours attention au sens le faire a l'inverse de l'inverse de ce qu'on pense car il doit etre visible par en bas
            triangles[triIndex++] = bottomStartIndex + (i * 2);
            triangles[triIndex++] = bottomStartIndex + (nextIndex * 2);
            triangles[triIndex++] = bottomStartIndex;

            // Couvercle supérieur (en haut) lui a l'inverse car on veut le voir d'en haut
            triangles[triIndex++] = topStartIndex;
            triangles[triIndex++] = topStartIndex + (nextIndex * 2);
            triangles[triIndex++] = topStartIndex + (i * 2);
        }
    }

    void Update()
    {
    }
}
