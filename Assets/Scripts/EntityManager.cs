using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private List<Entity> entities = new List<Entity>();
    private List<Entity> entitiesAwake = new List<Entity>();

    void TriggerSpawn()
    {
        int rnd = Random.Range(0, entities.Count - 1);
        Entity ent = entities[rnd];
        entitiesAwake.Add(ent);
        entities.Remove(ent);

        Instantiate(ent.gameObject);
    }

    void TriggerMove()
    {
        if (entitiesAwake.Count == 0)
        {
            return;
        }

        int rnd = Random.Range(0, entitiesAwake.Count - 1);
        Entity ent = entitiesAwake[rnd];

        rnd = Random.Range(0, ent.GetWaypoint().GetNeighbours().Count);
        Waypoint nextWaypoint = ent.GetWaypoint().GetNeighbours()[rnd];


    }

}
