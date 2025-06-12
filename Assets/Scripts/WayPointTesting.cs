using UnityEngine;

public class WayPointTesting : MonoBehaviour
{
    public Waypoint waypoint;
    public Vector3 offset = new Vector3(0, -0.65f, 0);

    private void Start()
    {
        transform.position = waypoint.transform.position + offset;
        transform.rotation = waypoint.GetRotation();
    }
}
