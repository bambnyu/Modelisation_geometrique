using UnityEngine;
using System.Collections.Generic;

public class ChaikinCurve : MonoBehaviour
{
    public List<Transform> controlPoints; // Assigner les sph�res 
    public int subdivisions = 2; // Nombre de subdivisions, �viter des valeurs trop �lev�es 20 = danger

    private void OnDrawGizmos()
    {
        if (controlPoints == null || controlPoints.Count < 2) return; // il faut qu'on ait au moins 2 sph�res sinon ca sert � rien

        // R�cup�rer les positions initiales des points de contr�le (sph�res)
        List<Vector3> points = new List<Vector3>();
        foreach (var point in controlPoints)
        {
            points.Add(point.position);
        }

        // Fermer la boucle en ajoutant le premier point � la fin de la liste
        points.Add(points[0]);



        // Dessiner la forme originale en jaune
        Gizmos.color = Color.yellow;
        for (int i = 0; i < points.Count - 1; i++)
        {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }

        // Dessiner le dernier segment pour fermer la boucle originale
        Gizmos.DrawLine(points[points.Count - 1], points[0]);




        // Effectuer les subdivisions de Chaikin pour la courbe liss�e
        List<Vector3> subdividedPoints = new List<Vector3>(points);
        for (int i = 0; i < subdivisions; i++)
        {
            subdividedPoints = ChaikinSubdivision(subdividedPoints);
        }

        // Dessiner la courbe liss�e (subdivis�e) en cyan
        Gizmos.color = Color.cyan;
        for (int i = 0; i < subdividedPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(subdividedPoints[i], subdividedPoints[i + 1]);
        }

        // Dessiner le dernier segment pour fermer la boucle subdivis�e
        Gizmos.DrawLine(subdividedPoints[subdividedPoints.Count - 1], subdividedPoints[0]);
    }

    private List<Vector3> ChaikinSubdivision(List<Vector3> points)
    {
        List<Vector3> subdividedPoints = new List<Vector3>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = points[i];
            Vector3 p1 = points[i + 1];

            // Calculer deux nouveaux points sur le segment entre p0 et p1
            Vector3 newP0 = Vector3.Lerp(p0, p1, 0.25f); // Point � 1/4 de la distance
            Vector3 newP1 = Vector3.Lerp(p0, p1, 0.75f); // Point � 3/4 de la distance

            subdividedPoints.Add(newP0);
            subdividedPoints.Add(newP1);
        }

        // Fermer la boucle en connectant le dernier point au premier
        subdividedPoints.Add(subdividedPoints[0]);

        return subdividedPoints;
    }
}
