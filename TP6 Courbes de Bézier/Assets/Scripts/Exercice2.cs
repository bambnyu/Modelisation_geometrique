using UnityEngine;

public class Exercice2 : MonoBehaviour
{
    [Header("Points de contrôle")]
    public Transform point0; // Sphère pour P0
    public Transform point1; // Sphère pour P1
    public Transform point2; // Sphère pour P2
    public Transform point3; // Sphère pour P3

    [Header("Résolution de la courbe")]
    public int resolution = 50;


    void OnDrawGizmos()
    {
        if (resolution < 2 || !point0 || !point1 || !point2 || !point3) return;

        // Dessiner la courbe de Bézier
        Gizmos.color = Color.red;
        Vector3 previousPoint = CalculateBezierPoint(0);
        for (int i = 1; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            Vector3 currentPoint = CalculateBezierPoint(t);
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }

        // Dessiner le polygone de contrôle
        Gizmos.color = Color.green;
        Gizmos.DrawLine(point0.position, point1.position);
        Gizmos.DrawLine(point1.position, point2.position);
        Gizmos.DrawLine(point2.position, point3.position);       
    }

    // Équation de la courbe de Bézier utilisant les polynômes de Bernstein
    private Vector3 CalculateBezierPoint(float t)
    {
        Vector3 P0 = point0.position;
        Vector3 P1 = point1.position;
        Vector3 P2 = point2.position;
        Vector3 P3 = point3.position;

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        // Polynômes de Bernstein
        Vector3 point = uuu * P0; // (1-t)^3 * P0
        point += 3 * uu * t * P1; // 3(1-t)^2 * t * P1
        point += 3 * u * tt * P2; // 3(1-t) * t^2 * P2
        point += ttt * P3;        // t^3 * P3

        return point;
    }

    
}
