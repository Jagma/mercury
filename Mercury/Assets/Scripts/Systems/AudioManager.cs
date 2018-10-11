using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
            return;
        }

        AudioClip[] clips =  Resources.LoadAll<AudioClip>("Audio/");

        for (int i=0; i < clips.Length; i ++)
        {
            if (clips[i] != null) {
                audioClipDictionary.Add(clips[i].name, clips[i]);
            }           
        }
    }

    Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>();
    List<AudioSource> audioSourceList = new List<AudioSource>();
    float masterVolume = 1;
    
    public void PlayAudio(string name, float volume, bool loop)
    {
        if (audioClipDictionary.ContainsKey(name) == false) {
            Debug.LogError("Audio clip not found. Check name and ensure the clip is located in Resources/Audio");
            return;
        }

        AudioSource a = null;
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            if (audioSourceList[i].isPlaying == false)
            {
                a = audioSourceList[i];
            }
        }

        if (a == null)
        {
            a = gameObject.AddComponent<AudioSource>();
            audioSourceList.Add(a);
        }

        a.clip = audioClipDictionary[name];
        a.volume = volume * masterVolume;
        a.loop = loop;
        a.Play();
    }

    public void StopAudio(string name)
    {
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            if (audioSourceList[i].clip.name == name)
            {
                if (audioSourceList[i].isPlaying)
                {
                    audioSourceList[i].Stop();
                }
            }
        }
    }

    public void StopAllAudio()
    {
        for (int i = 0; i < audioSourceList.Count; i++)
        {
           if (audioSourceList[i].isPlaying)
           {
             audioSourceList[i].Stop();
           }
        }
    }

    public bool IsPlaying (string name)
    {
        for (int i=0; i < audioSourceList.Count; i ++)
        {
            if (audioSourceList[i].clip.name == name)
            {
                if (audioSourceList[i].isPlaying)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }
}