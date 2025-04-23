using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Waypoint spawnPoint;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private Waypoint currentWaypoint;
    [SerializeField] private string spawnpoint = "";
    [SerializeField] private bool searchPlayer = true;

    private void Start()
    {
        Spawn();
    }

    private void Update()
    {
        //Experimental Defend
        if (Input.GetKeyDown("g"))
        {
            Move(currentWaypoint);
            isAttacking = false;
        }
    }

    //Getter
    public Waypoint GetWaypoint() { return currentWaypoint; }
    public bool GetSearchPlayer() { return searchPlayer; }
    public bool GetIsAttacking() { return isAttacking; }

    //Setter
    public void SetWaypoint(Waypoint w) { currentWaypoint = w; }

    //Entity goes to spawnpoint and occupies it
    void Spawn()
    {
        spawnPoint = GameObject.Find(spawnpoint).GetComponent<Waypoint>();
        transform.position = spawnPoint.transform.position;
        currentWaypoint = spawnPoint;
        currentWaypoint.Occupy();
    }

    //Entity moves to given waypoint
    public void Move(Waypoint newWaypoint)
    {
        currentWaypoint.Free();
        transform.position = newWaypoint.transform.position;
        currentWaypoint = newWaypoint;
        currentWaypoint.Occupy();
    }

    //Entity starts an Attack on Player
    public void Attack()
    {
        Debug.Log("Attack!");
        isAttacking = true;
        transform.position = GameObject.Find("Camera").transform.position + GameObject.Find("Camera").transform.forward * 2;
    }
}
