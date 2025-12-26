using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom Editor Window to manage waypoints in the Unity Editor.
/// </summary>
public class WaypointManagerWindow : EditorWindow
{
    [SerializeField] private Transform _waypointOrigin;

    /// <summary>
    /// Gets or sets the waypoint origin transform.
    /// </summary>
    public Transform WaypointOrigin { get => _waypointOrigin; set => _waypointOrigin = value; }

    /// <summary>
    /// Opens the Waypoint Manager Window from the Unity Editor menu.
    /// </summary>
    [MenuItem("Waypoint/Waypoints Editor Tools")]
    public static void ShowWindow()
    {
        GetWindow<WaypointManagerWindow>("Waypoints Editor Tools");
    }

    private void OnGUI()
    {
        SerializedObject obj = new(this);
        EditorGUILayout.PropertyField(obj.FindProperty("_waypointOrigin"));

        if (_waypointOrigin == null)
        {
            EditorGUILayout.HelpBox("Please assign a Waypoint Origin Transform.", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            CreateButtons();
            EditorGUILayout.EndVertical();
        }

        obj.ApplyModifiedProperties();
    }

    private void CreateButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }
    }

    private void CreateWaypoint()
    {
        GameObject waypointObject = new("Waypoint " + _waypointOrigin.childCount, typeof(Waypoint));
        waypointObject.transform.SetParent(_waypointOrigin, false);

        Waypoint waypoint = waypointObject.GetComponent<Waypoint>();
        if (_waypointOrigin.childCount > 1)
        {
            waypoint.PreviousWaypoint = _waypointOrigin.GetChild(_waypointOrigin.childCount - 2).GetComponent<Waypoint>();
            waypoint.PreviousWaypoint.NextWaypoint = waypoint;

            waypoint.transform.position = waypoint.PreviousWaypoint.transform.position;
            waypoint.transform.forward = waypoint.PreviousWaypoint.transform.forward;
        }

        Selection.activeGameObject = waypoint.gameObject;
    }
}
