using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
 * The code in this script is based on a publically available YouTube tutorial produced by user Brackeys (2017)
 * For the full reference to this source, along with others used for this Design Challenge 02, see Documentation\Design Challenge 02\Sources\Design Challenge 02 Reference List.pdf
 * */

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    class Sound
    {
        public string name = null;

        public AudioClip clip = null;

        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;
        [Range(0.1f, 3f)]
        public float pitch = 1.0f;

        public bool loop = false;
        public bool playOnAwake = false;

        [HideInInspector]
        public AudioSource source = null;
    }

    public static AudioManager instance;

    private List<string> m_currentlyPlayingSounds = new List<string>();

    [SerializeField]
    private Sound[] m_sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        AddSounds();
    }

    private void AddSounds()
    {
        for (int i = 0; i < m_sounds.Length; i++)
        {
            m_sounds[i].source = gameObject.AddComponent<AudioSource>();
            m_sounds[i].source.clip = m_sounds[i].clip;
            m_sounds[i].source.volume = m_sounds[i].volume;
            m_sounds[i].source.pitch = m_sounds[i].pitch;
            m_sounds[i].source.loop = m_sounds[i].loop;
            m_sounds[i].source.playOnAwake = m_sounds[i].playOnAwake;
        }
    }

    public void Play(string _name)
    {
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name == _name)
            {
                if (m_sounds[i].source != null && !m_sounds[i].source.isPlaying)
                {
                    m_sounds[i].source.Play();
                    Debug.Log("Playing audio: " + _name);
                    m_currentlyPlayingSounds.Add(_name);
                    Debug.Log("Audio added: " + _name);
                }
                else if (m_sounds[i].source.isPlaying)
                {
                    Debug.Log("Sound: " + _name + " is already playing");
                }
                else
                {
                    Debug.Log("Sound: " + _name + " was not found.");
                }
            }
        }
    }

    public void Stop(string _name)
    {
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name == _name)
            {
                if (m_sounds[i].source.isPlaying)
                {
                    m_sounds[i].source.Stop();
                    Debug.Log("Audio Stopped: " + _name);
                    if (m_currentlyPlayingSounds.Contains(_name))
                    {
                        m_currentlyPlayingSounds.Remove(_name);
                        Debug.Log("Audio Removed: " + _name);
                    }
                }
                else
                {
                    Debug.Log("Sound: " + _name + " is not playing");
                }
            }
        }
    }

    public void Pause(string _name)
    {
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].name == _name)
            {
                if (m_sounds[i].source.isPlaying)
                {
                    m_sounds[i].source.Pause();
                    Debug.Log("Sound: " + _name + " paused");
                }
            }
        }
    }

    public void StopAll()
    {
        for (int i = 0; i < m_sounds.Length; i++)
        {
            if (m_sounds[i].source.isPlaying)
            {
                m_sounds[i].source.Stop();
                m_currentlyPlayingSounds.Clear();
            }
        }

        Debug.Log("All audio stopped");
    }
}