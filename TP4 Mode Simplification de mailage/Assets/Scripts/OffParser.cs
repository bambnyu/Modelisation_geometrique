using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class OffParser : MonoBehaviour
{
    // Chemin du fichier OFF � charger
    public string filePath = "path a mettre";
    // Facteur de pourcentage pour calculer la taille des cellules en fonction de la taille du mesh (plus il est petit, plus le maillage est d�taill�)
    public float cellSizeFactor = 0.05f;

    // Listes pour stocker les sommets et les triangles du maillage
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    void Start()
    {
        Debug.Log("D�but du processus de chargement du fichier OFF...");
        // Chargement du fichier OFF
        LoadOFFFile(filePath);

        // Cr�ation du mesh � partir des donn�es charg�es
        Mesh mesh = CreateMesh();
        Debug.Log($"Maillage original - Sommets : {mesh.vertexCount}, Triangles : {mesh.triangles.Length / 3}");

        // Calcul d'une taille dynamique de cellule en fonction des dimensions du maillage
        float cellSize = CalculateDynamicCellSize(mesh);
        Debug.Log($"Taille de cellule calcul�e en fonction des dimensions : {cellSize}");

        // Simplification du maillage apr�s le chargement
        MeshSimplifier simplifier = new MeshSimplifier();
        mesh = simplifier.SimplifyMeshByGrid(mesh, cellSize);

        Debug.Log($"Maillage simplifi� - Sommets : {mesh.vertexCount}, Triangles : {mesh.triangles.Length / 3}");

        // Application du maillage simplifi� � l'objet
        ApplyMesh(mesh);
        Debug.Log("Maillage appliqu� avec succ�s.");
    }

    // M�thode pour charger le fichier OFF et extraire les donn�es de sommets et de triangles
    void LoadOFFFile(string path)
    {
        try
        {
            using (StreamReader sr = new StreamReader(path))
            {
                // V�rification du format du fichier (doit commencer par "OFF")
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

    // M�thode pour cr�er un mesh Unity � partir des sommets et des triangles charg�s
    Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    // M�thode pour appliquer le mesh g�n�r� ou simplifi� � l'objet
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
        // Obtention des dimensions de la bo�te englobante
        Vector3 size = mesh.bounds.size;
        float averageDimension = (size.x + size.y + size.z) / 3.0f;

        // Utilisation d'un pourcentage de la dimension moyenne comme taille de cellule
        return averageDimension * cellSizeFactor;
    }
}
