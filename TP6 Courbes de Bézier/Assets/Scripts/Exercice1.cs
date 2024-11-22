using UnityEngine;

public class Exercice1 : MonoBehaviour
{
    [Header("Points de contrôle et tangentes")]
    public Transform point0; // Sphère pour P0
    public Transform tangent0; // Sphère pour V0 (tangente de P0)
    public Transform point1; // Sphère pour P1
    public Transform tangent1; // Sphère pour V1 (tangente de P1)

    [Header("Résolution de la courbe")]
    public int resolution = 50;

    void OnDrawGizmos()
    {
        if (resolution < 2 || !point0 || !tangent0 || !point1 || !tangent1) return;

        // Courbe de Hermite
        Gizmos.color = Color.red;
        Vector3 previousPoint = CalculateHermitePoint(0);
        for (int i = 1; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 currentPoint = CalculateHermitePoint(t);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }

        // Dessiner les points de contrôle
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(point0.position, 0.1f);
        Gizmos.DrawSphere(point1.position, 0.1f);

        // Dessiner les lignes des tangentes
        Gizmos.color = Color.green;
        Gizmos.DrawLine(point0.position, tangent0.position);
        Gizmos.DrawLine(point1.position, tangent1.position);
    }

    // Équation de la courbe de Hermite
    private Vector3 CalculateHermitePoint(float t)
    {
        Vector3 P0 = point0.position;
        Vector3 V0 = tangent0.position - P0;
        Vector3 P1 = point1.position;
        Vector3 V1 = tangent1.position - P1;

        float h00 = 2 * t * t * t - 3 * t * t + 1;  // Fonction de base 1
        float h10 = t * t * t - 2 * t * t + t;      // Fonction de base 2
        float h01 = -2 * t * t * t + 3 * t * t;     // Fonction de base 3
        float h11 = t * t * t - t * t;             // Fonction de base 4

        return h00 * P0 + h10 * V0 + h01 * P1 + h11 * V1;
    }
}
