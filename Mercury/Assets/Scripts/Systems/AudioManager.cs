using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        for (int i=0; i < audioClips.Length; i ++) {
            audioClipDictionary.Add(audioClips[i].name, audioClips[i]);
        }
    }

    public AudioClip[] audioClips;
    Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>();
    List<AudioSource> audioSourceList = new List<AudioSource>();
    float masterVolume = 1;

    AudioSource a;
    public void PlayAudio(string name, float volume, bool loop) {
        a = null;
        for (int i = 0; i < audioSourceList.Count; i++) {
            if (audioSourceList[i].isPlaying == false) {
                a = audioSourceList[i];
            }
        }

        if (a == null) {
            a = gameObject.AddComponent<AudioSource>();
            audioSourceList.Add(a);
        }

        a.clip = audioClipDictionary[name];
        a.volume = volume * masterVolume;
        a.loop = loop;
        a.Play();
    }

    public void StopAudio(string name) {
        for (int i = 0; i < audioSourceList.Count; i++) {
            if (audioSourceList[i].clip.name == name) {
                if (audioSourceList[i].isPlaying) {
                    audioSourceList[i].Stop();
                }
            }
        }
    }

    public bool IsPlaying (string name) {
        for (int i=0; i < audioSourceList.Count; i ++) {
            if (audioSourceList[i].clip.name == name) {
                if (audioSourceList[i].isPlaying) {
                    return true;
                } else {
                    return false;
                }
            }
            
        }
        return false;
    }
}