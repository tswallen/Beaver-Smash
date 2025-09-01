using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    public float knockbackDistance = 1f;
    public float knockbackDuration = 0.01f;
    private bool isKnockback = false;
    private Vector3 knockbackDirection;
    private float knockbackTimer = 0f;



    void Start()
    {
        currentHealth = maxHealth;

        // Get the EnemyHealthBar component if it's on this GameObject

    }

    public void TakeDamage(float amount, Vector3 attackerPosition)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Calculate knockback direction (away from attacker)
        knockbackDirection = (transform.position - attackerPosition).normalized;

        // Start knockback effect
        isKnockback = true;
        knockbackTimer = knockbackDuration;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Update()
    {
        if (isKnockback)
        {
            if (knockbackTimer > 0)
            {
                float knockbackSpeed = knockbackDistance / knockbackDuration;
                transform.position += knockbackDirection * knockbackSpeed * 1.5f * Time.deltaTime;
                knockbackTimer -= Time.deltaTime;
            }
            else
            {
                isKnockback = false;
            }
        }
    }


    private void Die()
    {
        Debug.Log(name + " has died!");

        // Optional: Add death animation, destroy, etc.
        Destroy(gameObject);
    }
}
