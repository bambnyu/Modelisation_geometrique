using UnityEngine;

public class SphereControllerExo2 : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Vitesse de déplacement de la sphère
    public float potentialIncrement = 1.0f; // Valeur du potentiel à ajouter ou soustraire
    public VoxelManagerExo2 voxelManager; // Référence vers le gestionnaire de voxels
    public float sphereRadius = 1.5f; // Rayon de la sphère pour le potentiel

    private bool isSubtractionMode = false; // Indicateur de mode soustraction/ajout

    void Update()
    {
        // Toggle entre ajout et soustraction du potentiel avec la touche Espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSubtractionMode = !isSubtractionMode;
            //Debug.Log("Mode toggle: " + (isSubtractionMode ? "Soustraction" : "Ajout"));
        }

        // Gestion des déplacements avec les touches directionnelles
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float yMovement = 0;
        if (Input.GetKey(KeyCode.Q)) yMovement = -1; // Descente
        if (Input.GetKey(KeyCode.E)) yMovement = 1; // Montée

        // Calcul et application du mouvement en fonction de la vitesse
        Vector3 movement = new Vector3(horizontal, yMovement, vertical) * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // Calcul du changement de potentiel en fonction du mode actuel
        float potentialChange = isSubtractionMode ? -potentialIncrement : potentialIncrement;
        //Debug.Log((isSubtractionMode ? "Removing" : "Adding") + $" potential at position: {transform.position}");

        // Modification du potentiel à la position actuelle de la sphère
        voxelManager.ModifyPotentialAtPosition(transform.position, sphereRadius, potentialChange, isSubtractionMode);
    }
}
