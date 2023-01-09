using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] sounds;
    void Start()
    {
        if(Instance != null)
        {
            Debug.Log("Tried to create a duplicate AudioManager");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            
            foreach(Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.loop = s.loop;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
            }
        }

    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, s => s.name.Equals(name));
        if(s == null)
        {
            Debug.Log("Sound " + name + " could not be played");
            return;
        }
        if (!s.source.loop) s.source.PlayOneShot(s.clip);
        else s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, s => s.name.Equals(name));
        if (s == null)
        {
            Debug.Log("Sound " + name + " could not be stopped");
            return;
        }
        s.source.Stop();
    }


}

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public bool loop;

    [Range(0, 1)]
    public float volume = 1;

    public float pitch = 1;

    public AudioSource source;
}
