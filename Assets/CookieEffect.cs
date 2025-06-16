using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CookieEffect : MonoBehaviour
{
    public Volume volume;
    public float fadeDuration = 1.0f;

    private LensDistortion lensDistortion;
    private ChromaticAberration chromaticAberration;

    private Coroutine effectCoroutine;

    private bool effectOngoing = false;

    private void Update()
    {
        if (ActionMaster.GetPlayerProtected())
        {
            if (!effectOngoing)
            {
                StartEffect();
                effectOngoing = true;
            }
        }
        else
        {
            if (effectOngoing)
            {
                StopEffect();
                effectOngoing = false;
            }
        }
    }

    private void Awake()
    {
        if (volume == null)
        {
            Debug.LogError("Volume is not assigned!");
            enabled = false;
            return;
        }

        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out chromaticAberration);

        if (lensDistortion != null)
            lensDistortion.intensity.value = 0;

        if (chromaticAberration != null)
            chromaticAberration.intensity.value = 0;
    }

    public void StartEffect()
    {
        if (effectCoroutine != null)
            StopCoroutine(effectCoroutine);

        effectCoroutine = StartCoroutine(FadeEffect(0, -0.6f, 0, 1f));
    }

    public void StopEffect()
    {
        if (effectCoroutine != null)
            StopCoroutine(effectCoroutine);

        effectCoroutine = StartCoroutine(FadeEffect(lensDistortion.intensity.value, 0f, chromaticAberration.intensity.value, 0f));
    }

    private IEnumerator FadeEffect(float startLens, float endLens, float startRGB, float endRGB)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;

            if (lensDistortion != null)
                lensDistortion.intensity.value = Mathf.Lerp(startLens, endLens, t);

            if (chromaticAberration != null)
                chromaticAberration.intensity.value = Mathf.Lerp(startRGB, endRGB, t);

            time += Time.deltaTime;
            yield return null;
        }

        if (lensDistortion != null)
            lensDistortion.intensity.value = endLens;

        if (chromaticAberration != null)
            chromaticAberration.intensity.value = endRGB;

        effectCoroutine = null;
    }
}
