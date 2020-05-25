using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup audioMixer;
    public Sonido[] sonidos;

    public static AudioManager instance;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sonido s in sonidos)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = audioMixer;
        }
    }

    public void Stop(string nombre)
    {
        Sonido s = Array.Find(sonidos, sonido => sonido.nombre == nombre);
        if (s == null)
        {
            Debug.LogWarning("¡Sonido: " + nombre + "no encontrado!");
            return;
        }
        Debug.Log("Parando: " + nombre);
        s.source.Stop();
    }

    public void Play(string nombre)
    {
        Sonido s = Array.Find(sonidos, sonido => sonido.nombre == nombre);
        if (s == null)
        {
            Debug.LogWarning("¡Sonido: " + nombre + "no encontrado!");
            return;
        }
        Debug.Log("Reproduciendo: " + nombre);
        s.source.Play();        
    }
}
