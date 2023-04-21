using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingAudio : MonoBehaviour
{
    internal AudioSource audioSource;
    void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    virtual public void PlayAudio() {
        audioSource.Play();
    }

    virtual public void PauseAudio() {
        audioSource.Pause();
    }
}
