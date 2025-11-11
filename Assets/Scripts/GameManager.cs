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
}
