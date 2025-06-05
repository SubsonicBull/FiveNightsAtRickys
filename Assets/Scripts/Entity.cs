using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Waypoint spawnPoint;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private Waypoint currentWaypoint;
    [SerializeField] private string spawnpoint = "";
    [SerializeField] private bool searchPlayer = true;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float attackSpeed = 8f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool chasePlayer = false;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;

    private Transform player;


    private void Start()
    {
        Spawn();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        //Experimental Defend
        if (Input.GetKeyDown("g"))
        {
            Move(currentWaypoint);
            isAttacking = false;
        }

        //Chase Player
        if (chasePlayer)
        {

            Vector3 direction = player.position - transform.position;
            direction.y = 0f;
            direction.Normalize();
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
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
        transform.position = spawnPoint.transform.position + offset;
        transform.rotation = spawnPoint.GetRotation();
        currentWaypoint = spawnPoint;
        currentWaypoint.Occupy();
    }

    //Entity moves to given waypoint
    public void Move(Waypoint newWaypoint)
    {
        currentWaypoint.Free();
        transform.position = newWaypoint.transform.position + offset;
        transform.rotation = newWaypoint.GetRotation();
        currentWaypoint = newWaypoint;
        currentWaypoint.Occupy();
    }

    //Entity starts an Attack on Player
    public void Attack()
    {
        Debug.Log("Attack!");
        isAttacking = true;
        audioSource.Play();
        Invoke("Enter", 3f);
    }

    void Enter()
    {
        currentWaypoint.GetEnteringAction().Action();
        MoveToEnteringPos(currentWaypoint.GetEnteringAction().GetEnteringPos() + offset, currentWaypoint.GetEnteringAction().GetEnteringRot());
        Invoke("ChasePlayer", 6f);
    }

    //Lerp Towards EnteringPos and Rot
    public void MoveToEnteringPos(Vector3 targetPosition, Quaternion targetRotation)
    {
        StartCoroutine(MoveCoroutine(targetPosition, targetRotation));
    }

    private IEnumerator MoveCoroutine(Vector3 targetPos, Quaternion targetRot)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float elapsed = 0f;

        while (elapsed < attackSpeed)
        {
            float t = elapsed / attackSpeed;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;
    }

    void ChasePlayer()
    {
        transform.position = currentWaypoint.GetEnteringAction().GetJumpscarePos() + offset;
        transform.rotation = currentWaypoint.GetEnteringAction().GetJumpscareRot();
        chasePlayer = true;
    }
}
