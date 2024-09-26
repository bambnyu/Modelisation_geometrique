using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

public class Mesh_rectangle_parametric : MonoBehaviour
{
    public int nombre_Lignes = 10; // def le nombre de ligne que contiendra le rectangle
    public int nb_Colonnes = 10; // def le nombre de colonnes du rectangle
    // dans chaque case il y aura 2 triangles

    private Vector3[] vertices; // tableau de vecteurs qui contiendra les vertices
    private Vector2[] uv; // tableau de vecteurs qui contiendra les coordonnées UV
    private int[] triangles; // tableau d'entiers qui contiendra les triangles

    // pour le mesh
    private GameObject meshObject;
    private Mesh mesh;

    void Start()
    {
        // genere les données du mesh
        GenerateMeshData();

        // genere le mesh
        mesh = new Mesh();
        mesh.name = "Custom mesh";
        // assigne le mesh au meshFilter et au meshRenderer
        meshObject = new GameObject("Mesh Object Parametric", typeof(MeshRenderer), typeof(MeshFilter));
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        // assigne les différents matériaux au mesh

        mesh.vertices = vertices; // assigne les vertices au mesh
        mesh.uv = uv; // assigne les coordonnées UV
        mesh.triangles = triangles; // assigne les triangles
    }

    private void GenerateMeshData()
    {
        int verticesCount = (nombre_Lignes + 1) * (nb_Colonnes + 1); // nombre de vertices dans le rectangle (nb de lignes + 1 * nb de colonnes + 1) car on a besoin d'un vertex en plus pour les triangles
        vertices = new Vector3[verticesCount]; // tableau de vecteurs qui contiendra les vertices (permet de definir la taille du tableau)
        uv = new Vector2[verticesCount]; // tableau de vecteurs qui contiendra les coordonnées UV(permet de definir la taille du tableau)
        triangles = new int[nombre_Lignes * nb_Colonnes * 6]; // tableau d'entiers qui contiendra les triangles nb de lignes * nb de colonnes * 6 (2 triangles par case)

        // calcul des pas en fonction du nombre de lignes et de colonnes
        float stepX = 1.0f / nb_Colonnes;
        float stepY = 1.0f / nombre_Lignes;

        // Genere les vertices et les coordonnées UV
        int vertIndex = 0; // index pour les vertices
        for (int y = 0; y <= nombre_Lignes; y++)
        {
            for (int x = 0; x <= nb_Colonnes; x++)
            {
                vertices[vertIndex] = new Vector3(x * stepX, y * stepY, 0); // genere les vertices en fonction des pas et de l'index
                uv[vertIndex] = new Vector2(x / (float)nb_Colonnes, y / (float)nombre_Lignes); // genere les coordonnées UV
                vertIndex++; // on augmente l'index pour passer au suivant
            }
        }

        // Genere les triangles 2 par 2
        int triIndex = 0; // index pour les triangles
        for (int y = 0; y < nombre_Lignes; y++)
        {
            for (int x = 0; x < nb_Colonnes; x++)
            {
                // calcul des 4 indices
                int topLeft = y * (nb_Colonnes + 1) + x;
                int topRight = topLeft + 1;
                int bottomLeft = topLeft + (nb_Colonnes + 1);
                int bottomRight = bottomLeft + 1;


                // !ordre pour le voir faire l'inverse
                // permier triangle
                triangles[triIndex++] = topLeft;
                triangles[triIndex++] = bottomRight;
                triangles[triIndex++] = topRight;

                // deuxieme triangle
                triangles[triIndex++] = topLeft;
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = bottomRight;
            }
        }
    }

    void Update()
    {
    }
}
