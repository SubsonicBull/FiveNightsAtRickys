using UnityEngine;

public class Cookies : Interactable
{
    private GameObject volume;
    void Start()
    {
        SetName("Eat Luis' \"special\" cookies");
        SetDescribtion("eating...");
        SetIsUIInteraction(false);
        SetDuration(1f);
        SetLocksPlayer(false);
        SetUnlocksCursor(false);
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
        Done.Invoke();
        Destroy(gameObject);
    }
}
