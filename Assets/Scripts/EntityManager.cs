using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private List<Entity> entities = new List<Entity>();
    private List<Entity> entitiesAwake = new List<Entity>();
    private float timer = 0f;

    private void Start()
    {
        TriggerSpawn();
        TriggerSpawn();
    }
    private void Update()
    {
        //Call TriggerMove() after fixed delay
        timer += Time.deltaTime;
        if(timer >= 0.1f)
        {
            timer = 0;
            TriggerMove();
        }
    }

    //Spawns random entity at Spawnpoint
    void TriggerSpawn()
    {
        int rnd = Random.Range(0, entities.Count);
        Entity ent = entities[rnd];
        entitiesAwake.Add(Instantiate(ent.gameObject).GetComponent<Entity>());

        //maybe not so great because prefab is removed
        entities.Remove(ent);
    }

    //Moves Random Entity to next Waypoint
    void TriggerMove()
    {
        Debug.Log("Moved");

        if (entitiesAwake.Count == 0)
        {
            return;
        }

        int rnd = Random.Range(0, entitiesAwake.Count);
        Entity ent = entitiesAwake[rnd];

        if (ent.GetWaypoint().GetNeighbours().Count == 0)
        {
            Debug.Log("no neighbours");
            return;
        }

        //High priority waypoint
        rnd = Random.Range(0, 10);
        int wayPointIndex = 0;
        Waypoint nextWaypoint;
        if (rnd > 2)
        {
            if (ent.GetSearchPlayer())
            {
                //First in list
                wayPointIndex = 0;
                nextWaypoint = ent.GetWaypoint().GetNeighbours()[0];
            }
            else
            {
                //Last in list
                wayPointIndex = ent.GetWaypoint().GetNeighbours().Count - 1;
                nextWaypoint = ent.GetWaypoint().GetNeighbours()[wayPointIndex];
            }
        }
        //Lower priority(all other waypoints)
        else
        {
            rnd = Random.Range(0, ent.GetWaypoint().GetNeighbours().Count);
            wayPointIndex = rnd;
            nextWaypoint = ent.GetWaypoint().GetNeighbours()[rnd];
        }
        bool allOccupied = true;
        for(int i = wayPointIndex; i < ent.GetWaypoint().GetNeighbours().Count; i++)
        {
            if (!ent.GetWaypoint().GetNeighbours()[i].IsOccupied())
            {
                nextWaypoint = ent.GetWaypoint().GetNeighbours()[i];
                allOccupied = false;
                break;
            }
        }
        if (allOccupied)
        {
            for (int i = 0; i < wayPointIndex; i++)
            {
                if (!ent.GetWaypoint().GetNeighbours()[i].IsOccupied())
                {
                    nextWaypoint = ent.GetWaypoint().GetNeighbours()[i];
                    allOccupied = false;
                    break;
                }
            }
        }
        if (allOccupied)
        {
            TriggerMove();
            return;
        }
        ent.Move(nextWaypoint);
    }
    
}
