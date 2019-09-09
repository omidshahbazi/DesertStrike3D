using RamboTeam.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviorBase
{
    public GameObject audioItem;
    private static AudioManager instance;

    private List<AudioSource> audioSources = new List<AudioSource>();

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("AudioManager");
                DontDestroyOnLoad(gameObject);
                instance = gameObject.AddComponent<AudioManager>();
            }

            return instance;

        }
    }

    protected override void Awake()
    {
        base.Awake();

        instance = this;
    }

    public AudioSource PlayAudio(AudioClip AudioClip, Vector3 Pos, Transform Parent = null)
    {
        return PlayAudio(AudioClip, Pos, Parent, 0.0F, 1.0F, false);
    }

    public AudioSource PlayAudio(AudioClip AudioClip, Vector3 Pos, Transform Parent = null, float Delay = 0.0F, float Volume = 1, bool IsLopp = false)
    {
        if (AudioClip == null)
            return null;

        AudioSource audio = IsAlreadyExist(AudioClip, Parent, Volume, IsLopp);

        if (audio != null)
        {
            audio.Play();
            return audio;
        }

        Transform mainParent = this.transform;
        if (Parent != null)
            mainParent = Parent;

        if (audioItem == null)
        {
            Debug.Log("NULLLLLLLLLLL");
            return null;
        }
        GameObject item;
        if (audioItem == null)
            item = GameObject.Instantiate(Resources.Load("AudioManagerItem")) as GameObject;
        else
        {
            item = GameObject.Instantiate(audioItem, Pos, Quaternion.identity) as GameObject;
        }

        item.name = AudioClip.name;
        item.transform.parent = mainParent;
        item.transform.position = Pos;
        AudioSource audioComponent = item.GetComponent<AudioSource>();
        audioComponent.clip = AudioClip;
        audioComponent.volume = Volume;
        audioComponent.loop = IsLopp;
        audioComponent.playOnAwake = false;
        audioComponent.PlayDelayed(Delay);
        audioSources.Add(audioComponent);

        return audioComponent;
    }

    private AudioSource IsAlreadyExist(AudioClip audioClip, Transform parent, float volume, bool isLopp)
    {
        for (int i = 0; i < audioSources.Count; ++i)
        {
            AudioSource curr = audioSources[i];
            if (curr.clip == audioClip && !curr.isPlaying && (parent == null ? curr.transform.parent == this.transform : curr.transform.parent == parent) && curr.volume == volume && curr.loop == isLopp)
            {
                return curr;
            }

        }
        return null;
    }
}
