using System.Collections;
using System.Collections.Generic;
using System.Globalization; // Pour lire les flottants 
using System.IO; // Pour lire les fichiers
using UnityEngine;

public class OFFMeshLoader : MonoBehaviour
{
    private List<Vector3> vertices; // Liste des sommets
    private List<int> triangles; // Liste des indices des sommets pour former les triangles
    private List<Vector3> normals; // Liste des normales
    private Mesh mesh; // Le mesh Unity

    // Chemin vers le fichier OFF
    public string filePath = "Assets/Models/buddha.off";

    void Start()
    {
        // Initialiser les listes de sommets et de triangles
        vertices = new List<Vector3>();
        triangles = new List<int>();
        normals = new List<Vector3>();

        // Charger le fichier .off
        LoadOFFFile(filePath);

        // Calculer et recentrer le maillage
        CenterMesh();

        // Normaliser la taille du maillage
        NormalizeMesh();

        // Calculer les normales
        CalculateNormals();

        // Créer un nouveau mesh
        mesh = new Mesh();

        // Assigner les sommets et les triangles au mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();

        // Recalculer les normales pour une meilleure illumination
        mesh.RecalculateNormals();

        // Assigner le mesh à l'objet courant
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void LoadOFFFile(string path)
    {
        using (StreamReader sr = new StreamReader(path))
        {
            
            string firstLine = sr.ReadLine();
            
            // Lire le nombre de sommets et de facettes
            string[] info = sr.ReadLine().Split();
            int numVertices = int.Parse(info[0]);
            int numFaces = int.Parse(info[1]);

            // Lire les sommets (3 flottants pour chaque sommet)
            for (int i = 0; i < numVertices; i++)
            {
                string[] vertexInfo = sr.ReadLine().Split(); // Lire une ligne et la séparer en 3 flottants
                float x = float.Parse(vertexInfo[0], CultureInfo.InvariantCulture); // Utiliser InvariantCulture pour lire les flottants avec un point (et non une virgule)
                float y = float.Parse(vertexInfo[1], CultureInfo.InvariantCulture); // Utiliser InvariantCulture pour lire les flottants avec un point (et non une virgule)
                float z = float.Parse(vertexInfo[2], CultureInfo.InvariantCulture); // Utiliser InvariantCulture pour lire les flottants avec un point (et non une virgule)
                vertices.Add(new Vector3(x, y, z));
            }

            // Lire les facettes (le premier entier est toujours 3 pour des triangles)
            for (int i = 0; i < numFaces; i++)
            {
                string[] faceInfo = sr.ReadLine().Split(); // Lire une ligne et la séparer en 4 entiers
                triangles.Add(int.Parse(faceInfo[1])); // Premier sommet
                triangles.Add(int.Parse(faceInfo[2])); // Deuxième sommet
                triangles.Add(int.Parse(faceInfo[3])); // Troisième sommet
            }
        }
    }

    // Méthode pour centrer le maillage
    void CenterMesh()
    {
        // Calculer le centre de gravité (barycentre)
        Vector3 centerOfMass = Vector3.zero;

        // Additionner toutes les positions des sommets
        foreach (Vector3 vertex in vertices)
        {
            centerOfMass += vertex;
        }

        // Diviser par le nombre total de sommets
        centerOfMass /= vertices.Count;

        // Recentrer tous les sommets en soustrayant le centre de gravité
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] -= centerOfMass; // Soustraction car on veut recentrer autour de l'origine (0, 0, 0)
        }

        // Afficher le centre de gravité
        Debug.Log("Centre de gravité : " + centerOfMass);
    }


    void NormalizeMesh()
    {
        // Trouver la plus grande coordonnée en valeur absolue
        float maxCoord = 0f;

        foreach (Vector3 vertex in vertices)
        {
            // Comparer avec la plus grande valeur des coordonnées x, y, z
            maxCoord = Mathf.Max(maxCoord, Mathf.Abs(vertex.x), Mathf.Abs(vertex.y), Mathf.Abs(vertex.z));
        }

        // Diviser chaque sommet par cette valeur maximale pour normaliser
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] /= maxCoord;
        }

    }

    // Méthode pour calculer les normales
    void CalculateNormals()
    {
        // Initialiser les normales à 0
        for (int i = 0; i < vertices.Count; i++)
        {
            normals.Add(Vector3.zero);
        }

        // Calculer la normale pour chaque triangle et ajouter cette normale aux trois sommets
        for (int i = 0; i < triangles.Count; i += 3)
        {
            int index0 = triangles[i];
            int index1 = triangles[i + 1];
            int index2 = triangles[i + 2];

            Vector3 v0 = vertices[index0];
            Vector3 v1 = vertices[index1];
            Vector3 v2 = vertices[index2];

            // Calculer deux vecteurs des côtés du triangle
            Vector3 edge1 = v1 - v0;
            Vector3 edge2 = v2 - v0;

            // Calculer la normale du triangle (produit vectoriel)
            Vector3 triangleNormal = Vector3.Cross(edge1, edge2).normalized;

            // Ajouter cette normale aux sommets correspondants
            normals[index0] += triangleNormal;
            normals[index1] += triangleNormal;
            normals[index2] += triangleNormal;
        }

        // Normaliser les normales pour chaque sommet
        for (int i = 0; i < normals.Count; i++)
        {
            normals[i] = normals[i].normalized;
        }
    }
    // methode pour export de fichier en obj
}
