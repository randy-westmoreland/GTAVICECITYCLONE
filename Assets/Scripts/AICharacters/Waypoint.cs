using UnityEngine;

/// <summary>
/// Represents a waypoint in the game world for AI navigation.
/// </summary>
public class Waypoint : MonoBehaviour
{
    [Header("Waypoint Status")]
    [SerializeField] private Waypoint _previousWaypoint;
    [SerializeField] private Waypoint _nextWaypoint;

    [Range(0f, 10f)]
    [SerializeField] private float _waypointWidth = 5f;

    /// <summary>
    /// Gets or sets the previous waypoint in the sequence.
    /// </summary>
    public Waypoint PreviousWaypoint { get => _previousWaypoint; set => _previousWaypoint = value; }

    /// <summary>
    /// Gets or sets the next waypoint in the sequence.
    /// </summary>
    public Waypoint NextWaypoint { get => _nextWaypoint; set => _nextWaypoint = value; }

    /// <summary>
    /// Gets or sets the width of the waypoint for navigation purposes.
    /// </summary>
    public float WaypointWidth { get => _waypointWidth; set => _waypointWidth = value; }

    /// <summary>
    /// Gets a random position within the waypoint's width for AI characters to navigate to.
    /// </summary>
    /// <returns>A random position within the waypoint's width.</returns>
    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * WaypointWidth / 2f;
        Vector3 maxBound = transform.position - transform.right * WaypointWidth / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }
}
