using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 1f;

    public enum WeaponType { Melee, Bullet}
    public WeaponType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            BossMovement enemy = collision.GetComponent<BossMovement>();
            if (enemy != null)
            {
                if (type == WeaponType.Bullet)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
