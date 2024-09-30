using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_PacMan : MonoBehaviour
{
    public float rayon = 1.0f;   // Rayon de la sphère
    public int meridiens = 24;   // Nombre de segments horizontaux (méridiens)
    public int paralleles = 16;  // Nombre de segments verticaux (parallèles)
    public float sliceStartAngle = 0f; // Angle de départ pour supprimer une tranche (en radians)
    public float sliceAngleWidth = Mathf.PI / 4; // Largeur de la tranche à supprimer (en radians)

    // Données du mesh
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    // L'objet mesh et le mesh
    private GameObject meshObject;
    private Mesh mesh;

    void Start()
    {
        // Générer les données de la sphère
        GenerateSphereData();

        // Créer le mesh
        mesh = new Mesh();
        mesh.name = "Pacman Mesh";
        meshObject = new GameObject("Mesh Pacman", typeof(MeshRenderer), typeof(MeshFilter));
        meshObject.GetComponent<MeshFilter>().mesh = mesh;

        // Affecter les données du mesh
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private void GenerateSphereData()
    {
        // Calculer le nombre de vertices et de triangles
        int verticesCount = (meridiens + 1) * (paralleles + 1) + 1; // vertices + centre
        vertices = new Vector3[verticesCount]; // Tableau des vertices
        uv = new Vector2[verticesCount]; // Tableau des UVs

        List<int> triangleList = new List<int>(); // Liste temporaire pour stocker les triangles

        float meridienStep = 2 * Mathf.PI / meridiens; // Step pour la longitude
        float paralleleStep = Mathf.PI / paralleles;   // Step pour la latitude

        int vertIndex = 0;
        int centerIndex = verticesCount - 1; // Dernier index pour le centre de la sphère

        // Générer les vertices et UVs
        for (int lat = 0; lat <= paralleles; lat++) // Latitude
        {
            // valeurs pour la latitude
            float theta = lat * paralleleStep;
            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);

            for (int lon = 0; lon <= meridiens; lon++) // Longitude
            {
                // valeurs pour la longitude
                float phi = lon * meridienStep;
                float sinPhi = Mathf.Sin(phi);
                float cosPhi = Mathf.Cos(phi);

                // Calculer les coordonnées du vertex
                float x = sinTheta * cosPhi * rayon;
                float y = cosTheta * rayon;
                float z = sinTheta * sinPhi * rayon;

                vertices[vertIndex] = new Vector3(x, y, z); // Ajouter le vertex

                // UV mapping 
                uv[vertIndex] = new Vector2((float)lon / meridiens, (float)lat / paralleles);

                // Générer les triangles pour les faces de la sphère
                if (lat < paralleles && lon < meridiens)  // Ne pas générer de triangles pour le dernier parallèle et le dernier méridien
                {
                    // Vérifier si cette longitude se trouve pas dans la tranche à supprimer comme ca on ne génère pas de triangles pour le trou
                    if (phi < sliceStartAngle || phi >= sliceStartAngle + sliceAngleWidth) // Si la longitude n'est pas dans la tranche
                    {
                        int nextLat = lat + 1;
                        int nextLon = lon + 1;

                        // Ajouter les triangles 1 !! attention à l'ordre des vertices
                        triangleList.Add(vertIndex);
                        triangleList.Add(vertIndex + meridiens + 2);
                        triangleList.Add(vertIndex + meridiens + 1);
                        // Ajouter les triangles 2
                        triangleList.Add(vertIndex);
                        triangleList.Add(vertIndex + 1);
                        triangleList.Add(vertIndex + meridiens + 2);
                    }
                }

                vertIndex++;
            }
        }

        // Ajouter le point central
        vertices[centerIndex] = Vector3.zero; // Centre de la sphère

        // Créer les triangles reliant les bords du trou au centre
        for (int lat = 0; lat < paralleles; lat++)
        {
            // Pour le bord gauche de la tranche (sliceStartAngle)
            int leftIndex = lat * (meridiens + 1) + Mathf.FloorToInt(sliceStartAngle / meridienStep);
            int leftNextLatIndex = leftIndex + (meridiens + 1);

            // Triangle reliant le centre au bord gauche (sliceStartAngle)
            triangleList.Add(leftIndex);
            triangleList.Add(centerIndex);
            triangleList.Add(leftNextLatIndex);

            // Pour le bord droit de la tranche (sliceStartAngle + sliceAngleWidth)
            int rightIndex = lat * (meridiens + 1) + Mathf.FloorToInt((sliceStartAngle + sliceAngleWidth) / meridienStep);
            int rightNextLatIndex = rightIndex + (meridiens + 1);

            // Triangle reliant le centre au bord droit (sliceEndAngle)
            triangleList.Add(rightNextLatIndex);
            triangleList.Add(centerIndex);
            triangleList.Add(rightIndex);
        }

        // Convertir la liste des triangles en tableau
        triangles = triangleList.ToArray();
    }

    void Update()
    {
    }
}
