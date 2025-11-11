using UnityEngine;

public class BossMeleeHitBox : MonoBehaviour
{
    public int damage = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            VidaJugador vida = other.GetComponent<VidaJugador>();
            if (vida != null)
                vida.RecibirDanio(damage);
        }
    }
}
