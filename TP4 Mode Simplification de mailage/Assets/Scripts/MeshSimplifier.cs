using System.Collections.Generic;
using UnityEngine;

public class MeshSimplifier
{
    // Simplifie un mesh en utilisant une grille pour grouper les sommets
    public Mesh SimplifyMeshByGrid(Mesh mesh, float cellSize)
    {
        Vector3 minBound = mesh.bounds.min;
        Vector3 maxBound = mesh.bounds.max;
        //Debug.Log($"Limites du maillage - Min : {minBound}, Max : {maxBound}, Taille de cellule : {cellSize}");

        // Dictionnaire pour stocker les sommets regroupés par cellules
        Dictionary<Vector3Int, List<Vector3>> grid = new Dictionary<Vector3Int, List<Vector3>>();
        List<Vector3> vertices = new List<Vector3>(mesh.vertices);
        List<int> triangles = new List<int>(mesh.triangles);


        // Regroupement des sommets dans les cellules de la grille
        foreach (Vector3 vertex in vertices)
        {
            Vector3Int cellIndex = GetCellIndex(vertex, minBound, cellSize);
            if (!grid.ContainsKey(cellIndex))
            {
                grid[cellIndex] = new List<Vector3>();
            }
            grid[cellIndex].Add(vertex);
        }

        //Debug.Log($"Cellules de la grille créées : {grid.Count}");

        // Calcul des sommets représentatifs pour chaque cellule
        Dictionary<Vector3Int, Vector3> representativeVertices = new Dictionary<Vector3Int, Vector3>();
        foreach (var cell in grid)
        {
            Vector3 mergedPosition = AveragePosition(cell.Value);
            representativeVertices[cell.Key] = mergedPosition;
            //Debug.Log($"Cellule {cell.Key} contient {cell.Value.Count} sommets fusionnés en {mergedPosition}");
        }

        List<int> newTriangles = new List<int>();
        //Debug.Log("Mise à jour des triangles avec les sommets représentatifs...");

        // Mise à jour des indices des triangles avec les sommets représentatifs
        foreach (var triIndex in triangles)
        {
            Vector3 vertex = vertices[triIndex];
            Vector3Int cellIndex = GetCellIndex(vertex, minBound, cellSize);
            newTriangles.Add(AddOrGetVertex(representativeVertices[cellIndex]));
        }

        //Debug.Log($"Maillage simplifié final - Sommets : {vertexList.Count}, Triangles : {newTriangles.Count / 3}");

        // Création du nouveau mesh simplifié
        Mesh newMesh = new Mesh();
        newMesh.vertices = vertexList.ToArray();
        newMesh.triangles = newTriangles.ToArray();
        newMesh.RecalculateNormals();
        return newMesh;
    }

    // Calcul de l'index de la cellule en fonction de la position du sommet
    private Vector3Int GetCellIndex(Vector3 position, Vector3 minBound, float cellSize)
    {
        return new Vector3Int(
            Mathf.FloorToInt((position.x - minBound.x) / cellSize),
            Mathf.FloorToInt((position.y - minBound.y) / cellSize),
            Mathf.FloorToInt((position.z - minBound.z) / cellSize)
        );
    }

    // Calcule la position moyenne d'une liste de sommets
    private Vector3 AveragePosition(List<Vector3> vertices)
    {
        Vector3 sum = Vector3.zero;
        foreach (Vector3 v in vertices) sum += v;
        return sum / vertices.Count;
    }

    // Liste des sommets simplifiés et dictionnaire pour gérer les indices des sommets
    private List<Vector3> vertexList = new List<Vector3>();
    private Dictionary<Vector3, int> vertexIndexMap = new Dictionary<Vector3, int>();

    // Ajoute un sommet à la liste ou retourne son index s'il existe déjà
    private int AddOrGetVertex(Vector3 vertex)
    {
        if (vertexIndexMap.TryGetValue(vertex, out int index)) return index;
        index = vertexList.Count;
        vertexList.Add(vertex);
        vertexIndexMap[vertex] = index;
        return index;
    }
}
