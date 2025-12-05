using UnityEngine;
using TMPro;
using System.Collections;

public class DoorOpener : MonoBehaviour
{
    [Header("Door Settings")]
    public KeyCode openKey = KeyCode.O;
    public float openAngle = 90f;
    public float speed = 2f;
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    [Header("UI (optional)")]
    public GameObject doorPanel;         // UI panel for "Press O to open"
    public TMP_Text doorText;
    public GameObject loadingPanel;      // UI panel for loading/transition
    public float loadingDuration = 1.5f; // Duration to show loading screen

    // Internal
    private bool canOpen = false;        // true when player is inside trigger
    private Transform player;            // reference to player transform

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        if (doorPanel != null)
            doorPanel.SetActive(false);

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

    void Update()
    {
        // Show/hide panel
        if (doorPanel != null)
        {
            doorPanel.SetActive(canOpen);
            if (canOpen && doorText != null)
                doorText.text = $"Press {openKey} to open/close the door";
        }

        // Open/close door
        if (Input.GetKeyDown(openKey) && canOpen)
        {
            isOpen = !isOpen;

            if (isOpen)
            {
                // Update Level to 2 when door opens
                if (LevelManager.Instance != null)
                    LevelManager.Instance.SetLevel(2);

                StartCoroutine(ShowLoadingAndMovePlayer());
            }
        }

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, isOpen ? openRotation : closedRotation, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            canOpen = true;
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            canOpen = false;
        }
    }

    // Coroutine to show loading screen and move player
    private IEnumerator ShowLoadingAndMovePlayer()
    {
        if (doorPanel != null)
            doorPanel.SetActive(false);

        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        yield return new WaitForSeconds(loadingDuration);

        // Move player to next level spawn
        Transform targetPosition = GameObject.Find("NextLevelSpawn")?.transform;
        if (targetPosition != null && player != null)
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.position = targetPosition.position;

            if (cc != null) cc.enabled = true;
        }

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }
}
