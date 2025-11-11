using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicaSource; // Música de fondo

    void Awake()
    {
        if (FindObjectsByType<AudioManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (musicaSource != null)
        {
            musicaSource.loop = true;
            musicaSource.Play();
        }

        int audioGuardado = PlayerPrefs.GetInt("AudioActivo", 1);
        SetAudio(audioGuardado == 1);
    }

    public void SetAudio(bool activo)
    {
        // Música
        if (musicaSource != null)
            musicaSource.mute = !activo;

        // Silenciar/activar efectos del jugador
        PlayerSound[] efectosJugador = FindObjectsByType<PlayerSound>(FindObjectsSortMode.None);
        foreach (PlayerSound ps in efectosJugador)
        {
            if (ps.CompareTag("Jugador"))
                ps.audioSource.mute = !activo;
        }

        PlayerPrefs.SetInt("AudioActivo", activo ? 1 : 0);
    }
    public bool audioDesactivado()
    {
        int estado = PlayerPrefs.GetInt("AudioActivo", 1);
        return estado == 0;
    }
}
