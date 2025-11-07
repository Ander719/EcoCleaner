using UnityEngine;
using UnityEngine.Audio;

public class MusicaInicio : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip musicaFondo;

    void Start()
    {
        Source.clip = musicaFondo;   
        Source.loop = true;          
        Source.Play();               
    }

}
