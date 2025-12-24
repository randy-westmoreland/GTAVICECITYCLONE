using UnityEditor;
using UnityEngine;

/// <summary>
/// Limits the editor's frame rate to improve performance and reduce resource usage.
/// </summary>
[InitializeOnLoad]
public class EditorFPSLimiter : MonoBehaviour
{
    /// <summary>
    /// Static constructor to set the target frame rate when the editor loads.
    /// </summary>
    static EditorFPSLimiter()
    {
        Application.targetFrameRate = 60;
    }
}
