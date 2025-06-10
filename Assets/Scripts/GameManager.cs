using UnityEngine;
using System.Collections;
using TMPro;
public class GameManager : MonoBehaviour
{
    private EntityManager eM;
    private NightUIManager nM;
    [SerializeField] TMP_Text clockText;

    [SerializeField] private float timeTillNextTime;

    private int time = 0;
    private float timer = 0;
    private float timer2 = 0;
    private float timer3 = 0;
    private float moveTime = 10;
    private float triggerAttackTime = 30;
    private int night = 1;

    private bool changingNights = false;
    private int searchPlayerResets = 0;
    private int entitiesSpawned = 0;


    private void Start()
    {
        eM = GameObject.Find("EnitityManager").GetComponent<EntityManager>();
        nM = GameObject.Find("NightUIManager").GetComponent<NightUIManager>();
        nM.DisplayNightUI("Night 1", false);
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
        if (time == 6)
        {
            changingNights = true;
            eM.ResetEntities();
            entitiesSpawned = 0;
            night++;
            nM.DisplayNightUI("Night " + night.ToString(), true);
            time = 0;
            Invoke("ResetTime", 4f);
        }




        //Nights

        if (changingNights || eM.GetAtackOngoing())
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

            case 5:
                if (time == 1 && entitiesSpawned < 1)
                {
                    eM.TriggerSpawn(-1);
                    entitiesSpawned++;
                    timer2 = 0;
                }
                if (time == 2 && entitiesSpawned < 2)
                {
                    eM.TriggerSpawn(-1);
                    eM.TriggerSpawn(-1);
                    entitiesSpawned += 2;
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
                    moveTime = (float)Random.Range(15, 30);
                    break;
                case 2:
                    moveTime = (float)Random.Range(15, 20);
                    break;
                case 3:
                    moveTime = (float)Random.Range(10, 20);
                    break;
                case 5:
                    moveTime = (float)Random.Range(7, 20);
                    break;
                default:
                    moveTime = (float)Random.Range(15, 45);
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
}
