using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour
{
    [Header("Ataque")]
    public float attackCooldown = 0.5f;
    public float attackDelay = 0.2f; // tiempo hasta que se activa el golpe
    public Transform hitBox;
    public Animator animator;

    private BossController controller;
    private bool isAttacking = false;
    private Transform playerTarget;

    public bool IsAttacking => isAttacking;

    private void Start()
    {
        controller = GetComponent<BossController>();
        animator = GetComponent<Animator>();
        if (hitBox != null)
            hitBox.gameObject.SetActive(false);
    }

    public void StartAttackCycle(Transform target)
    {
        if (!isAttacking)
            StartCoroutine(AttackCoroutine(target));
    }

    private IEnumerator AttackCoroutine(Transform target)
    {
        isAttacking = true;
        playerTarget = target;
        controller.SetCanMove(false);

        animator.SetBool("IsAttacking", true);

        yield return new WaitForSeconds(attackDelay);
        if (hitBox != null) hitBox.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f); // duración activa del golpe
        if (hitBox != null) hitBox.gameObject.SetActive(false);

        animator.SetBool("IsAttacking", false);

        yield return new WaitForSeconds(attackCooldown);

        // Si el jugador sigue en rango, volver a atacar
        float distance = Vector2.Distance(transform.position, playerTarget.position);
        if (distance < controller.detectionRange)
            StartCoroutine(AttackCoroutine(playerTarget));
        else
            controller.SetCanMove(true);

        isAttacking = false;
    }
}
