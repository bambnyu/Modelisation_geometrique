using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class OffParser : MonoBehaviour
{
    // Chemin du fichier OFF à charger
    public string filePath = "path a mettre";
    // Facteur de pourcentage pour calculer la taille des cellules en fonction de la taille du mesh (plus il est petit, plus le maillage est détaillé)
    public float cellSizeFactor = 0.05f;

    // Listes pour stocker les sommets et les triangles du maillage
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    void Start()
    {
        Debug.Log("Début du processus de chargement du fichier OFF...");
        // Chargement du fichier OFF
        LoadOFFFile(filePath);

        // Création du mesh à partir des données chargées
        Mesh mesh = CreateMesh();
        Debug.Log($"Maillage original - Sommets : {mesh.vertexCount}, Triangles : {mesh.triangles.Length / 3}");

        // Calcul d'une taille dynamique de cellule en fonction des dimensions du maillage
        float cellSize = CalculateDynamicCellSize(mesh);
        Debug.Log($"Taille de cellule calculée en fonction des dimensions : {cellSize}");

        // Simplification du maillage après le chargement
        MeshSimplifier simplifier = new MeshSimplifier();
        mesh = simplifier.SimplifyMeshByGrid(mesh, cellSize);

        Debug.Log($"Maillage simplifié - Sommets : {mesh.vertexCount}, Triangles : {mesh.triangles.Length / 3}");

        // Application du maillage simplifié à l'objet
        ApplyMesh(mesh);
        Debug.Log("Maillage appliqué avec succès.");
    }

    // Méthode pour charger le fichier OFF et extraire les données de sommets et de triangles
    void LoadOFFFile(string path)
    {
        try
        {
            using (StreamReader sr = new StreamReader(path))
            {
                // Vérification du format du fichier (doit commencer par "OFF")
                string line = sr.ReadLine();
                if (line != "OFF")
                {
                    Debug.LogError("Ce n'est pas un fichier OFF valide");
                    return;
                }

                // Lecture du nombre de sommets et de faces
                string[] info = sr.ReadLine().Split();
                int numVertices = int.Parse(info[0]);
                int numFaces = int.Parse(info[1]);
                Debug.Log($"Nombre de sommets : {numVertices}, Nombre de faces : {numFaces}");

                // Lecture des sommets
                for (int i = 0; i < numVertices; i++)
                {
                    string[] vertexInfo = sr.ReadLine().Split();
                    float x = float.Parse(vertexInfo[0], CultureInfo.InvariantCulture);
                    float y = float.Parse(vertexInfo[1], CultureInfo.InvariantCulture);
                    float z = float.Parse(vertexInfo[2], CultureInfo.InvariantCulture);
                    vertices.Add(new Vector3(x, y, z));
                }
                Debug.Log($"Chargement de {vertices.Count} sommets.");

                // Lecture des faces
                for (int i = 0; i < numFaces; i++)
                {
                    string[] faceInfo = sr.ReadLine().Split();
                    int vertexCountInFace = int.Parse(faceInfo[0]);

                    // On suppose ici que les faces sont triangulaires
                    if (vertexCountInFace == 3)
                    {
                        triangles.Add(int.Parse(faceInfo[1]));
                        triangles.Add(int.Parse(faceInfo[2]));
                        triangles.Add(int.Parse(faceInfo[3]));
                    }
                }
                Debug.Log($"Chargement de {triangles.Count / 3} triangles.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erreur lors de la lecture du fichier OFF : {e.Message}");
        }
    }

    // Méthode pour créer un mesh Unity à partir des sommets et des triangles chargés
    Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    // Méthode pour appliquer le mesh généré ou simplifié à l'objet
    void ApplyMesh(Mesh mesh)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null) meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.mesh = mesh;

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer == null) renderer = gameObject.AddComponent<MeshRenderer>();

        renderer.material = new Material(Shader.Find("Standard"));
    }

    // Calcul d'une taille de cellule dynamique en fonction des dimensions du maillage
    float CalculateDynamicCellSize(Mesh mesh)
    {
        // Obtention des dimensions de la boîte englobante
        Vector3 size = mesh.bounds.size;
        float averageDimension = (size.x + size.y + size.z) / 3.0f;

        // Utilisation d'un pourcentage de la dimension moyenne comme taille de cellule
        return averageDimension * cellSizeFactor;
    }
}
