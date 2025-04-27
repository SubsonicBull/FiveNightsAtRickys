using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField] private string hidingSpotName = "couch";
    private DisplayVignette displayVig;

    private void Start()
    {
        displayVig = GameObject.FindGameObjectWithTag("GlobalVolume").GetComponent<DisplayVignette>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ActionMaster.SetHidingSpot(hidingSpotName);
            ActionMaster.SetPlayerHidden(true);
            displayVig.TurnOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ActionMaster.SetPlayerHidden(false);
            displayVig.TurnOff();
        }
    }

}
