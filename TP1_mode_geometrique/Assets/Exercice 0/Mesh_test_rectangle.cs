using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_test : MonoBehaviour

{


    private Vector3[] vertices = new Vector3[4]; // 4 vertices pour un carr� (2 triangles)
    private Vector2[] uv = new Vector2[4]; // 4 uv pour 4 vertices
    private int[] triangles = new int[6]; // 6 indices pour 2 triangles

    // On cr�e un objet meshObject et un mesh pour le stocker
    private GameObject meshObject;
    private Mesh mesh;


    void Start()
    {
        // On g�n�re les donn�es du mesh
        GenerateMeshData();

        // On cr�e un mesh et on l'assigne � l'objet meshObject
        mesh = new Mesh();
        mesh.name = "Mesh test Rectangle";

        // On cr�e un objet meshObject et on lui assigne le mesh
        meshObject = new GameObject("Mesh test", typeof(MeshRenderer), typeof(MeshFilter));
        meshObject.GetComponent<MeshFilter>().mesh = mesh;

        // On assigne les donn�es du mesh aux variables du mesh
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }

    private void GenerateMeshData()
    {
        // On cr�e les vertices du mesh
        // On cr�e des vecteurs 3D (x,y,z) correspondant aux sommets du carr�
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 1, 0);
        vertices[2] = new Vector3(1, 1, 0);
        vertices[3] = new Vector3(1, 0, 0);

        // On cr�e les uv du mesh
        triangles[0] = 0; // On assigne le sommet 0 au triangle 0
        triangles[1] = 1; // On assigne le sommet 1 au triangle 0
        triangles[2] = 2; // On assigne le sommet 2 au triangle 0
        // On cr�e le deuxi�me triangle attentions aux sens des triangles
        triangles[3] = 0; // On assigne le sommet 0 au triangle 1
        triangles[4] = 2; // On assigne le sommet 2 au triangle 1
        triangles[5] = 3; // On assigne le sommet 3 au triangle 1
    }


    void Update()
    {
        // ne sert � rien 
    }

}
