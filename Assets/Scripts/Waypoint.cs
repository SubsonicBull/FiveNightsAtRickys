using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private bool occupied = false;
    [SerializeField] private List<Waypoint> neighbours = new List<Waypoint>();
    public List<Waypoint> GetNeighbours() { return neighbours; }

    public bool IsOccupied() { return occupied; }
    public void Occupy() { occupied = true; }
    public void Free() { occupied = false; }
    
}
