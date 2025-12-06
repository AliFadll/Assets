using UnityEngine;

public class ZombieController : MonoBehaviour
{
    private Animator animator;
    private Transform player;

    [Header("Movement Settings")]
    public float speed = 2f;            // How fast zombie moves
    public float chaseDistance = 10f;   // Distance to start chasing

    [Header("Health")]
    public int maxHealth = 100;

    private int currentHealth;
    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();

        // Find player by tag
        GameObject p = GameObject.FindGameObjectWithTag("player");
        if (p != null) player = p.transform;
        else Debug.LogError("Player not found! Make sure your Player is tagged 'Player'.");
    }

    void Update()
    {
        if (player == null || isDead) return;

        // Calculate distance to player
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= chaseDistance)
        {
            // Start chasing
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Rotate zombie to face player
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      5f * Time.deltaTime);

            animator.SetFloat("Speed", speed); // Play Walk/Run animation
        }
        else
        {
            // Idle
            animator.SetFloat("Speed", 0f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);
        Destroy(gameObject, 5f); // Remove after death animation
    }

    // Optional: visualize chase radius in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
