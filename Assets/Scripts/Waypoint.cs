using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private bool occupied = false;
    [SerializeField] private bool isAttackPoint = false;
    [SerializeField] private List<Waypoint> neighbours = new List<Waypoint>();
    public List<Waypoint> GetNeighbours() { return neighbours; }

    public bool GetIsAttackPoint() { return isAttackPoint; }
    public bool IsOccupied() { return occupied; }
    public void Occupy() { occupied = true; }
    public void Free() { occupied = false; }

    private void OnDrawGizmos()
    {
        foreach(Waypoint w in neighbours)
        {
            Gizmos.DrawLine(transform.position, w.transform.position);
            Gizmos.DrawSphere(transform.position + 0.75f * (w.transform.position - transform.position), 0.1f);
            Gizmos.DrawCube(transform.position + 0.75f * (w.transform.position - transform.position) - (w.transform.position - transform.position).normalized * 0.1f, new Vector3(0.1f, 0.1f, 0.1f));
        }
    }
}
