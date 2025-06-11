using UnityEngine;

public class Window : Interactable
{
    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        SetIsUIInteraction(false);
        SetName("scream eieiei");
        SetDescribtion("screaming...");
        SetDuration(0.2f);
        SetLocksPlayer(true);
        SetUnlocksCursor(false);
    }

    public override void StartInteraction()
    {
        ActionMaster.SetIsScreaming(true);
        audioSource.Play();
        Invoke("Stop", 0.1f);
    }

    public override void StoppInteraction()
    {
        ActionMaster.SetIsScreaming(false);
        CancelInvoke();
    }

    void Stop()
    {
        ActionMaster.SetIsScreaming(false);
        Done.Invoke();
    }
}
