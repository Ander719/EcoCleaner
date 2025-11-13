using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicaSource; // AudioSource que reproduce la música
    public AudioClip musicaMenu;     // Música de Menu/Opciones/Créditos
    public AudioClip musicaNivel1;   // Música de Nivel1
    // Puedes agregar más clips para otros niveles si quieres

    void Awake()
    {
        // Evita duplicados
        if (FindObjectsByType<AudioManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // Reproducir música del menú al inicio
        if (musicaSource != null && !musicaSource.isPlaying)
        {
            musicaSource.clip = musicaMenu;
            musicaSource.loop = true;
            musicaSource.Play();
        }

        int audioGuardado = PlayerPrefs.GetInt("AudioActivo", 1);
        SetAudio(audioGuardado == 1);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (musicaSource == null) return;

        // Cambiar música según la escena
        switch (scene.name)
        {
            case "Menu":
            case "Opciones":
            case "Creditos":
                if (musicaSource.clip != musicaMenu)
                {
                    musicaSource.clip = musicaMenu;
                    musicaSource.loop = true;
                    musicaSource.Play();
                }
                break;

            case "Nivel1":
                if (musicaSource.clip != musicaNivel1)
                {
                    musicaSource.clip = musicaNivel1;
                    musicaSource.loop = true;
                    musicaSource.Play();
                }
                break;

                // Aquí puedes añadir más niveles con más casos
        }
    }

    public void SetAudio(bool activo)
    {
        if (musicaSource != null)
            musicaSource.mute = !activo;

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
