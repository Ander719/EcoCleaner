using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 1f;
    public enum WeaponType { Melee, Bullet }
    public WeaponType type;
    public string targetTag = "Jugador"; // a quién puede dañar (Jugador o Enemigo)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(targetTag)) return;

        // Si el objetivo tiene vida
        var vida = collision.GetComponent<IVida>();
        if (vida != null)
        {
            vida.TakeDamage(damage);
        }

        // Si es proyectil, lo destruimos tras impactar
        if (type == WeaponType.Bullet)
        {
            Destroy(gameObject);
        }
    }
}
