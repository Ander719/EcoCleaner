using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BossController))]
public class BossHealth : MonoBehaviour
{
    public int maxHealth = 10;

    private int currentHealth;
    private bool isDead = false;
    private bool isTakingDamage = false;

    private Animator animator;
    private BossController bossController;
    private BossAttack bossAttack;
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        bossController = GetComponent<BossController>();
        bossAttack = GetComponent<BossAttack>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage(int amount)
    {
        if (isDead || isTakingDamage) return; // evita recibir daño múltiple mientras se anima

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            StartCoroutine(HurtRoutine());
        }
        else
        {
            StartCoroutine(DieRoutine());
        }
    }
    private IEnumerator HurtRoutine()
    {
        isTakingDamage = true;
        animator.SetBool("GetHurt", true);

        // Detiene el movimiento y ataques mientras se reproduce la animación
        bossController.enabled = false;
        bossAttack.enabled = false;
        rb.linearVelocity = Vector2.zero;

        // Esperar hasta que termine la animación "GetHurt"
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetBool("GetHurt", false);
        isTakingDamage = false;

        // Reactiva el movimiento y ataques si sigue vivo
        bossController.enabled = true;
        bossAttack.enabled = true;
    }

    private IEnumerator DieRoutine()
    {
        isDead = true;
        animator.SetBool("IsDead", true);

        // Desactiva todo movimiento y ataques
        bossController.enabled = false;
        bossAttack.enabled = false;
        rb.linearVelocity = Vector2.zero;

        // Esperar la duración de la animación de muerte
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Aquí puedes hacer que desaparezca o destruirlo
        Destroy(gameObject);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Victoria");
    }
}
