using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EditorFPSLimiter : MonoBehaviour
{
    static EditorFPSLimiter()
    {
        Application.targetFrameRate = 60;
    }
}
