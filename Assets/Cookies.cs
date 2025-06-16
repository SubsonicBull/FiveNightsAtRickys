using UnityEngine;

public class Cookies : Interactable
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject coookies;
    private BoxCollider coll;
    void Start()
    {
        SetName("Eat Luis' \"special\" cookies");
        SetDescribtion("eating...");
        SetIsUIInteraction(false);
        SetDuration(1f);
        SetLocksPlayer(false);
        SetUnlocksCursor(false);
        coll = GetComponent<BoxCollider>();
    }
    public override void StartInteraction()
    {
        Invoke("Eat", 1f);
    }

    public override void StoppInteraction()
    {
        CancelInvoke();
    }

    void Eat()
    {
        ActionMaster.SetPlayerProtected(true);
        audioSource.Play();
        Done.Invoke();
        coll.enabled = false;
        Destroy(coookies);
    }
}
