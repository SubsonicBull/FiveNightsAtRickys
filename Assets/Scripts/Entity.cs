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
    [SerializeField] private float attackSpeed = 5f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool chasePlayer = false;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private characterName character;

    [SerializeField] private AudioClip sign;
    [SerializeField] private AudioClip jumpscareSound;

    private bool checkForPlayer = false;
    private bool executeAttack = false;

    private Quaternion lastRot;
    private Vector3 lastPos;
    private enum characterName
    {
        Ricky,Lukas,Riceman
    }

    private Transform player;


    private void Start()
    {
        Spawn();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
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

        //Checking for Player and required actions/hidingspot
        if (checkForPlayer && !executeAttack)
        {
            bool requiredCharacterActions = false;
            bool requiredHidingSpot = false;

            //required character specific actions
            switch (character)
            {
                case characterName.Ricky:
                    requiredCharacterActions = (ActionMaster.GetSong() == "Phonk");
                    break;
                case characterName.Lukas:
                    requiredCharacterActions = (ActionMaster.GetSong() == "Metal" && !ActionMaster.GetComicHidden() && !ActionMaster.GetRiceHidden());
                    break;
                case characterName.Riceman:
                    requiredCharacterActions = (ActionMaster.GetSong() == "off" && ActionMaster.GetRiceHidden() && ActionMaster.GetComicHidden());
                    break;
            }

            requiredHidingSpot = (ActionMaster.GetHidingSpot() == currentWaypoint.GetRequiredHidingSpot() && ActionMaster.GetPlayerHidden());

            if (!requiredCharacterActions || !requiredHidingSpot)
            {
                executeAttack = true;
            }
        }
    }

    //Getter
    public Waypoint GetWaypoint() { return currentWaypoint; }
    public bool GetSearchPlayer() { return searchPlayer; }
    public bool GetIsAttacking() { return isAttacking; }

    //Setter
    public void SetWaypoint(Waypoint w) { currentWaypoint = w; }
    public void SetSearchPlayer(bool b) { searchPlayer = b; }

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
        audioSource.clip = sign;
        audioSource.Play();
        Invoke("Enter", 6f);
    }

    void Enter()
    {
        checkForPlayer = true;
        currentWaypoint.GetEnteringAction().Action();
        lastPos = transform.position;
        lastRot = transform.rotation;
        MoveToPos(currentWaypoint.GetEnteringAction().GetEnteringPos() + offset, currentWaypoint.GetEnteringAction().GetEnteringRot());
        Invoke("ChasePlayer", 15f);
    }

    //Lerp Towards EnteringPos and Rot
    public void MoveToPos(Vector3 targetPosition, Quaternion targetRotation)
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
        if (executeAttack)
        {
            transform.position = currentWaypoint.GetEnteringAction().GetJumpscarePos() + offset;
            transform.rotation = currentWaypoint.GetEnteringAction().GetJumpscareRot();
            chasePlayer = true;
        }
        else
        {
            StopAttack();
        }
        executeAttack = false;
        checkForPlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (chasePlayer && other.gameObject.tag == "Player")
        {
            GameObject.Find("GameOver").GetComponent<GameOver>().Over();
            chasePlayer = false;
            player.gameObject.GetComponent<Player>().LockPlayer(true);
            audioSource.clip = jumpscareSound;
            audioSource.Play();
        }
    }

    //Stop Attack

    void StopAttack()
    {
        MoveToPos(lastPos, lastRot);
        currentWaypoint.GetEnteringAction().Action();
        Invoke("GoBackToWaypoint", 6f);
    }

    void GoBackToWaypoint()
    {
        Move(currentWaypoint);
        isAttacking = false;
    }
}
