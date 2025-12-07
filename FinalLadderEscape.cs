using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalLadderEscape : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject interactPanel;      // "Press E to escape"
    public GameObject finalEscapePanel;   // Winning screen panel

    [Header("Final Escape UI")]
    public Text finalMessageText;         // Winning message
    public Button mainMenuButton;         // Button to restart
    public string finalMessage = "You Have Escaped Successfully!\nCongratulations!";

    private bool playerInRange = false;

    private void Start()
    {
        interactPanel.SetActive(false);
        finalEscapePanel.SetActive(false);

        // Setup Main Menu button
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(RestartGame);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            playerInRange = true;
            interactPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            playerInRange = false;
            interactPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        interactPanel.SetActive(false);
        finalEscapePanel.SetActive(true);

        if (finalMessageText != null)
            finalMessageText.text = finalMessage;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // in case game was paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reloads current scene
    }
}
