using UnityEngine;

public class MusicaInicio : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip musicaFondo;

    void Awake()
    {
        if (FindObjectsByType<MusicaInicio>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject); // Persiste entre escenas
    }

    void Start()
    {
        Source.clip = musicaFondo;
        Source.loop = true;
        Source.Play();

        // Aplicar estado guardado
        int musicaGuardada = PlayerPrefs.GetInt("MusicaActiva", 1);
        Source.mute = (musicaGuardada == 0);
    }

    // Método público para silenciar/activar música
    public void SetMute(bool activo)
    {
        Source.mute = !activo;
    }
}
