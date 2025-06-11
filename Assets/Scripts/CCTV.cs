using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CCTV : Interactable
{
    private Material CCTV_display;
    [SerializeField] private Material blockVisionMatierial;
    private List<Camera> cams;
    private bool using_CCTV;
    [SerializeField] GameObject camsparent;
    private GameObject canvas;
    [SerializeField] TMP_Text roomText;
    private int camIndex = 0;
    private AudioSource nextCamSound;

    void Awake()
    {
        //Setting Up Interaction
        SetIsUIInteraction(true);
        SetLocksPlayer(true);
        SetUnlocksCursor(true);
        SetName("Cameras");

        CCTV_display = GetComponent<Renderer>().material;

        canvas = GetComponentInChildren<Canvas>().gameObject;

        nextCamSound = GetComponentInChildren<AudioSource>();

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


    public override void UIInteract()
    {
        CCTV_display.EnableKeyword("_EMISSION");
        using_CCTV = true;
        canvas.SetActive(true);
    }

    public override void QuitUIInteraction()
    {
        CCTV_display.DisableKeyword("_EMISSION");
        using_CCTV = false;
        canvas.SetActive(false);
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

        switch (camIndex)
        {
            case 0:
                roomText.text = "ballway";
                break;
            case 1:
                roomText.text = "bathroom";
                break;
            case 2:
                roomText.text = "bedroom_1";
                break;
            case 3:
                roomText.text = "dining_room";
                break;
            case 4:
                roomText.text = "vents";
                break;
            case 5:
                roomText.text = "bedroom_2";
                break;
            case 6:
                roomText.text = "front_porch";
                break;
            default:
                roomText.text = "ERROR";
                break;
        }

        nextCamSound.Play();
    }
    public void SwitchCam()
    {
        camIndex = (camIndex + 1) % cams.Count;
        ActivateCam(camIndex);
    }

    public void BlockVision()
    {
        if (using_CCTV)
        {
            GetComponent<Renderer>().material = blockVisionMatierial;
            Invoke("UnblockVision", 1f);
        }
    }

    void UnblockVision()
    {
        GetComponent<Renderer>().material = CCTV_display;
    }
}
