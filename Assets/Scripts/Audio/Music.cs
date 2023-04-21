using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : Audio
{
    void Awake()
    {
        audioClip = GetComponent<AudioClip>();
        audioSource = GetComponent<AudioSource>();
    }

    override public void PlayAudio(string _audio) {
        audioClip = Resources.Load<AudioClip>("Audio/Looping/" + _audio);
        audioClip.LoadAudioData();
        audioSource.clip = audioClip;
        StartCoroutine(musicCoroutine());
    }

    IEnumerator musicCoroutine() {
        while (true) {
            switch (audioClip.loadState) {
                case AudioDataLoadState.Loaded:
                    audioSource.clip = audioClip;
                    audioSource.Play();
                    yield return new WaitForSeconds(audioClip.length);
                    AudioManager.Instance.PlayMusic();
                    Destroy(this.gameObject);
                    yield break;
                case AudioDataLoadState.Failed:
                case AudioDataLoadState.Unloaded:
                    yield break;
                default:
                    yield return new WaitForEndOfFrame();
                    break;
            }
        }
    }
}
