using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRadius = 1f;
    public float attackAngle = 100f;
    public LayerMask enemyLayer;

    [Tooltip("Assign the rotating cube (the part that faces the mouse)")]
    public Transform facingTransform;

    [Header("Attack Cooldown")]
    public float attackCooldown = 0.5f; // Seconds between attacks
    private float lastAttackTime = -Mathf.Infinity;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    void PerformAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);

        Vector3 attackForward = facingTransform.forward;
        attackForward.y = 0;
        attackForward.Normalize();

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToTarget = hitCollider.transform.position - transform.position;
            directionToTarget.y = 0;
            directionToTarget.Normalize();

            float angle = Vector3.Angle(attackForward, directionToTarget);

            if (angle < attackAngle / 2f)
            {
                Debug.Log("Hit an enemy: " + hitCollider.name);

                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(10f, transform.position);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (facingTransform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Vector3 forward = facingTransform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 leftArc = Quaternion.Euler(0, -attackAngle / 2f, 0) * forward * attackRadius;
        Vector3 rightArc = Quaternion.Euler(0, attackAngle / 2f, 0) * forward * attackRadius;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftArc);
        Gizmos.DrawLine(transform.position, transform.position + rightArc);
    }
}
