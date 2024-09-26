using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_Cylindre : MonoBehaviour
{
    public float rayon = 1.0f; // Le rayon du cylindre
    public float hauteur = 4.0f; // La hauteur du cylindre
    public int nombre_Meridiens = 20; // Le nombre de méridiens (divisions autour du cylindre) je sais pas combien en mettre de base

    // Les données du mesh
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    // Le GameObject et le Mesh
    private GameObject meshObject;
    private Mesh mesh;

    void Start()
    {
        // on va générer les données du cylindre
        GenerateCylinderData();
        // on va générer le mesh du cylindre
        mesh = new Mesh();
        mesh.name = "Cylindre";
        // on va assigner les données du mesh
        meshObject = new GameObject("Mesh Object Cylindre", typeof(MeshRenderer), typeof(MeshFilter));
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        // on va assigner le material
        mesh.vertices = vertices; // Assigner les sommets
        mesh.uv = uv; // Assigner les coordonnées UV
        mesh.triangles = triangles; // Assigner les triangles
        
    }

    private void GenerateCylinderData()
    {
        // Calculer le nombre de sommets et de triangles
        int verticesCount = (nombre_Meridiens + 1) * 2 + (nombre_Meridiens * 2); // Sommets pour le corps + bords des couvercles
        vertices = new Vector3[verticesCount]; // Sommets pour le corps + bords des couvercles
        uv = new Vector2[verticesCount]; // Coordonnées UV
        triangles = new int[nombre_Meridiens * 12]; // Triangles pour le corps + couvercles

        // Calculer l'angle entre chaque méridien
        float angleStep = 2 * Mathf.PI / nombre_Meridiens;
        // Index des sommets et des triangles
        int vertIndex = 0;
        int triIndex = 0;

        // on va faire ca en 2 parties la partie du cylindre donc le contour puis on fera les couvercles pour le fermer des deux cotés

        // Générer les sommets pour le corps du cylindre (contour)
        for (int i = 0; i <= nombre_Meridiens; i++)
        {
            // Calculer la position du sommet sur le méridien actuel 
            float angle = i * angleStep; // Angle du méridien
            float x = Mathf.Cos(angle) * rayon; // Coordonnée X
            float z = Mathf.Sin(angle) * rayon; // Coordonnée Z

            // Sommets bas du cylindre (contour)
            vertices[vertIndex] = new Vector3(x, 0, z); // Position du sommet
            uv[vertIndex] = new Vector2(i / (float)nombre_Meridiens, 0); // Coordonnées UV
            vertIndex++; // Passer au sommet suivant

            // Sommets haut du cylindre (contour) // on va faire la meme chose que pour le bas mais on va changer la hauteur
            vertices[vertIndex] = new Vector3(x, hauteur, z); // Position du sommet
            uv[vertIndex] = new Vector2(i / (float)nombre_Meridiens, 1); // Coordonnées UV
            vertIndex++; // Passer au sommet suivant

            if (i < nombre_Meridiens)
            {
                // Triangles du corps (contour uniquement)
                int nextIndex = (i + 1) * 2; // Indice du sommet suivant

                // Triangle 1 la galere pour comprendre le sens des triangles
                triangles[triIndex++] = i * 2;
                triangles[triIndex++] = i * 2 + 1;
                triangles[triIndex++] = nextIndex + 1;
                // Triangle 2 toujours attention au sens
                triangles[triIndex++] = i * 2;
                triangles[triIndex++] = nextIndex + 1;
                triangles[triIndex++] = nextIndex;
            }
        }

        // Ajouter les couvercles (supérieur et inférieur) 
        int bottomStartIndex = 0; // Indice du premier sommet du couvercle inférieur
        int topStartIndex = 1; // Indice du premier sommet du couvercle supérieur

        for (int i = 0; i < nombre_Meridiens; i++)
        {
            int nextIndex = (i + 1) % nombre_Meridiens; // Indice du sommet suivant

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
