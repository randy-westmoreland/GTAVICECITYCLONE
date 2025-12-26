using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for visualizing waypoints in the Unity Editor.
/// </summary>
[InitializeOnLoad()]
public class WaypointEditor
{
    static WaypointEditor()
    {
        // Ensure that the OnDrawGizmos method is registered.
    }

    /// <summary>
    /// Draws gizmos for the Waypoint component in the scene view.
    /// </summary>
    /// <param name="waypoint"></param>
    /// <param name="gizmoType"></param>
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(Waypoint waypoint, GizmoType gizmoType)
    {
        if ((gizmoType & GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.blue * 0.5f;
        }

        Gizmos.DrawSphere(waypoint.transform.position, 0.5f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine
        (
            waypoint.transform.position + (waypoint.transform.right * waypoint.WaypointWidth / 2f),
            waypoint.transform.position - (waypoint.transform.right * waypoint.WaypointWidth / 2f)
        );
    }
}
