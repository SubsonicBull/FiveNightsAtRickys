using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameoverUI;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private GameObject panel;
    Image im;

    private void Start()
    {
        gameoverUI.SetActive(false);
        im = panel.GetComponent<Image>();
        im.color = new Color(im.color.r, im.color.g, im.color.b, 0);
        gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, 0);
    }
    public void Over()
    {
        gameoverUI.SetActive(true);
        StartCoroutine(FadeIn());
        Invoke("BackToMenu", 5f);
    }
    IEnumerator FadeIn()
    {
        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = elapsed / duration;

            gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, alpha);
            im.color = new Color(im.color.r, im.color.g, im.color.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, 1f);
        im.color = new Color(im.color.r, im.color.g, im.color.b, 1f);
    }

    void BackToMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
    }
}
