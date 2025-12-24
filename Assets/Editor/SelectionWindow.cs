using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Custom Editor Window to display and manage the current selection in the Unity Editor.
/// </summary>
public class SelectionWindow : EditorWindow
{
    private Vector2 _scrollPos;
    private string _filterText = "";
    private bool _showGameObjectsOnly = false;
    private bool _showAssetsOnly = false;

    /// <summary>
    /// Opens the Selection Window from the Unity Editor menu.
    /// </summary>
    [MenuItem("Window/Selection Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<SelectionWindow>("Selection");
        window.Show();
    }

    private void OnEnable()
    {
        // Repaint whenever the selection changes so the list stays in sync
        Selection.selectionChanged += Repaint;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= Repaint;
    }

    private void OnGUI()
    {
        Object[] selection = Selection.objects;

        EditorGUILayout.LabelField("Current Selection", EditorStyles.boldLabel);
        EditorGUILayout.Space(2);

        // Filter controls
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Filter:", GUILayout.Width(40));
        _filterText = EditorGUILayout.TextField(_filterText);
        if (GUILayout.Button("Clear", GUILayout.Width(50)))
        {
            _filterText = "";
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        _showGameObjectsOnly = EditorGUILayout.Toggle("GameObjects Only", _showGameObjectsOnly);
        _showAssetsOnly = EditorGUILayout.Toggle("Assets Only", _showAssetsOnly);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(4);

        if (selection == null || selection.Length == 0)
        {
            EditorGUILayout.HelpBox("Nothing selected.\nSelect objects in the Scene or Hierarchy to see them here.", MessageType.Info);
            return;
        }

        // Apply filters
        var filteredSelection = ApplyFilters(selection);

        EditorGUILayout.LabelField($"Count: {filteredSelection.Length} of {selection.Length}");
        EditorGUILayout.Space(4);

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        foreach (Object obj in filteredSelection)
        {
            if (obj == null) continue;

            EditorGUILayout.BeginHorizontal();

            // Small icon
            Texture icon = AssetPreview.GetMiniThumbnail(obj);
            if (GUILayout.Button(icon, GUILayout.Width(20), GUILayout.Height(20)))
            {
                EditorGUIUtility.PingObject(obj);
            }

            // Name + type button (click to select)
            if (GUILayout.Button($"{obj.name}  ({obj.GetType().Name})", GUILayout.ExpandWidth(true)))
            {
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            }

            // Remove button (X)
            if (GUILayout.Button("×", GUILayout.Width(20), GUILayout.Height(20)))
            {
                RemoveFromSelection(obj);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(4);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Ping All Filtered"))
        {
            foreach (Object obj in filteredSelection)
            {
                if (obj != null)
                {
                    EditorGUIUtility.PingObject(obj);
                }
            }
        }
        if (GUILayout.Button("Select Filtered Only"))
        {
            Selection.objects = filteredSelection;
        }
        if (GUILayout.Button("Clear All Selection"))
        {
            Selection.objects = new Object[0];
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Delete", GUILayout.Width(100)))
        {
            if (EditorUtility.DisplayDialog("Delete Objects",
                $"Are you sure you want to delete {filteredSelection.Length} object(s)?",
                "Delete", "Cancel"))
            {
                foreach (Object obj in filteredSelection)
                {
                    if (obj != null)
                    {
                        Undo.DestroyObjectImmediate(obj);
                    }
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void RemoveFromSelection(Object objectToRemove)
    {
        List<Object> currentSelection = new(Selection.objects);
        currentSelection.Remove(objectToRemove);
        Selection.objects = currentSelection.ToArray();
    }

    private Object[] ApplyFilters(Object[] objects)
    {
        var filtered = objects.Where(obj => obj != null);

        // Text filter
        if (!string.IsNullOrEmpty(_filterText))
        {
            filtered = filtered.Where(obj =>
                obj.name.ToLower().Contains(_filterText.ToLower()) ||
                obj.GetType().Name.ToLower().Contains(_filterText.ToLower()));
        }

        // Type filters
        if (_showGameObjectsOnly && !_showAssetsOnly)
        {
            filtered = filtered.Where(obj => obj is GameObject);
        }
        else if (_showAssetsOnly && !_showGameObjectsOnly)
        {
            filtered = filtered.Where(obj => !(obj is GameObject));
        }

        return filtered.ToArray();
    }
}
