using UnityEngine;

public class Door : EnteringAction
{
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float rotationSpeed = 2f;

    private bool isRotating = false;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, rotationAngle, 0));
    }

    public override void Action()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateDoor(isOpen ? closedRotation : openRotation));
            isOpen = !isOpen;
        }
    }

    private System.Collections.IEnumerator RotateDoor(Quaternion targetRotation)
    {
        isRotating = true;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }
}

