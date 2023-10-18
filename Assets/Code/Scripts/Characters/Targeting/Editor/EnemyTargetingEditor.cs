using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FOVPursuer))]
public class EnemyTargetingEditor : TargetingEditor
{
    public override void OnSceneGUI()
    {
        base.OnSceneGUI();

        FOVPursuer enemyTargeting;
        enemyTargeting = target as FOVPursuer;
        Vector3 enemyOrigin = (enemyTargeting.PursuitOrigin != default) ? enemyTargeting.PursuitOrigin : enemyTargeting.transform.position;

        Handles.color = Color.blue;
        Handles.DrawWireArc(enemyOrigin, Vector3.up, Vector3.forward, 360, enemyTargeting.MaxPursuitRadius);

        if(enemyTargeting.InPursuit && !Targeting.CanSeeTarget)
        {
            Handles.color = Color.green;
            Handles.DrawLine(Origin.position, Targeting.Target.transform.position);
        }
    }
}
