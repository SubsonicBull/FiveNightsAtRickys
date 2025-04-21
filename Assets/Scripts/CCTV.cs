using UnityEngine;

public class CCTV : MonoBehaviour, IInteractable 
{

    private Material CCTV_display;

    public Player player_sc;

    bool using_CCTV;
    void Awake()
    {
        CCTV_display = GetComponent<Renderer>().material;
        using_CCTV = false;
        CCTV_display.DisableKeyword("_EMISSION");
    }

    public void Interact()
    {
        if (CCTV_display.IsKeywordEnabled("_EMISSION"))
        {
            CCTV_display.DisableKeyword("_EMISSION");
            using_CCTV = false;
            player_sc.Set_using_CCTV(using_CCTV);
        }
        else
        {
            CCTV_display.EnableKeyword("_EMISSION");
            using_CCTV = true;
            player_sc.Set_using_CCTV(using_CCTV);
        }
    }
}
