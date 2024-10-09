using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_Sphere 
{
    public float rayon = 1.0f;   // rayon de la sphère
    public int meridiens = 24;  // nombre de segments horizontaux (méridiens)
    public int paralleles = 16;   // nombre de segments verticaux (parallèles)

    // données du mesh
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    // l'objet mesh et le mesh
    private GameObject meshObject;
    private Mesh mesh;


    // pour utiliser son centre pour l'octree
    public Vector3 center;

    public Mesh_Sphere(){
        // Genere les données de la sphère
        GenerateSphereData();

        // Cree le mesh
        mesh = new Mesh();
        mesh.name = "Sphere Mesh";
        meshObject = new GameObject("Mesh Sphere", typeof(MeshRenderer), typeof(MeshFilter));
        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        // on affecte les données du mesh
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        center = (meshObject.transform.position);
    }

    private void GenerateSphereData()
    {
        // Calcule le nombre de vertices et de triangles
        int verticesCount = (meridiens + 1) * (paralleles + 1); // Vertices pour le corps de la sphère
        vertices = new Vector3[verticesCount]; // Tableau des vertices
        uv = new Vector2[verticesCount]; // Tableau des UVs qui fait la même taille que le tableau des vertices (1 UV par vertex)

        int triangleCount = meridiens * paralleles * 6;  // Triangles pour le corps de la sphère (deux par carré) (2*3)
        triangles = new int[triangleCount];

        float meridienStep = 2 * Mathf.PI / meridiens; // taille d'un step pour la longitude (horizontal)
        float paralleleStep = Mathf.PI / paralleles; // taille d'un step pour la latitude (vertical)

        int vertIndex = 0;
        int triIndex = 0;

        // boucle sur la latitude et la longitude pour générer les vertices et les UVs
        for (int lat = 0; lat <= paralleles; lat++) // boucle sur la latitude
        {
            // variable angle de la latitude
            float theta = lat * paralleleStep; // angle de la latitude
            float sinTheta = Mathf.Sin(theta); // sinus de l'angle de la latitude 
            float cosTheta = Mathf.Cos(theta); // cosinus de l'angle de la latitude

            for (int lon = 0; lon <= meridiens; lon++) // boucle sur la longitude
            {
                // variable angle de la longitude
                float phi = lon * meridienStep; // angle de la longitude
                float sinPhi = Mathf.Sin(phi); // sinus de l'angle de la longitude
                float cosPhi = Mathf.Cos(phi); // cosinus de l'angle de la longitude

                // calcul des coordonnées du vertex en fonction de la latitude et de la longitude
                // x = sin(theta) * cos(phi) * rayon
                float x = sinTheta * cosPhi * rayon;
                float y = cosTheta * rayon;
                float z = sinTheta * sinPhi * rayon;

                vertices[vertIndex] = new Vector3(x, y, z); // coordonnées du vertex

                // UV mapping
                uv[vertIndex] = new Vector2((float)lon / meridiens, (float)lat / paralleles);

                // Genere les triangles 
                if (lat < paralleles && lon < meridiens)
                {
                    // On calcule les indices des vertices des 4 coins du carré
                    int nextLat = lat + 1; // latitude suivante
                    int nextLon = lon + 1; // longitude suivante

                    // premier triangle !!!attention à l'ordre des vertices!!!
                    triangles[triIndex++] = vertIndex;
                    triangles[triIndex++] = vertIndex + meridiens + 2;
                    triangles[triIndex++] = vertIndex + meridiens + 1;

                    // Second triangle !!!attention à l'ordre des vertices!!!
                    triangles[triIndex++] = vertIndex;
                    triangles[triIndex++] = vertIndex + 1;
                    triangles[triIndex++] = vertIndex + meridiens + 2;
                }

                vertIndex++;
            }
        }
    }

   
}
