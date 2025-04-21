using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCTV : MonoBehaviour, IInteractable 
{

    private Material CCTV_display;
    public Player player_sc;
    public List<Camera> cams;
    bool using_CCTV;
    public GameObject camsparent;
    public GameObject canvas;
    int camIndex = 0;
    public Button nextCamButton;
    public AudioSource nextCamSound;

    void Awake()
    {
        CCTV_display = GetComponent<Renderer>().material;
        using_CCTV = false;
        CCTV_display.DisableKeyword("_EMISSION");
        canvas.SetActive(false);

        cams = new List<Camera>();

        foreach (Transform child in camsparent.transform)
        {
            Camera cam = child.GetComponent<Camera>();
            if (cam != null)
            {
                cams.Add(cam);
            }
        }

        foreach(Camera cam in cams)
        {
            cam.gameObject.SetActive(false);
        }

        cams[0].gameObject.SetActive(true);
    }

    public void Interact()
    {
        if (CCTV_display.IsKeywordEnabled("_EMISSION"))
        {
            CCTV_display.DisableKeyword("_EMISSION");
            using_CCTV = false;
            player_sc.Set_using_CCTV(using_CCTV);
            canvas.SetActive(false);
        }
        else
        {
            CCTV_display.EnableKeyword("_EMISSION");
            using_CCTV = true;
            player_sc.Set_using_CCTV(using_CCTV);
            canvas.SetActive(true);
        }
    }

    void ActivateCam(int index)
    {
        foreach(Camera cam in cams)
        {
            cam.gameObject.SetActive(false);
        }

        if (index >= 0 && index < cams.Count)
        {
            cams[index].gameObject.SetActive(true);
        }

        nextCamSound.Play();
    }

    public void SwitchCam()
    {
        camIndex = (camIndex + 1) % cams.Count;
        ActivateCam(camIndex);
    }


}
