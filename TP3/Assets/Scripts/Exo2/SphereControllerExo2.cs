using UnityEngine;

public class SphereControllerExo2 : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Vitesse de déplacement de la sphère
    public float potentialIncrement = 1.0f; // Incrément de potentiel par mise à jour
    public VoxelManagerExo2 voxelManager; // Référence vers le gestionnaire de voxels
    public float sphereRadius = 1.5f; // Rayon de la sphère pour définir la zone de voxelisation

    void Update()
    {
        // Déplacer la sphère avec les touches de direction
        float horizontal = Input.GetAxis("Horizontal"); // Axe X avec A/D
        float vertical = Input.GetAxis("Vertical"); // Axe Z avec W/S

        // Utiliser Q et E pour le mouvement vertical (axe Y)
        float yMovement = 0;
        if (Input.GetKey(KeyCode.Q)) yMovement = -1;
        if (Input.GetKey(KeyCode.E)) yMovement = 1;

        Vector3 movement = new Vector3(horizontal, yMovement, vertical) * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // Ajouter du potentiel autour de la position de la sphère pour créer un ensemble de voxels
        Debug.Log($"Adding potential at position: {transform.position}");
        voxelManager.AddPotentialAtPosition(transform.position, sphereRadius, potentialIncrement);
    }
}
