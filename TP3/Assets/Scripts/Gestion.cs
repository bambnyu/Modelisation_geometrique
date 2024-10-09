using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gestion : MonoBehaviour
{
    private Mesh_Sphere sphere;
    // Start is called before the first frame update
    void Start()
    {
        sphere = new Mesh_Sphere();
        place_Cube();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // place the cube at the right spot and scale
    private void place_Cube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        float rayon = sphere.rayon;
        // a delete ne sert plus a rien
            //GameObject objectsphere = GameObject.Find("Mesh Sphere");
            //Vector3 position = objectsphere.transform.position;
            //cube.transform.position = position;
            // pas sure d'etre au bon endroit
        cube.transform.position = sphere.center;
        cube.transform.localScale = new Vector3(2*rayon, 2*rayon, 2*rayon);
    }
}
