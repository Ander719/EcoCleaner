// GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int puntuacion = 0;
    public int puntuacionEnemigos = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ResetearDatos()
    {
        puntuacion = 0;
        puntuacionEnemigos = 0;
    }
}
