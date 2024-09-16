using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_TruncatedCone : MonoBehaviour
{
    public float rayonBas = 1.0f;  // Rayon de la base
    public float rayonHaut = 0.5f; // Rayon au sommet (tronqué)
    public float hauteur = 2.0f;   // Hauteur totale du cône
    public int nombre_Meridiens = 20; // Nombre de méridiens (lignes verticales)

    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    private GameObject meshObject;
    private Mesh mesh;

    void Start()
    {
        GenerateConeData();

        mesh = new Mesh();
        mesh.name = "Cône Tronqué";

        meshObject = new GameObject("Mesh Object Cone", typeof(MeshRenderer), typeof(MeshFilter));

        meshObject.GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices; // Assigner les sommets
        mesh.uv = uv; // Assigner les coordonnées UV
        mesh.triangles = triangles; // Assigner les triangles
        mesh.RecalculateNormals(); // Recalculer les normales pour l'éclairage
    }

    private void GenerateConeData()
    {
        int verticesCount = (nombre_Meridiens + 1) * 2 + (nombre_Meridiens * 2); // Sommets pour le corps + bords des couvercles
        vertices = new Vector3[verticesCount];
        uv = new Vector2[verticesCount];
        triangles = new int[nombre_Meridiens * 12]; // Triangles pour le corps + couvercles

        float angleStep = 2 * Mathf.PI / nombre_Meridiens;

        int vertIndex = 0;
        int triIndex = 0;


        // on va faire ca en 2 parties la partie du cone donc le contour puis on fera les couvercles pour le fermer des deux cotés

        // Générer les sommets pour le corps du cone (contour)
        for (int i = 0; i <= nombre_Meridiens; i++)
        {
            float angle = i * angleStep;
            float x;
            float z;

            // Sommets bas du cone (contour)
            x = Mathf.Cos(angle) * rayonBas;
            z = Mathf.Sin(angle) * rayonBas;
            vertices[vertIndex] = new Vector3(x, 0, z);
            uv[vertIndex] = new Vector2(i / (float)nombre_Meridiens, 0);
            vertIndex++;

            // Sommets haut du cone (contour)
            x = Mathf.Cos(angle) * rayonHaut;
            z = Mathf.Sin(angle) * rayonHaut;
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
