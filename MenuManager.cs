using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject startMenuCanvas;
    public GameObject pauseMenuCanvas;

    private bool isPaused = false;

    void Start()
    {
        // Show start menu at the beginning
        startMenuCanvas.SetActive(true);
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 0f; // Freeze game until Start is clicked
    }

    void Update()
    {
        // Toggle Pause Menu with P
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    // Start Game Button
    public void StartGame()
    {
        startMenuCanvas.SetActive(false);
        Time.timeScale = 1f; // Start the game
    }

    // Pause
    public void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // Continue Button
    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Restart Game Button
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Exit Game Button
    public void ExitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }

    // Controls Button (example)
    public void ShowControls()
    {
        Debug.Log("Show Controls Panel");
        // You can open another UI panel for controls here
    }
}
