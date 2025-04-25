using UnityEngine;
using UnityEngine.Events;

public abstract class Interactable : MonoBehaviour
{
    private bool locksPlayer = true;
    private bool unlocksCursor = false;
    private bool isUIInteraction = true;
    private float duration = 1f;
    private string describtion = "performing unnamed_interaction";
    private string interactionName = "unnamed_interaction";
    public UnityEvent Done = new UnityEvent();

    //Getters
    public bool GetLocksPlayer() { return locksPlayer; }
    public bool GetUnlocksCursor() { return unlocksCursor; }
    public bool GetIsUIInteraction() { return isUIInteraction; }

    public float GetDuration() { return duration; }

    public string GetDescribtion() { return describtion; }
    public string GetInteractionName() { return interactionName; }

    //Setters
    public void SetLocksPlayer(bool b) { locksPlayer = b; }
    public void SetUnlocksCursor(bool b) { unlocksCursor = b; }
    public void SetIsUIInteraction(bool b) { isUIInteraction = b; }

    public void SetDuration(float d) { duration = d; }

    public void SetDescribtion(string d) { describtion = d; }
    public void SetName(string s) { interactionName = s; }

    //Function to override for UI-interaction
    public virtual void UIInteract()
    {
        return;
    }

    //Quits the UI-Interaction (like exiting cctv)
    public virtual void QuitUIInteraction()
    {
        return;
    }
    
    //Function to override for starting a non-instant Interaction
    public virtual void StartInteraction()
    {
        return;
    }

    //Function to override for stopping a non-instant Interaction
    public virtual void StoppInteraction()
    {
        return;
    }
}
