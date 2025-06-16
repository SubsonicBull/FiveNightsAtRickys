using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private EntityManager eM;
    private NightUIManager nM;
    [SerializeField] TMP_Text clockText;

    [SerializeField] private float timeTillNextTime;

    [SerializeField] private Astra astra;

    [SerializeField] private Player player;

    private int time = 0;
    private float timer = 0;
    private float timer2 = 0;
    private float timer3 = 0;
    private float moveTime = 10;
    private float triggerAttackTime = 30;
    private int night = 1;

    private bool changingNights = false;
    private bool astraAttackOngoing = false;
    private bool playerScreamed = false;
    private int searchPlayerResets = 0;
    private int astraAttacks = 0;
    private int entitiesSpawned = 0;

    private Vector3 playerStartPos;
    private Quaternion playerStartRotation;


    private void Start()
    {
        eM = GameObject.Find("EnitityManager").GetComponent<EntityManager>();
        nM = GameObject.Find("NightUIManager").GetComponent<NightUIManager>();
        changingNights = true;
        player.LockPlayer(true);
        nM.DisplayStartUI("Night " + night.ToString());
        Invoke("ResetTime", 4f);
        playerStartPos = player.gameObject.transform.position;
        playerStartRotation = player.gameObject.transform.rotation;
    }

    private void Update()
    {
        //Clock
        timer += Time.deltaTime;
        if (timer >= timeTillNextTime)
        {
            time++;
            timer = 0;
        }
        if (!changingNights)
        {
            UpdateClockText();
        }

        //Next night
        if (time == 6 && !eM.GetAtackOngoing())
        {
            changingNights = true;
            player.LockPlayer(true);
            eM.ResetEntities();
            entitiesSpawned = 0;
            astraAttacks = 0;
            if (night == 5)
            {
                nM.DisplayStartUI("You Survived all 5 nights");
                Invoke("BackToMenu", 4f);
            }
            else
            {
                night++;
                nM.DisplayNightUI("Night " + night.ToString(), true);
                time = 0;
                Invoke("ResetTime", 3f);
            }
        }

        //Checking for Scream
        if (astraAttackOngoing && !playerScreamed)
        {
            playerScreamed = ActionMaster.GetIsScreaming();
        }


        //Nights

        if (changingNights || eM.GetAtackOngoing() || astraAttackOngoing)
        {
            return;
        }

        timer2 += Time.deltaTime;
        timer3 += Time.deltaTime;



        switch (night)
        {
            case 1:
                if (time == 1 && entitiesSpawned < 1)
                {
                    eM.TriggerSpawn(0);
                    entitiesSpawned++;
                    timer2 = 0;
                }
                if (time == 3 && searchPlayerResets == 0)
                {
                    eM.ReactivateSearchPlayer();
                    searchPlayerResets++;
                }
                break;

            case 2:
                if (time == 1 && entitiesSpawned < 1)
                {
                    eM.TriggerSpawn(1);
                    entitiesSpawned++;
                    timer2 = 0;
                }
                if (time == 3 && entitiesSpawned < 2)
                {
                    eM.TriggerSpawn(0);
                    entitiesSpawned++;
                }
                break;

            case 3:
                if (time == 1 && entitiesSpawned < 1)
                {
                    eM.TriggerSpawn(-1);
                    entitiesSpawned++;
                    timer2 = 0;
                }
                if (time == 2 && entitiesSpawned < 2)
                {
                    eM.TriggerSpawn(-1);
                    entitiesSpawned++;
                }
                if (time == 3 && entitiesSpawned < 3)
                {
                    eM.TriggerSpawn(-1);
                    entitiesSpawned++;
                }
                break;

            case 4:
                if (time == 1 && entitiesSpawned < 1)
                {
                    StartAstraAttack();
                    astraAttacks++;
                    eM.TriggerSpawn(-1);
                    entitiesSpawned++;
                    timer2 = 0;
                }
                if (time == 2 && entitiesSpawned < 2)
                {
                    eM.TriggerSpawn(-1);
                    entitiesSpawned++;
                }
                if (time == 3 && entitiesSpawned < 3)
                {
                    eM.TriggerSpawn(-1);
                    entitiesSpawned++;
                }
                if (time == 4 && astraAttacks == 1)
                {
                    StartAstraAttack();
                    astraAttacks++;
                }
                break;

            case 5:
                if (time == 1 && entitiesSpawned < 1)
                {
                    StartAstraAttack();
                    astraAttacks++;
                    eM.TriggerSpawn(-1);
                    entitiesSpawned++;
                    timer2 = 0;
                }
                if (time == 2 && entitiesSpawned < 2)
                {
                    StartAstraAttack();
                    astraAttacks++;
                    eM.TriggerSpawn(-1);
                    eM.TriggerSpawn(-1);
                    entitiesSpawned += 2;
                }
                if (time == 3 && astraAttacks == 2)
                {
                    StartAstraAttack();
                    astraAttacks++;
                }
                if (time == 4 && astraAttacks == 3)
                {
                    StartAstraAttack();
                    astraAttacks++;
                }
                if (time == 5 && astraAttacks == 4)
                {
                    StartAstraAttack();
                    astraAttacks++;
                }
                break;

            default:
                if (time == 1 && entitiesSpawned < 1)
                {
                    eM.TriggerSpawn(0);
                    entitiesSpawned++;
                    timer2 = 0;
                }
                break;
        }

        if (timer2 >= moveTime && entitiesSpawned != 0)
        {
            eM.TriggerMove();
            timer2 = 0;
            switch (night)
            {
                case 1:
                    moveTime = (float)Random.Range(10, 18);
                    break;
                case 2:
                    moveTime = (float)Random.Range(10, 18);
                    break;
                case 3:
                    moveTime = (float)Random.Range(8, 16);
                    break;
                case 4:
                    moveTime = (float)Random.Range(4, 16);
                    break;
                case 5:
                    moveTime = (float)Random.Range(3, 10);
                    break;
                default:
                    moveTime = (float)Random.Range(10, 18);
                    break;
            }
        }

        if (timer3 >= triggerAttackTime)
        {
            eM.TryRandomAttack();
            timer3 = 0;
        }
    }

    private void ResetTime()
    {
        time = 0;
        timer = 0;
        changingNights = false;
        player.LockPlayer(false);
        player.gameObject.transform.position = playerStartPos;
        player.gameObject.transform.rotation = playerStartRotation;
        ActionMaster.ResetPower();
    }

    void UpdateClockText()
    {
        if (timer / timeTillNextTime * 60 < 10)
        {
            clockText.text = "0" + time.ToString() + ":0" + Mathf.Floor(timer / timeTillNextTime * 60).ToString() + " AM";
        }
        else
        {
            clockText.text = "0" + time.ToString() + ":" + Mathf.Floor(timer / timeTillNextTime * 60).ToString() + " AM";
        }
    }

    void StartAstraAttack()
    {
        astraAttackOngoing = true;
        astra.TurnOnLights();
        astra.Honk();
        Invoke("StopAstra", 10f);
    }
    void StopAstra()
    {
        if (playerScreamed)
        {
            astra.TurnOffLights();
            astraAttackOngoing = false;
            playerScreamed = false;
        }
        else
        {
            astra.Go();
        }
    }

    void BackToMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }
}
