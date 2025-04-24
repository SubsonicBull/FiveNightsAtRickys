using UnityEngine;
using System.Collections;

public class TestInteraction : Interactable
{
    private void Awake()
    {
        SetIsUIInteraction(false);
        SetLocksPlayer(true);
        SetUnlocksCursor(false);
        SetDescribtion("move cube");
        SetDuration(5f);
    }

    public override void StartInteraction()
    {
        Debug.Log("Started Interaction");
        Invoke("Move", 5f);
    }
    public override void StoppInteraction()
    {
        CancelInvoke();
    }
    private void Move()
    {
        transform.position += transform.up;
        Done.Invoke();
    }
}
