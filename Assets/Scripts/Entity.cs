using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private Waypoint spawnPoint;
    [SerializeField] private Waypoint currentWaypoint;

    private void Start()
    {
        transform.position = spawnPoint.transform.position;
    }
    public Waypoint GetWaypoint() { return currentWaypoint; }
    public void SetWaypoint(Waypoint w) { currentWaypoint = w; }
    void Spawn()
    {
        transform.position = spawnPoint.transform.position;
        currentWaypoint = spawnPoint;
        currentWaypoint.Occupie();
    }

    public void Move(Waypoint newWaypoint)
    {
        currentWaypoint.Free();
        transform.position = newWaypoint.transform.position;
        currentWaypoint = newWaypoint;
        currentWaypoint.Occupie();
    }
}
