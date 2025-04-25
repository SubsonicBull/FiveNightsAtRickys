using UnityEngine;

public class HideComic : Interactable
{
    [SerializeField] private Transform hidePos;
    [SerializeField] private Transform tablePos;
    private bool hidden = true;
    private void Awake()
    {
        SetIsUIInteraction(false);
        SetLocksPlayer(true);
        SetUnlocksCursor(false);
        SetDescribtion("moving comic...");
        SetName("put comic on table");
        SetDuration(4f);
        transform.position = hidePos.position;

    }

    public override void StartInteraction()
    {
        Invoke("Move", 4f);
    }
    public override void StoppInteraction()
    {
        CancelInvoke();
    }
    private void Move()
    {
        if (hidden)
        {
            transform.position = tablePos.position;
            SetName("hide comic");
            hidden = false;
            ActionMaster.SetComicHidden(false);
        }
        else
        {
            transform.position = hidePos.position;
            SetName("put comic on table");
            hidden = true;
            ActionMaster.SetComicHidden(true);
        }
        Done.Invoke();
    }
}