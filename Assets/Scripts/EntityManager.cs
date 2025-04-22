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
    }
    private void Update()
    {
        //Call TriggerMove() by time delay
        timer += Time.deltaTime;
        if(timer >= 10f)
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
        entitiesAwake.Add(ent);
        entities.Remove(ent);

        Instantiate(ent.gameObject);
    }

    //Moves Random Entity to next Waypoint
    void TriggerMove()
    {
        if (entitiesAwake.Count == 0)
        {
            return;
        }

        int rnd = Random.Range(0, entitiesAwake.Count);
        Entity ent = entitiesAwake[rnd];

        rnd = Random.Range(0, 10);
        Waypoint nextWaypoint;
        if (rnd > 1)
        {
            nextWaypoint = ent.GetWaypoint().GetNeighbours()[0];
        }
        else
        {
            rnd = Random.Range(1, ent.GetWaypoint().GetNeighbours().Count);
            nextWaypoint = ent.GetWaypoint().GetNeighbours()[rnd];
        }
        bool allOccupied = true;
        for(int i = rnd; i < ent.GetWaypoint().GetNeighbours().Count; i++)
        {
            if (!ent.GetWaypoint().GetNeighbours()[i].IsOccupied())
            {
                allOccupied = false;
                break;
            }
        }
        if (allOccupied)
        {
            for (int i = 0; i < rnd; i++)
            {
                if (!ent.GetWaypoint().GetNeighbours()[i].IsOccupied())
                {
                    allOccupied = false;
                    break;
                }
            }
        }
        if (allOccupied)
        {
            TriggerMove();
        }
        

    }
    
}
