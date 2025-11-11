using UnityEngine;

[RequireComponent(typeof(BossController))]
public class BossHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    private Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        animator.SetBool("IsDead", true);
        GetComponent<BossController>().enabled = false;
        GetComponent<BossAttack>().enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
}
