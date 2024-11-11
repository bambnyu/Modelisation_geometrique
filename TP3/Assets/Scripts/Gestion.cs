using System.Collections.Generic;
using UnityEngine;

public class Gestion : MonoBehaviour
{
    private List<SphereManager> spheres = new List<SphereManager>();

    void Start()
    {
        // Initialisation de deux sph�res
        SphereManager sphere1 = new SphereManager(Vector3.zero, 5.0f, 10.0f, 20);
        SphereManager sphere2 = new SphereManager(new Vector3(10.0f, 0.0f, 0.0f), 5.0f, 10.0f, 20);

        spheres.Add(sphere1);
        spheres.Add(sphere2);

        // Affichage des voxels en contact pour chaque sph�re
        foreach (var sphere in spheres)
        {
            DisplayVoxelsInContactWithSphere(sphere);
        }
    }

    // Afficher les voxels en contact avec chaque sph�re
    void DisplayVoxelsInContactWithSphere(SphereManager sphere)
    {
        List<Bounds> voxels = sphere.GetVoxelsInContact();
        foreach (var voxel in voxels)
        {
            CreateCube(voxel);
        }
    }

    // M�thode pour cr�er un cube bas� sur les limites (Bounds) d'un voxel
    void CreateCube(Bounds bounds)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = bounds.center;
        cube.transform.localScale = bounds.size;
        cube.GetComponent<Renderer>().material.color = Color.green;
    }
}
