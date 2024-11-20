using UnityEngine;
using System.Collections.Generic;

public class Exercice3 : MonoBehaviour
{
    [Header("Points de contrôle (Utilisez des sphères)")]
    public List<Transform> controlPoints; // Liste des Transforms pour les points de contrôle

    [Header("Résolution de la courbe")]
    public int resolution = 50;

    private int selectedPoint = -1; // Aucun point sélectionné par défaut

    void OnDrawGizmos()
    {
        if (controlPoints.Count < 2) return;

        // Dessiner les points de contrôle
        Gizmos.color = Color.blue;
        foreach (Transform point in controlPoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, 0.1f);
        }

        // Dessiner le polygone de contrôle
        Gizmos.color = Color.green;
        for (int i = 0; i < controlPoints.Count - 1; i++)
        {
            if (controlPoints[i] != null && controlPoints[i + 1] != null)
                Gizmos.DrawLine(controlPoints[i].position, controlPoints[i + 1].position);
        }

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

        // Mettre en évidence le point de contrôle sélectionné
        if (selectedPoint >= 0 && selectedPoint < controlPoints.Count && controlPoints[selectedPoint] != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(controlPoints[selectedPoint].position, 0.15f);
        }
    }

    // Algorithme récursif de De Casteljau
    private Vector3 DeCasteljau(List<Transform> points, float t)
    {
        if (points.Count == 1)
            return points[0].position;

        List<Transform> nextPoints = new List<Transform>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (points[i] != null && points[i + 1] != null)
            {
                Vector3 interpolated = Vector3.Lerp(points[i].position, points[i + 1].position, t);

                // Créer un objet temporaire pour stocker la position interpolée
                GameObject temp = new GameObject("TempPoint");
                temp.transform.position = interpolated;
                nextPoints.Add(temp.transform);
            }
        }

        Vector3 result = DeCasteljau(nextPoints, t);

        // Supprimer les objets temporaires
        foreach (Transform tempPoint in nextPoints)
        {
            DestroyImmediate(tempPoint.gameObject);
        }

        return result;
    }

    // Méthode pour calculer un point sur la courbe avec De Casteljau
    private Vector3 CalculateBezierPoint(float t)
    {
        return DeCasteljau(controlPoints, t);
    }
}
