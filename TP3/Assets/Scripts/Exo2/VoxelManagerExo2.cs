using UnityEngine;

public class VoxelManagerExo2 : MonoBehaviour
{
    public float octreeSize = 50.0f; // Taille de l'octree pour couvrir la zone
    public float visibilityThreshold = 1.0f; // Seuil pour rendre les voxels visibles
    public int maxDepth = 7; // Profondeur augmentée pour une plus grande résolution

    private OctreeExo2 octree;

    void Start()
    {
        // Initialiser l'octree avec le centre ajusté
        octree = new OctreeExo2(new Vector3(0, -2, -20), octreeSize, visibilityThreshold, maxDepth);
        Debug.Log("Octree initialized.");
    }

    // Méthode pour ajouter du potentiel avec un rayon autour de la position
    public void AddPotentialAtPosition(Vector3 position, float sphereRadius, float amount)
    {
        Debug.Log($"Adding potential at position {position} with radius {sphereRadius}");
        octree.AddPotential(position, sphereRadius, amount);
    }
}
