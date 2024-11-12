using UnityEngine;

public class SphereControllerExo2 : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Vitesse de d�placement de la sph�re
    public float potentialIncrement = 1.0f; // Incr�ment de potentiel par mise � jour
    public VoxelManagerExo2 voxelManager; // R�f�rence vers le gestionnaire de voxels

    void Update()
    {
        // D�placer la sph�re avec les touches de direction
        float horizontal = Input.GetAxis("Horizontal"); // Axe X avec A/D
        float vertical = Input.GetAxis("Vertical"); // Axe Z avec W/S

        // Utiliser Q et E pour le mouvement vertical (axe Y)
        float yMovement = 0;
        if (Input.GetKey(KeyCode.Q)) yMovement = -1;
        if (Input.GetKey(KeyCode.E)) yMovement = 1;

        Vector3 movement = new Vector3(horizontal, yMovement, vertical) * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // Ajout de potentiel avec message de d�bogage
        Debug.Log($"Adding potential at position: {transform.position}");
        voxelManager.AddPotentialAtPosition(transform.position, potentialIncrement);
    }
}
