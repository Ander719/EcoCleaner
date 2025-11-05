using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private BossMovement bossMovement;
    private bool playerInRange = false;
    private GameObject currentPlayer;

    [SerializeField] private float reattackDelay = 0.5f; // tiempo de espera tras un ataque
    private Coroutine attackLoop;

    private void Start()
    {
        bossMovement = GetComponentInParent<BossMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            playerInRange = true;
            currentPlayer = other.gameObject;

            // Si no hay un loop de ataque activo, lo iniciamos
            attackLoop ??= StartCoroutine(AttackLoop());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            playerInRange = false;
            currentPlayer = null;

            // Detenemos el loop si ya no hay jugador
            if (attackLoop != null)
            {
                StopCoroutine(attackLoop);
                attackLoop = null;
            }
        }
    }

    private IEnumerator AttackLoop()
    {
        // Bucle mientras el jugador siga dentro del área
        while (playerInRange)
        {
            // Esperamos a que el boss esté libre para atacar
            if (!bossMovement.IsAttacking)
            {
                bossMovement.StartAttack(currentPlayer);

                // Esperamos a que acabe el ataque
                yield return new WaitUntil(() => !bossMovement.IsAttacking);

                // Espera el tiempo de recuperación (0.5s o lo que pongas)
                yield return new WaitForSeconds(reattackDelay);
            }

            // Pequeña pausa por frame para evitar bucle infinito instantáneo
            yield return null;
        }

        attackLoop = null;
    }
}
