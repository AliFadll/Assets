using UnityEngine;

public class LadderEscape : MonoBehaviour
{
    public GameObject interactPanel;
    public GameObject loadingPanel;
    public Transform newPlayerPosition;

    private bool playerInRange = false;
    private Transform player;

    void Start()
    {
        interactPanel.SetActive(false);
        loadingPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            player = other.transform;
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

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(EscapeSequence());
        }
    }

    private System.Collections.IEnumerator EscapeSequence()
    {
        interactPanel.SetActive(false);
        loadingPanel.SetActive(true);

        // Wait 1.5 seconds for loading effect
        yield return new WaitForSeconds(1.5f);

        // Move player
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.position = newPlayerPosition.position;

        if (cc != null) cc.enabled = true;

        loadingPanel.SetActive(false);
    }
}
