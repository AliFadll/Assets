using UnityEngine;
using TMPro;

public class DoorOpener : MonoBehaviour
{
    [Header("Door Settings")]
    public KeyCode openKey = KeyCode.O;  // Key to open/close
    public float openAngle = 90f;        // How far it rotates when opened
    public float speed = 2f;             // Rotation speed
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    [Header("UI (optional)")]
    public GameObject doorPanel;         // Panel GameObject that contains doorText (assign in Inspector)
    public TMP_Text doorText;            // TextMeshPro text inside panel (assign in Inspector)

    // Internal
    private bool canOpen = false;        // true when player is inside trigger area

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        // Hide UI panel initially (if assigned)
        if (doorPanel != null)
            doorPanel.SetActive(false);
    }

    void Update()
    {
        // Show/hide panel based on player proximity
        if (doorPanel != null)
        {
            if (canOpen)
            {
                doorPanel.SetActive(true);
                if (doorText != null)
                    doorText.text = $"Press {openKey} to open/close the door";
            }
            else
            {
                doorPanel.SetActive(false);
            }
        }

        // Keep your original open/close key handling intact
        if (Input.GetKeyDown(openKey) && canOpen)
        {
            isOpen = !isOpen; // toggle open/close only when player is near
        }

        // Smooth rotation (unchanged)
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * speed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRotation, Time.deltaTime * speed);
        }
    }

    // Enter/Exit trigger for showing UI
    private void OnTriggerEnter(Collider other)
    {
        // make sure your player GameObject has the "Player" tag
        if (other.CompareTag("player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            canOpen = false;
        }
    }
}
