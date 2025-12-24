using UnityEditor;
using UnityEngine;

public class WaypointManagerWindow : EditorWindow
{
    [SerializeField] private Transform _waypointOrigin;

    public Transform WaypointOrigin { get => _waypointOrigin; set => _waypointOrigin = value; }

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
            // Create Waypoint Logic
        }
    }
}
