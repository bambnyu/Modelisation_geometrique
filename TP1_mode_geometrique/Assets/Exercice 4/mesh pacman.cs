using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshpacman : MonoBehaviour
{
    public float rayon = 1.0f; // Rayon de la sphère
    public int nombre_Paralleles = 10; // Nombre de parallèles (lignes horizontales)
    public int nombre_Meridiens = 20; // Nombre de méridiens (lignes verticales)
    public int taille_bouche = 3;

    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    private GameObject meshObject;
    private Mesh mesh;

    void Start()
    {
        GenerateSphereData();

        mesh = new Mesh();
        mesh.name = "Sphère";

        meshObject = new GameObject("Mesh Object Pac man", typeof(MeshRenderer), typeof(MeshFilter));

        meshObject.GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices; // Assigne les sommets
        mesh.uv = uv; // Assigne les coordonnées UV
        mesh.triangles = triangles; // Assigne les triangles

    }

    private void GenerateSphereData()
    {
        int verticesCount = (nombre_Paralleles + 1) * (nombre_Meridiens + 1) + 2; // Sommets pour chaque parallèle + les pôles Nord et Sud
        vertices = new Vector3[verticesCount];
        uv = new Vector2[verticesCount];
        triangles = new int[nombre_Meridiens * nombre_Paralleles * 6]; // 6 indices par face (2 triangles)

        float phiStep = Mathf.PI / nombre_Paralleles; // Division de l'angle en latitude (parallèles)
        float thetaStep = 2 * Mathf.PI / nombre_Meridiens; // Division de l'angle en longitude (méridiens)

        int vertIndex = 0;
        int triIndex = 0;

        // Pôle nord
        vertices[vertIndex] = new Vector3(0, rayon, 0); // Sommet du pôle nord
        uv[vertIndex] = new Vector2(0.5f, 1.0f);
        int poleNord = vertIndex; // Stocker l'index du pôle nord
        vertIndex++;

        // Générer les sommets pour chaque parallèle
        for (int i = 0; i <= nombre_Paralleles; i++)
        {
            float phi = i * phiStep;
            float sinPhi = Mathf.Sin(phi);
            float cosPhi = Mathf.Cos(phi);

            for (int j = 0; j <= nombre_Meridiens ; j++)
            {
                float theta = j * thetaStep;
                float x = rayon * sinPhi * Mathf.Cos(theta);
                float z = rayon * sinPhi * Mathf.Sin(theta);
                float y = rayon * cosPhi;

                vertices[vertIndex] = new Vector3(x, y, z); // Position des sommets
                uv[vertIndex] = new Vector2(j / (float)nombre_Meridiens, 1 - (i / (float)nombre_Paralleles)); // UV mapping
                vertIndex++;
            }
        }

        // Pôle sud
        vertices[vertIndex] = new Vector3(0, -rayon, 0); // Sommet du pôle sud
        uv[vertIndex] = new Vector2(0.5f, 0.0f);
        int poleSud = vertIndex; // Stocker l'index du pôle sud
        vertIndex++;

        // Générer les triangles reliant le pôle nord aux premiers parallèles
        for (int j = 0; j < nombre_Meridiens; j++)
        {
            triangles[triIndex++] = poleNord;
            triangles[triIndex++] = j + 1;
            triangles[triIndex++] = j;
        }

        // Générer les triangles pour le reste de la sphère (entre les parallèles)
        for (int i = 0; i < nombre_Paralleles - 1; i++)
        {
            for (int j = 0; j < nombre_Meridiens - taille_bouche; j++)
            {
                int current = i * (nombre_Meridiens + 1) + j + 1;
                int next = current + nombre_Meridiens + 1;

                // Triangle 1
                triangles[triIndex++] = current;
                triangles[triIndex++] = current + 1;
                triangles[triIndex++] = next;

                // Triangle 2
                triangles[triIndex++] = current + 1;
                triangles[triIndex++] = next + 1;
                triangles[triIndex++] = next ;
            }
        }

        // Générer les triangles reliant le dernier parallèle au pôle sud
        int baseIndex = (nombre_Paralleles - 1) * (nombre_Meridiens + 1);
        for (int j = 0; j < nombre_Meridiens-taille_bouche +1 ; j++)
        {
            triangles[triIndex++] = poleSud;
            triangles[triIndex++] = baseIndex + j;
            triangles[triIndex++] = baseIndex + j + 1;
        }

        // fermer la bouche du pac man  
        for (int i = 0; i < nombre_Paralleles - 1; i++)
        {
            for (int j = nombre_Meridiens - taille_bouche; j < nombre_Meridiens; j++)
            {
                // ici on veut fermer la bouche du pac man pour pas que ce soit du vide
                // donc on veut prendre les cotes et les refermer vers le centre de de la sphere sur tout l'axe y
            }
        }

     }

    void Update()
    {
    }
}


