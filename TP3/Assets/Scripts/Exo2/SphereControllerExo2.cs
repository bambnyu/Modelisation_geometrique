using UnityEngine;

public class SphereControllerExo2 : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float potentialIncrement = 0.1f;
    public VoxelManagerExo2 voxelManager;

    void Update()
    {
        // Move sphere with arrow keys
        float horizontal = Input.GetAxis("Horizontal"); // A/D for x-axis
        float vertical = Input.GetAxis("Vertical"); // W/S for z-axis

        // Use Q and E for y-axis movement
        float yMovement = 0;
        if (Input.GetKey(KeyCode.Q)) yMovement = -1;
        if (Input.GetKey(KeyCode.E)) yMovement = 1;

        Vector3 movement = new Vector3(horizontal, yMovement, vertical) * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // Add potential at the sphere's position
        voxelManager.AddPotentialAtPosition(transform.position, potentialIncrement);
    }
}
