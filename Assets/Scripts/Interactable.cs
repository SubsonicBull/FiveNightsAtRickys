using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private bool locksPlayer = true;
    private bool unlocksCursor = false;
    private bool isUIInteraction = true;

    //Getters
    public bool GetLocksPlayer() { return locksPlayer; }
    public bool GetUnlocksCursor() { return unlocksCursor; }
    public bool GetIsUIInteraction() { return isUIInteraction; }

    //Setters
    public void SetLocksPlayer(bool b) { locksPlayer = b; }
    public void SetUnlocksCursor(bool b) { unlocksCursor = b; }
    public void SetIsUIInteraction(bool b) { isUIInteraction = b; }

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
