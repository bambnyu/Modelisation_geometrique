using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_test : MonoBehaviour

{

    private Vector3[] vertices = new Vector3[4];
    private Vector2[] uv = new Vector2[4];
    private int[] triangles = new int[6];

    private GameObject meshObject;
    private Mesh mesh;


    void Start()
    {
        GenerateMeshData();

        mesh = new Mesh();
        mesh.name = "Custom mesh";

        meshObject = new GameObject("Mesh Object", typeof(MeshRenderer), typeof(MeshFilter));
        
        meshObject.GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

    }

    private void GenerateMeshData()
    {
        vertices[0] = new Vector3(0,0,0);
        vertices[1] = new Vector3(0,1,0);
        vertices[2] = new Vector3(1,1,0);
        vertices[3] = new Vector3(1,0,0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;
    }


    void Update()
    {
        
    }

}
