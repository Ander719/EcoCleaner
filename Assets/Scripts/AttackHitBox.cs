using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {
            Debug.Log("Golpe al enemigo: " + collision.name); // Para probar en consola
            Destroy(collision.gameObject);
        }
    }
}
