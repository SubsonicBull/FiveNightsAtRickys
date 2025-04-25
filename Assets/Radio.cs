using UnityEngine;

public class Radio : Interactable
{

    void Start()
    {
        SetIsUIInteraction(true);
        SetLocksPlayer(true);
        SetUnlocksCursor(true);
        SetName("Radio");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
