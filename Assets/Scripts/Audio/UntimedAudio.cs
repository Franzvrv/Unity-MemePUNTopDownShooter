using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UntimedAudio : MonoBehaviour
{
    internal AudioClip audioClip;
    internal AudioSource audioSource;
    void Awake() {
        audioClip = GetComponent<AudioClip>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {

    }

    virtual public void PlayAudio(string _audio) {
        audioClip = Resources.Load<AudioClip>("Audio/" + _audio);
        audioSource.clip = audioClip;
        StartCoroutine(soundCoroutine());
    }

    IEnumerator soundCoroutine() {
        while (true) {
            switch (audioClip.loadState) {
                case AudioDataLoadState.Loaded:
                    audioSource.clip = audioClip;
                    audioSource.Play();
                    yield return new WaitForSecondsRealtime(audioClip.length);
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
