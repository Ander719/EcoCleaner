using UnityEngine;
using System.Collections;

public class BossAttack : MonoBehaviour
{
    public float attackDelay = 0.5f;    // tiempo de animación antes de golpear
    public float attackCooldown = 0.5f; // tiempo entre ataques
    public int damage = 2;              // daño del boss

    private Animator animator;
    private BossController controller;
    private bool isAttacking = false;
    private Transform playerTarget;

    public bool IsAttacking => isAttacking;

    private void Start()
    {
        controller = GetComponent<BossController>();
        animator = GetComponent<Animator>();
    }

    public void PlayerEnRango(Transform player)
    {
        playerTarget = player;

        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackLoop());
        }
    }

    public void PlayerSalioRango()
    {
        playerTarget = null;
    }

    private IEnumerator AttackLoop()
    {
        controller.SetCanMove(false);

        while (playerTarget != null)
        {
            // Iniciar animación de ataque
            animator.SetBool("IsAttacking", true);

            // Esperar la duración de la animación antes de aplicar daño
            yield return new WaitForSeconds(attackDelay);

            // Aplicar daño al jugador si sigue en rango
            if (playerTarget != null)
            {
                VidaJugador vida = playerTarget.GetComponent<VidaJugador>();
                if (vida != null)
                    vida.RecibirDanio(damage);
            }

            // Terminar animación
            animator.SetBool("IsAttacking", false);

            // Esperar cooldown antes del siguiente ataque
            yield return new WaitForSeconds(attackCooldown);
        }

        controller.SetCanMove(true);
        isAttacking = false;
    }
}
