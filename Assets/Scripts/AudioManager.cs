using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Array to store all needed sounds
    public Sound[] sounds;

    public static AudioManager instance;

    private bool golemSpawned = false;

    // Start is called before the first frame update
    void Awake()
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

        // Create an audio source and set vol and pitch for each sound on the AudioManager
        foreach (Sound curSound in sounds)
        {
            curSound.source = gameObject.AddComponent<AudioSource>();

            curSound.source.clip = curSound.clip;
            curSound.source.volume = curSound.volume;
            curSound.source.pitch = curSound.pitch;
            curSound.source.loop = curSound.loop;
        }
    }

    void Start()
    {
        Play("ForestBG");
    }

    private void Update()
    {
        // Switch music once golem arrives
        if (TreeTracker.treesCut >= 4 && !golemSpawned)
        {
            golemSpawned = true; // Prevent restarting music
            StopPlaying("ForestBG");
            Play("GolemMusic");
        }
    }

    // To play a sound, add this line where needed
    // FindObjectOfType<AudioManager>().Play("soundName");
    public void Play(string soundName)
    {
        Sound selSound = Array.Find(sounds, sound => sound.name == soundName);

        if (selSound == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found.");
            return;
        }

        selSound.source.Play();
    }

    // To stop sound, add this line where needed
    // FindObjectOfType<AudioManager>().StopPlaying("soundName");
    public void StopPlaying(string soundName)
    {
        Sound selSound = Array.Find(sounds, sound => sound.name == soundName);

        if (selSound == null)
        {
            Debug.LogWarning("Sound: " + soundName + " not found.");
            return;
        }

        selSound.source.Stop();
    }
}
