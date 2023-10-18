using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VisualDetection))]
public class TargetingEditor : Editor
{
    protected VisualDetection Targeting { get; private set; }
    protected Transform Origin { get; private set; }

    public virtual void OnSceneGUI()
    {
        Targeting = target as VisualDetection;
        Origin = Targeting.Origin;
        float currentRadius = (Targeting.CurrentRadius != default) ? Targeting.CurrentRadius : Targeting.Radius;
        float currentAngle = (Targeting.CurrentAngle != default) ? Targeting.CurrentAngle : Targeting.Angle;

        // Determine color based on if the Origin has been set or not
        if (Origin != null)
            Handles.color = Color.white;
        else
        {
            Origin = Targeting.gameObject.transform;
            Handles.color = Color.magenta;
        }

        // Draw Radius visualizer
        Handles.DrawWireArc(Origin.position, Vector3.up, Vector3.forward, 360, currentRadius);

        // Draw View Angle visualizer
        Vector3 viewAngle01 = DirectionFromAngle(Origin.eulerAngles.y, -currentAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(Origin.eulerAngles.y, currentAngle / 2);
        Handles.color = Color.yellow;
        Handles.DrawLine(Origin.position, Origin.position + viewAngle01 * currentRadius, 2f);
        Handles.DrawLine(Origin.position, Origin.position + viewAngle02 * currentRadius, 2f);
        Handles.DrawWireArc(Origin.position, Vector3.up, viewAngle01, currentAngle, currentRadius, 2f);

        // Dray Line indicating that a target is within the FOV
        if (Targeting.CanSeeTarget)
        {
            Handles.color = Color.red;
            //for (int i = 0; i < fov.Targets.Count; i++)
            //    Handles.DrawLine(fovOrigin.position, fov.Targets[i].transform.position); 
            Handles.DrawLine(Origin.position, Targeting.Target.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
