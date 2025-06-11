using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Radio : Interactable
{
    [SerializeField] private List<AudioClip> songs;
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private GameObject radioUI;
    [SerializeField] private float timeTillPowerConsumption = 2f;
    private float timer = 0;
    private bool isOff = true;
    private int currentSongIndex = 0;
    private AudioSource audioS;
    void Start()
    {
        SetIsUIInteraction(true);
        SetLocksPlayer(true);
        SetUnlocksCursor(true);
        SetName("Radio");
        audioS = GetComponent<AudioSource>();
        displayText.text = songs[currentSongIndex].name;
        audioS.clip = songs[currentSongIndex];
        audioS.Play();
        ActionMaster.SetSong(songs[currentSongIndex].name);
        radioUI.SetActive(false);
    }

    private void Update()
    {
        if (!isOff)
        {
            timer += Time.deltaTime;
            if (timer >= timeTillPowerConsumption)
            {
                ActionMaster.ConsumePower(1);
                timer = 0;
            }
        }
        if (!isOff && ActionMaster.GetPower() == 0)
        {
            currentSongIndex = 0;
            audioS.clip = songs[currentSongIndex];
            isOff = true;
        }
    }

    public override void UIInteract()
    {
        if (ActionMaster.GetPower() != 0)
        {
            radioUI.SetActive(true);
        }
    }

    public override void QuitUIInteraction()
    {
        radioUI.SetActive(false);
    }
    public void NextSong()
    {
        if (currentSongIndex < songs.Count - 1)
        {
            currentSongIndex++;
            audioS.clip = songs[currentSongIndex];
            audioS.Play();
        }
        else
        {
            currentSongIndex = 0;
            audioS.clip = songs[currentSongIndex];
            audioS.Play();
        }
        displayText.text = songs[currentSongIndex].name;
        ActionMaster.SetSong(songs[currentSongIndex].name);
        if (songs[currentSongIndex].name == "off")
        {
            isOff = true;
        }
        else
        {
            isOff = false;
        }
    }
}
