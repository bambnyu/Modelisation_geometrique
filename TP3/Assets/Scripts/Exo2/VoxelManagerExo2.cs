using UnityEngine;

public class VoxelManagerExo2 : MonoBehaviour
{
    public float octreeSize = 10.0f;
    public float visibilityThreshold = 1.0f;
    public int maxDepth = 5;

    private OctreeExo2 octree;

    void Start()
    {
        // Initialize the octree
        octree = new OctreeExo2(Vector3.zero, octreeSize, visibilityThreshold, maxDepth);
    }

    public void AddPotentialAtPosition(Vector3 position, float amount)
    {
        octree.AddPotential(position, amount);
    }
}
