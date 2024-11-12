using UnityEngine;

public class VoxelManagerExo2 : MonoBehaviour
{
    public float octreeSize = 50.0f; // Taille étendue pour couvrir plus de zone
    public float visibilityThreshold = 1.0f; // Seuil pour rendre les voxels visibles
    public int maxDepth = 5; // Profondeur maximale de subdivision de l'octree

    private OctreeExo2 octree;

    void Start()
    {
        // Initialiser l'octree au centre ajusté pour couvrir la zone de mouvement de la sphère
        octree = new OctreeExo2(new Vector3(0, -2, -20), octreeSize, visibilityThreshold, maxDepth);
        Debug.Log("Octree initialized.");
    }

    public void AddPotentialAtPosition(Vector3 position, float amount)
    {
        Debug.Log($"Calling AddPotential at position {position}");
        octree.AddPotential(position, amount);
    }
}
