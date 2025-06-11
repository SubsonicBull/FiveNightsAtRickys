using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private CCTV cctv;
    [SerializeField] private List<Entity> entityPrefabs = new List<Entity>();
    private List<Entity> entities = new List<Entity>();
    private List<Entity> entitiesAwake = new List<Entity>();
    private float timer = 0f;
    private bool attackOngoing = false;
    private Entity attackingEntity;
    private int recursionCounter = 0;

    private void Start()
    {
        ResetEntities();
    }
    private void Update()
    {
        if (attackOngoing)
        {
            if (!attackingEntity.GetIsAttacking())
            {
                attackOngoing = false;
            }
        }
    }

    //Resets Entities
    public void ResetEntities()
    {
        entities.Clear();
        foreach(Entity e in entityPrefabs)
        {
            entities.Add(e);
        }
        foreach (Entity e in entitiesAwake)
        {
            Destroy(e.gameObject);
        }
        entitiesAwake.Clear();
    }

    public bool GetAtackOngoing()
    {
        return attackOngoing;
    }


    //Spawns random entity at Spawnpoint
    public void TriggerSpawn(int i)
    {
        cctv.BlockVision();
        if (i == -1)
        {
            i = Random.Range(0, entities.Count);
        }
        Entity ent = entities[i];
        entitiesAwake.Add(Instantiate(ent.gameObject).GetComponent<Entity>());
        entities.Remove(ent);
    }

    //Moves Random Entity to next Waypoint
    public void TriggerMove()
    {
        cctv.BlockVision();
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

                //Only one Entity on Attackpoint is allowed
                if (GetEntitiesOnAttackPoints().Count > 0 && ent.GetWaypoint().GetNeighbours()[0].GetIsAttackPoint())
                {
                    return;
                }
                nextWaypoint = ent.GetWaypoint().GetNeighbours()[0];
            }
            else
            {
                //Last in list
                wayPointIndex = ent.GetWaypoint().GetNeighbours().Count - 1;

                //Only one Entity on Attackpoint is allowed
                if (GetEntitiesOnAttackPoints().Count > 0 && ent.GetWaypoint().GetNeighbours()[wayPointIndex].GetIsAttackPoint())
                {
                    return;
                }
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
            recursionCounter++;
            if (recursionCounter > 20)
            {
                return;
                Debug.Log("StackOverflowSituation");
            }
            else
            {
                TriggerMove();
                recursionCounter = 0;
                return;
            }
        }
        ent.Move(nextWaypoint);
    }
    
    //Tries an attack from one radnom entity on an Attack-Waypoint
    public void TryRandomAttack()
    {
        List<Entity> entsOnAttackPoint = GetEntitiesOnAttackPoints();
        if (entsOnAttackPoint.Count != 0)
        {
            int rnd = Random.Range(0, entsOnAttackPoint.Count);
            attackOngoing = true;
            attackingEntity = entsOnAttackPoint[rnd];
            entsOnAttackPoint[rnd].Attack();

            //makes sure that entity, who just attacked tends to run away from player
            foreach (Entity e in entitiesAwake)
            {
                e.SetSearchPlayer(true);
            }
            entsOnAttackPoint[rnd].SetSearchPlayer(false);
        }
    }

    //returns all entities on attackpoints
    List<Entity> GetEntitiesOnAttackPoints()
    {
        List<Entity> entitiesOnAttackPoints = new List<Entity>();
        foreach (Entity e in entitiesAwake)
        {
            if (e.GetWaypoint() != null)
            {
                if (e.GetWaypoint().GetIsAttackPoint())
                {
                    entitiesOnAttackPoints.Add(e);
                }
            }
        }

        return entitiesOnAttackPoints;
    }

    //Reactivates Searchplayer-Mode on all Entities awake
    public void ReactivateSearchPlayer()
    {
        foreach(Entity e in entitiesAwake)
        {
            e.SetSearchPlayer(true);
        }
    }
}
