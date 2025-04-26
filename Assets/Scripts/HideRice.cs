using UnityEngine;

public class HideRice : Interactable
{
    [SerializeField] private GameObject bowl;
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
            ActionMaster.SetRiceHidden(false);
        }
        else
        {
            SetName("remove bowl");
            SetDescribtion("removing bowl...");
            bowl.SetActive(true);
            ActionMaster.SetRiceHidden(true);
        }
        Done.Invoke();
    }
}
