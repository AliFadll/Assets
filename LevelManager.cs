using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public TMP_Text levelText; // Drag your "Level Text" here
    public int currentLevel = 1;

    private void Awake()
    {
        // Singleton pattern for easy access
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
        if (levelText != null)
        {
            levelText.text = $"Level: {level}";
        }
    }
}
