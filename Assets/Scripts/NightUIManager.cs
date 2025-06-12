using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NightUIManager : MonoBehaviour
{
    [SerializeField] private GameObject nightUI;
    [SerializeField] private TMP_Text nightText;
    [SerializeField] private GameObject panel;
    [SerializeField] private AudioSource audioSource;
    Image im;

    private void Start()
    {
        nightUI.SetActive(false);
        im = panel.GetComponent<Image>();
        im.color = new Color(im.color.r, im.color.g, im.color.b, 0);
        nightText.color = new Color(nightText.color.r, nightText.color.g, nightText.color.b, 0);
    }

    public void ChangeText(string t)
    {
        nightText.text = t;
    }

    public void Show()
    {
        StartCoroutine(FadeIn());
    }

    public void Hide()
    {
        StartCoroutine(FadeOut());
    }

    public void DisplayNightUI(string t, bool st)
    {
        audioSource.Play();
        StartCoroutine(DispNightUI(t, st));
    }

    public void DisplayStartUI(string t)
    {
        StartCoroutine(DispStartUI(t));
    }

    IEnumerator DispNightUI(string nightText, bool showTime)
    {
        Show();
        if (showTime)
        {
            ChangeText("6:00AM");
            yield return new WaitForSeconds(3f);
        }
        ChangeText(nightText);
        yield return new WaitForSeconds(2f);
        Hide();
    }

    IEnumerator DispStartUI(string nightT)
    {
        while(im == null)
        {
            yield return null;
        }

        nightUI.SetActive(true);
        nightText.color = new Color(nightText.color.r, nightText.color.g, nightText.color.b, 1f);
        im.color = new Color(im.color.r, im.color.g, im.color.b, 1f);

        ChangeText(nightT);
        yield return new WaitForSeconds(3f);
        Hide();
    }

    IEnumerator FadeIn()
    {
        while(im == null)
        {
            yield return null;
        }
        nightUI.SetActive(true);
        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = elapsed / duration;

            nightText.color = new Color(nightText.color.r, nightText.color.g, nightText.color.b, alpha);
            im.color = new Color(im.color.r, im.color.g, im.color.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        nightText.color = new Color(nightText.color.r, nightText.color.g, nightText.color.b, 1f);
        im.color = new Color(im.color.r, im.color.g, im.color.b, 1f);
    }

    IEnumerator FadeOut()
    {
        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = elapsed / duration;

            nightText.color = new Color(nightText.color.r, nightText.color.g, nightText.color.b, 1 - alpha);
            im.color = new Color(im.color.r, im.color.g, im.color.b, 1 - alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        nightText.color = new Color(nightText.color.r, nightText.color.g, nightText.color.b, 0f);
        im.color = new Color(im.color.r, im.color.g, im.color.b, 0f);
        nightUI.SetActive(false);
    }
}
