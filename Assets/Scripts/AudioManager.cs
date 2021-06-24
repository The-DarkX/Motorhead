using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] Sound[] sounds;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("_Sound" + i + "_" + sounds[i].name);
            _go.AddComponent<AudioSource>();
            sounds[i].SetSource(_go.GetComponent<AudioSource>());
            _go.transform.SetParent(transform);
        }
    }

	public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Play();
                return;
            }
        }

        //no sound with name
        Debug.LogWarning("AudioManager: Sound not found in list, " + name);
    }

    public void StopSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Stop();
                return;
            }
        }

        //no sound with name
        Debug.LogWarning("AudioManager: Sound not found in list, " + name);
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 0.7f;
    [Range(0.5f, 1.5f)] public float pitch = 1f;

    [Range(0, 0.5f)] public float randomVolume = 0.1f;
    [Range(0, 0.5f)] public float randomPitch = 0.1f;

    public bool loop = false;

    private AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));

        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}