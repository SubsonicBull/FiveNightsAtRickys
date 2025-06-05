using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Radio : Interactable
{
    [SerializeField] private List<AudioClip> songs;
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private GameObject radioUI;
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

    public override void UIInteract()
    {
        radioUI.SetActive(true);
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
    }
}
