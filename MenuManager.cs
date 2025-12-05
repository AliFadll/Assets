using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Canvases")]
    public GameObject startMenuCanvas;
    public GameObject pauseMenuCanvas;

    [Header("Gameplay UI")]
    public GameObject inventoryCanvas;
    public GameObject healthCanvas;

    private bool isPaused = false;

    void Start()
    {
        // Show start menu and freeze game
        startMenuCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(false);
        inventoryCanvas.SetActive(false);
        healthCanvas.SetActive(false);
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !startMenuCanvas.activeSelf)
        {
            if (!isPaused) PauseGame();
            else ResumeGame();
        }
    }

    // Start Menu Button
    public void StartGame()
    {
        startMenuCanvas.SetActive(false);
        inventoryCanvas.SetActive(true);
        healthCanvas.SetActive(true);
        Time.timeScale = 1f;
    }

    // Pause Menu Buttons
    public void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        // Stop play mode in editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // Quit the game in build
            Application.Quit();
#endif
    }

    public void ShowControls()
    {
        Debug.Log("Controls Panel Opened");
    }
}
