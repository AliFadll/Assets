using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public KeyCode openKey = KeyCode.O;  // Key to open/close
    public float openAngle = 90f;        // How far it rotates when opened
    public float speed = 2f;             // Rotation speed
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        if (Input.GetKeyDown(openKey))
        {
            isOpen = !isOpen; // toggle open/close
        }

        // Smooth rotation
        if (isOpen)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * speed);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRotation, Time.deltaTime * speed);
        }
    }
}
