using UnityEngine;

public class HideRice : Interactable
{
    [SerializeField] private GameObject bowl;
    [SerializeField] private AudioClip open;
    [SerializeField] private AudioClip close;
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        SetIsUIInteraction(false);
        SetLocksPlayer(true);
        SetUnlocksCursor(false);
        SetDescribtion("hiding rice...");
        SetName("hide rice");
        bowl.SetActive(false);
        SetDuration(2f);

    }

    public override void StartInteraction()
    {
        Invoke("Hide", 2f);
    }
    public override void StoppInteraction()
    {
        CancelInvoke();
    }
    private void Hide()
    {
        if (bowl.activeSelf)
        {
            SetName("hide rice");
            SetDescribtion("hiding rice...");
            bowl.SetActive(false);
            audioSource.clip = open;
            audioSource.Play();
            ActionMaster.SetRiceHidden(false);
        }
        else
        {
            SetName("remove bowl");
            SetDescribtion("removing bowl...");
            bowl.SetActive(true);
            audioSource.clip = close;
            audioSource.Play();
            ActionMaster.SetRiceHidden(true);
        }
        Done.Invoke();
    }
}
