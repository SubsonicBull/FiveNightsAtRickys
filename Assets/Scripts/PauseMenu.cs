using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    private CursorLockMode previousCursorLockMode;
    public static bool gamePaused;


    void Start()
    {
        pauseMenu.SetActive(false);
        gamePaused = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {           
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
        previousCursorLockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
        Cursor.lockState = previousCursorLockMode;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
        ResumeGame();
        Cursor.lockState = CursorLockMode.None;
    }
}
