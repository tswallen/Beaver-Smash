using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRadius = 1f; // The 1-meter range
    public float attackAngle = 100f; // The 100-degree arc
    public LayerMask enemyLayer; // A LayerMask to identify enemies

    void Update()
    {
        // Check for player input to trigger the attack
        // TODO: replace with other input type
        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    void PerformAttack()
    {
        // Use OverlapSphere to find all colliders within the attack radius
        // The array stores all colliders found within the sphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);

        foreach (var hitCollider in hitColliders)
        {
            // Calculate the direction from the player to the enemy
            Vector3 directionToTarget = (hitCollider.transform.position - transform.position).normalized;

            // Calculate the angle between the player's forward direction and the direction to the enemy
            float angle = Vector3.Angle(transform.forward, directionToTarget);

            // Check if the enemy is within the defined attack angle
            if (angle < attackAngle / 2)
            {
                // The enemy is within the arc, so register a hit!
                Debug.Log("Hit an enemy: " + hitCollider.name);

                // You can add damage logic here
                // e.g., hitCollider.GetComponent<EnemyHealth>().TakeDamage(10);
            }
        }
    }

    // Optional: Draw the attack arc in the editor for visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Draw the sphere to visualize the attack radius
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Calculate and draw the arc lines
        Vector3 forward = transform.forward * attackRadius;
        Vector3 leftArc = Quaternion.Euler(0, -attackAngle / 2, 0) * forward;
        Vector3 rightArc = Quaternion.Euler(0, attackAngle / 2, 0) * forward;

        Gizmos.DrawLine(transform.position, transform.position + leftArc);
        Gizmos.DrawLine(transform.position, transform.position + rightArc);
    }
}