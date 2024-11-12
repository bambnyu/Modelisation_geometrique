using UnityEngine;

public class VoxelManagerExo2 : MonoBehaviour
{
    public float octreeSize = 50.0f; // Taille initiale de l'octree
    public float visibilityThreshold = 1.0f; // Seuil de visibilité pour un voxel
    public int maxDepth = 7; // Profondeur maximale de l'octree

    private OctreeExo2 octree; // Instance de l'octree

    void Start()
    {
        // Initialisation de l'octree avec la taille et le seuil définis
        octree = new OctreeExo2(new Vector3(0, -2, -20), octreeSize, visibilityThreshold, maxDepth);
        //Debug.Log("Octree initialized.");
    }

    // Méthode pour modifier le potentiel d'une région en fonction du mode (ajout ou soustraction)
    public void ModifyPotentialAtPosition(Vector3 position, float sphereRadius, float amount, bool isSubtractionMode)
    {
        if (isSubtractionMode)
        {
            //Debug.Log($"Removing potential at position {position} with radius {sphereRadius}");
            octree.RemovePotential(position, sphereRadius);
        }
        else
        {
            //Debug.Log($"Adding potential at position {position} with radius {sphereRadius}");
            octree.AddPotential(position, sphereRadius, amount);
        }
    }
}
