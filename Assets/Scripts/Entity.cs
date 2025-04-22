using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Waypoint spawnPoint;
    [SerializeField] private Waypoint currentWaypoint;
    [SerializeField] private string spawnpoint = "";
    [SerializeField] private bool searchPlayer = true;

    private void Start()
    {
        Spawn();
    }

    //Getter
    public Waypoint GetWaypoint() { return currentWaypoint; }
    public bool GetSearchPlayer() { return searchPlayer; }

    //Setter
    public void SetWaypoint(Waypoint w) { currentWaypoint = w; }
    void Spawn()
    {
        spawnPoint = GameObject.Find(spawnpoint).GetComponent<Waypoint>();
        transform.position = spawnPoint.transform.position;
        currentWaypoint = spawnPoint;
        currentWaypoint.Occupy();
    }

    public void Move(Waypoint newWaypoint)
    {
        currentWaypoint.Free();
        transform.position = newWaypoint.transform.position;
        currentWaypoint = newWaypoint;
        currentWaypoint.Occupy();
    }
}
