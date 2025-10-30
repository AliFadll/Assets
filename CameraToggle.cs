using UnityEngine;
using UnityEngine.InputSystem; // New Input System

public class CameraToggle : MonoBehaviour
{
    [Header("Cameras")]
    public Camera tpCamera;      // Third-person camera
    public Camera fpCamera;      // First-person camera

    [Header("Player")]
    public Transform player;     // Player root transform

    [Header("Offsets (World Space)")]
    public Vector3 headOffset = new Vector3(0, 1000f, 100f); // First-person head
    public Vector3 tpOffset = new Vector3(0, 1200f, 2400f);  // Third-person behind

    private bool isThirdPerson = true;

    void Start()
    {
        // Ensure initial camera states
        tpCamera.enabled = true;
        fpCamera.enabled = false;

        tpCamera.tag = "MainCamera";
        fpCamera.tag = "Untagged";

        // Ensure Audio Listeners
        EnableAudioListener(tpCamera, tpCamera.enabled);
        EnableAudioListener(fpCamera, fpCamera.enabled);
    }

    void Update()
    {
        // Toggle cameras using new Input System (C key)
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            ToggleCameras();
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Update First-person camera
        if (fpCamera.enabled)
        {
            fpCamera.transform.position = player.position + headOffset;
            fpCamera.transform.rotation = player.rotation;
        }

        // Update Third-person camera
        // Third-person camera: behind the player
        if (tpCamera.enabled)
        {
            // Use player's forward to go behind
            tpCamera.transform.position = player.position - player.forward * tpOffset.z + Vector3.up * tpOffset.y + player.right * tpOffset.x;
            tpCamera.transform.LookAt(player.position + headOffset);
        }

    }

    private void ToggleCameras()
    {
        isThirdPerson = !isThirdPerson;

        tpCamera.enabled = isThirdPerson;
        fpCamera.enabled = !isThirdPerson;

        tpCamera.tag = isThirdPerson ? "MainCamera" : "Untagged";
        fpCamera.tag = !isThirdPerson ? "MainCamera" : "Untagged";

        // Enable Audio Listener only on active camera
        EnableAudioListener(tpCamera, tpCamera.enabled);
        EnableAudioListener(fpCamera, fpCamera.enabled);
    }

    private void EnableAudioListener(Camera cam, bool enabled)
    {
        if (cam != null)
        {
            AudioListener listener = cam.GetComponent<AudioListener>();
            if (listener != null)
            {
                listener.enabled = enabled;
            }
        }
    }
}
