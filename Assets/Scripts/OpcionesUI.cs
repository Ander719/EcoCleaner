using UnityEngine;
using UnityEngine.UI;

public class OpcionesUI : MonoBehaviour
{
    public Toggle toggleMusica;

    void Start()
    {
        // Cargar estado guardado
        int audioGuardado = PlayerPrefs.GetInt("AudioActivo", 1);
        toggleMusica.isOn = (audioGuardado == 1);

        // Conectar el evento
        toggleMusica.onValueChanged.AddListener(delegate { CambiarAudio(toggleMusica.isOn); });
    }

    void CambiarAudio(bool activo)
    {
        AudioManager audioManager = FindAnyObjectByType<AudioManager>();
        if (audioManager != null)
        {
            audioManager.SetAudio(activo);
        }
    }
}
