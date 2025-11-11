using UnityEngine;

public class BossMeleeHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            // Avisar al BossAttack que hay jugador en rango
            BossAttack bossAttack = GetComponentInParent<BossAttack>();
            if (bossAttack != null)
                bossAttack.PlayerEnRango(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            BossAttack bossAttack = GetComponentInParent<BossAttack>();
            if (bossAttack != null)
                bossAttack.PlayerSalioRango();
        }
    }
}
