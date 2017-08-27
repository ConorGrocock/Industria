using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviourSingleton<MusicManager>
{
    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;

    public bool randomSelection = false;
    public float delayBetweenMusic = 0.0f;

    public List<AudioClip> musicList = new List<AudioClip>();

    private int previousPointer = 0;
    private int currentPointer = 0;

    private bool noMusic = false;
    private bool appPaused = false;

    private float delayTimer = 0.0f;

    private GameObject currentMusicContainer;
    private AudioSource currentMusic;

    void Start()
    {
        currentMusicContainer = new GameObject("Current Music");
        currentMusicContainer.transform.parent = transform;
        currentMusic = currentMusicContainer.AddComponent<AudioSource>();
        currentMusic.bypassEffects = true;
        currentMusic.bypassListenerEffects = true;
        currentMusic.bypassReverbZones = true;
        currentMusic.playOnAwake = false;

        if (musicList.Count == 0)
        {
            noMusic = true;
            Debug.LogError("[MusicManager] No music added to music list! Disabling.");
            gameObject.SetActive(false);
            return;
        }

        PlayMusic();
    }

    void OnApplicationPause(bool pause)
    {
        appPaused = pause;
    }

    void Update()
    {
        if (noMusic) return;

		if (!currentMusic.isPlaying && !appPaused)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= delayBetweenMusic)
            {
                delayTimer = 0.0f;
                NextMusic();
            }
        }
        else
        {
            if (volume < 0) volume = 0;
            if (volume > 1) volume = 1;

            currentMusic.volume = volume;
        }
	}

    void NextMusic()
    {
        if (noMusic) return;

        previousPointer = currentPointer;

        if (randomSelection)
        {
            while (previousPointer != currentPointer)
            {
                currentPointer = Random.Range(0, musicList.Count);

                if (musicList.Count == 1)
                {
                    break;
                }
            }
        }
        else
        {
            currentPointer++;
        }

        if (currentPointer + 1 > musicList.Count)
        {
            currentPointer = 0;
        }

        PlayMusic();
    }

    void PlayMusic()
    {
        if (noMusic) return;

        if (musicList[currentPointer] != null)
        {
            currentMusic.clip = musicList[currentPointer];
            currentMusic.Play();
        }
        else
        {
            Debug.LogError("[MusicManager] Music element " + currentPointer + " is null! Skipping.");
            NextMusic();
        }
    }

    public void AddMusic(AudioClip music)
    {
        musicList.Add(music);

        if (noMusic)
        {
            noMusic = false;
            gameObject.SetActive(true);
        }
    }
}
