using UnityEngine;
using TMPro;

public class BreakerBox : Interactable
{
    [Header("Door Settings")]
    [SerializeField] private GameObject door;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 90f;

    private Quaternion closedRotation;
    private Quaternion openedRotation;
    private bool shouldOpen = false;
    private bool isMoving = false;

    [SerializeField] private TMP_Text powertext;

    private void Start()
    {
        SetIsUIInteraction(true);
        SetLocksPlayer(true);
        SetName("Breaker Box");

        closedRotation = door.transform.localRotation;
        openedRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    private void Update()
    {
        if (isMoving)
        {
            Quaternion targetRotation = shouldOpen ? openedRotation : closedRotation;
            door.transform.localRotation = Quaternion.RotateTowards(
                door.transform.localRotation,
                targetRotation,
                openSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(door.transform.localRotation, targetRotation) < 0.1f)
            {
                door.transform.localRotation = targetRotation;
                isMoving = false;
            }
        }
        powertext.text = ActionMaster.GetPower().ToString() + "%";
    }

    public override void UIInteract()
    {
        shouldOpen = true;
        isMoving = true;
    }

    public override void QuitUIInteraction()
    {
        shouldOpen = false;
        isMoving = true;
    }
}

