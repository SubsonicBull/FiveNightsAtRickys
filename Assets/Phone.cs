using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject phoneScreen;
    [SerializeField] private float callDelay = 2f;
    private Material material;

    private void Start()
    {
        material = phoneScreen.GetComponent<Renderer>().material;
        Invoke("StartCallAudio", callDelay);
        Invoke("TurnOffScreen", audioSource.clip.length + callDelay);
    }

    void StartCallAudio()
    {
        audioSource.Play();
    }

    void TurnOffScreen()
    {
        material.DisableKeyword("_EMISSION");
    }
}
