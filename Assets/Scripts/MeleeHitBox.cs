using UnityEngine;

public class MeleeHitBox : MonoBehaviour
{
    public int damage = 2;
    public float attackCooldown = 0.5f; // tiempo entre golpes

    private Boss boss;
    private float lastAttackTime = 0f;

    private void Start()
    {
        boss = GetComponentInParent<Boss>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Jugador")) return;

        boss.animator.SetBool("IsAttacking", true);
        TryAttack(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Jugador")) return;

        boss.animator.SetBool("IsAttacking", true);
        TryAttack(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Jugador")) return;

        boss.animator.SetBool("IsAttacking", false);
    }

    private void TryAttack(Collider2D other)
    {
        // Comprobamos si ha pasado suficiente tiempo desde el último ataque
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            VidaJugador vidaJugador = other.GetComponent<VidaJugador>();
            if (vidaJugador != null)
            {
                vidaJugador.RecibirDanio(damage);
            }

            // Reiniciamos el temporizador
            lastAttackTime = Time.time;
        }
    }
}
