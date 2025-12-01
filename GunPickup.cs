using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GunPickup : MonoBehaviour
{
    [Header("Gun Settings")]
    public int gunAmmo = 90;           // Ammo when picked up
    public Sprite gunSprite;           // Gun icon

    [Header("UI Elements")]
    public GameObject pickupPanel;     // "Press T" panel
    public TMP_Text pickupText;        // Text inside panel
    public Image gunIcon;              // Inventory gun icon
    public TMP_Text ammoText;          // Ammo counter text

    [Header("Pickup Settings")]
    public float messageDuration = 2f; // How long pickup message shows

    private bool canPickup = false;
    private GameObject currentGun;
    private bool showingMessage = false;

    void Awake()
    {
        // Hide UI elements at start
        if (pickupPanel != null) pickupPanel.SetActive(false);
        if (gunIcon != null) gunIcon.gameObject.SetActive(false);
        if (ammoText != null) ammoText.text = "Ammo: 0";
    }

    void Update()
    {
        if (canPickup && !showingMessage)
        {
            if (pickupPanel != null) pickupPanel.SetActive(true);
            if (pickupText != null) pickupText.text = "Press T to pick up the gun";

            if (Input.GetKeyDown(KeyCode.T))
            {
                PickupGun();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gun"))
        {
            canPickup = true;
            currentGun = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gun"))
        {
            canPickup = false;
            currentGun = null;
            if (!showingMessage && pickupPanel != null) pickupPanel.SetActive(false);
        }
    }

    private void PickupGun()
    {
        if (currentGun == null) return;

        // Show gun in inventory
        if (gunIcon != null && gunSprite != null)
        {
            gunIcon.sprite = gunSprite;
            gunIcon.gameObject.SetActive(true);
        }

        // Update ammo
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + gunAmmo;
        }

        // Destroy gun in the scene
        Destroy(currentGun);

        // Show pickup message
        if (pickupText != null) pickupText.text = "Gun picked up successfully!";
        if (pickupPanel != null) pickupPanel.SetActive(true);

        canPickup = false;
        currentGun = null;
        showingMessage = true;
        StartCoroutine(HidePickupMessage(messageDuration));
    }

    private IEnumerator HidePickupMessage(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (pickupPanel != null) pickupPanel.SetActive(false);

        showingMessage = false;
    }
}
