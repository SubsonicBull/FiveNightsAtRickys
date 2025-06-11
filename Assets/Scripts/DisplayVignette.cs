using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DisplayVignette : MonoBehaviour
{
    private float vignetteIntensity = 0.4f;
    private float initialVignetteIntensity = 0f;
    private bool isOn = false;
    private Volume v;

    [SerializeField] private GameObject hiddenText;

    private void Start()
    {
        hiddenText.SetActive(false);
        v = GetComponent<Volume>();
        if (v.profile.TryGet(out Vignette vig))
        {
            initialVignetteIntensity = vig.intensity.value;
        }
    }
    private void Update()
    {
        if (isOn)
        {
            if (v.profile.TryGet(out Vignette vignette))
            {
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, vignetteIntensity, 0.1f);
            }
        }
        else
        {
            if (v.profile.TryGet(out Vignette vignette))
            {
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, initialVignetteIntensity, 0.05f);
            }
        }
    }
    public void TurnOn()
    {
        hiddenText.SetActive(true);
        isOn = true;
    }

    public void TurnOff()
    {
        hiddenText.SetActive(false);
        isOn = false;
    }
}
