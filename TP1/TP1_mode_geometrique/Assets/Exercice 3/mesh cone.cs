using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_TruncatedCone : MonoBehaviour
{
    // on va faire un cone tronqu� donc on aura un rayon bas et un rayon haut
    public float rayonBas = 1.0f;  // Rayon de la base
    public float rayonHaut = 0.5f; // Rayon au sommet (tronqu�)

    public float hauteur = 2.0f;   // Hauteur totale du c�ne
    public int nombre_Meridiens = 20; // Nombre de m�ridiens (lignes verticales)

    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    private GameObject meshObject;
    private Mesh mesh;

    void Start()
    {
        // G�n�re les donn�es du c�ne tronqu�
        GenerateConeData();
        // Cr�e le mesh du c�ne 
        mesh = new Mesh();
        mesh.name = " Mesh C�ne Tronqu�";
        // Cr�e un objet avec un MeshFilter et un MeshRenderer et assigne le mesh
        meshObject = new GameObject("Mesh Cone", typeof(MeshRenderer), typeof(MeshFilter));
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices; // Assigner les sommets
        mesh.uv = uv; // Assigner les coordonn�es UV
        mesh.triangles = triangles; // Assigner les triangles
        mesh.RecalculateNormals(); // Recalculer les normales pour l'�clairage
    }

    private void GenerateConeData()
    {
        // Calculer le nombre de sommets et de triangles
        int verticesCount = (nombre_Meridiens + 1) * 2 + (nombre_Meridiens * 2); // Sommets pour le corps + bords des couvercles
        vertices = new Vector3[verticesCount]; // Sommets pour le corps + bords des couvercles
        uv = new Vector2[verticesCount];
        triangles = new int[nombre_Meridiens * 12]; // Triangles pour le corps + couvercles
        // Calculer l'angle entre chaque m�ridien 
        float angleStep = 2 * Mathf.PI / nombre_Meridiens;

        int vertIndex = 0;
        int triIndex = 0;


        // on va faire ca en 2 parties la partie du cone donc le contour puis on fera les couvercles pour le fermer des deux cot�s

        // G�n�rer les sommets pour le corps du cone (contour)
        for (int i = 0; i <= nombre_Meridiens; i++)
        {
            float angle = i * angleStep; // Angle du m�ridien
            float x;
            float z;

            // Sommets bas du cone (contour)
            x = Mathf.Cos(angle) * rayonBas; // Calculer la coordonn�e x
            z = Mathf.Sin(angle) * rayonBas; // Calculer la coordonn�e z
            vertices[vertIndex] = new Vector3(x, 0, z); // Cr�er le sommet
            uv[vertIndex] = new Vector2(i / (float)nombre_Meridiens, 0); // Coordonn�es UV
            vertIndex++;

            // Sommets haut du cone (contour)
            x = Mathf.Cos(angle) * rayonHaut; // Calculer la coordonn�e x
            z = Mathf.Sin(angle) * rayonHaut; // Calculer la coordonn�e z
            vertices[vertIndex] = new Vector3(x, hauteur, z); // Cr�er le sommet
            uv[vertIndex] = new Vector2(i / (float)nombre_Meridiens, 1); // Coordonn�es UV
            vertIndex++;


            if (i < nombre_Meridiens)
            {
                // Triangles du corps (contour uniquement)
                int nextIndex = (i + 1) * 2;
                // Triangle 1
                triangles[triIndex++] = i * 2;
                triangles[triIndex++] = i * 2 + 1;
                triangles[triIndex++] = nextIndex + 1;
                // Triangle 2
                triangles[triIndex++] = i * 2;
                triangles[triIndex++] = nextIndex + 1;
                triangles[triIndex++] = nextIndex;
            }

        }

        // Ajouter les couvercles (sup�rieur et inf�rieur) 
        int bottomStartIndex = 0;
        int topStartIndex = 1;

        for (int i = 0; i < nombre_Meridiens; i++)
        {
            int nextIndex = (i + 1) % nombre_Meridiens;

            // Couvercle inf�rieur (en bas) toujours attention au sens le faire a l'inverse de l'inverse de ce qu'on pense car il doit etre visible par en bas
            triangles[triIndex++] = bottomStartIndex + (i * 2);
            triangles[triIndex++] = bottomStartIndex + (nextIndex * 2);
            triangles[triIndex++] = bottomStartIndex;

            // Couvercle sup�rieur (en haut) lui a l'inverse car on veut le voir d'en haut
            triangles[triIndex++] = topStartIndex;
            triangles[triIndex++] = topStartIndex + (nextIndex * 2);
            triangles[triIndex++] = topStartIndex + (i * 2);



        }
    }


    void Update()
    {
    }
}
