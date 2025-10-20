using UnityEngine;

public class PruebaUIHealth : MonoBehaviour
{
    public UIHealth uiHealth;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            uiHealth.RecibirDaño(1); // Reduce 1 corazón
        }
    }
}
