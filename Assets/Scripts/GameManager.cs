using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int puntuacion = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ResetearDatos()
    {
        puntuacion = 0; // La puntuación vuelve a cero
        Debug.Log("Puntuación restablecida a cero.");
        // Si tuvieras más variables persistentes (vidas, nivel, etc.), también las pondrías aquí
    }
}
