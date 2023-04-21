using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private GameObject audioPrefab, musicPrefab, untimedAudioPrefab, spatialAudioPrefab, loopingAudioPrefab;
    [SerializeField] private ArrayList audioSourceArray;
    private AudioClip walkingAudioClip, runningAudioClip;
    private GameObject walkingAudio, runningAudio;

    void Awake() {
        if(!Instance) {
            Instance = this;
            audioPrefab = Resources.Load<GameObject>("Prefabs/Audio");
            untimedAudioPrefab = Resources.Load<GameObject>("Prefabs/UntimedAudio");
            spatialAudioPrefab = Resources.Load<GameObject>("Prefabs/SpatialAudio");
            loopingAudioPrefab = Resources.Load<GameObject>("Prefabs/LoopingAudio");
            musicPrefab = Resources.Load<GameObject>("Prefabs/Music");
            walkingAudioClip = Resources.Load<AudioClip>("Audio/Looping/Walking");
            runningAudioClip = Resources.Load<AudioClip>("Audio/Looping/Running");
            DontDestroyOnLoad(this.gameObject);
            return;
        }

        if (Instance && Instance != this) {
            Instance.StartGame();
            Destroy(gameObject);
            return;
        }
    }

    void Start() {
        StartGame();
    }

    void StartGame() {
        PlayMusic();
    }

    public void PlayAudio(string audioString) {
        GameObject soundObject = Instantiate(audioPrefab);
        Audio sound = soundObject.GetComponent<Audio>();
        sound.PlayAudio(audioString);
    }

    public void PlaySpatialAudio(string audioString, Vector3 position) {
        GameObject soundObject = Instantiate(spatialAudioPrefab);
        Audio sound = soundObject.GetComponent<Audio>();
        soundObject.transform.position = position;
        sound.PlayAudio(audioString);
    }

        public void PlaySpatialAudio(string audioString, Vector3 position, float maxDistance) {
        GameObject soundObject = Instantiate(spatialAudioPrefab);
        Audio sound = soundObject.GetComponent<Audio>();
        soundObject.transform.position = position;
        sound.audioSource.maxDistance = maxDistance;
        sound.PlayAudio(audioString);
    }

    public void PlayUntimedAudio(string soundString) {
        GameObject soundObject = Instantiate(untimedAudioPrefab);
        UntimedAudio sound = soundObject.GetComponent<UntimedAudio>();
        AudioSource soundSource = soundObject.GetComponent<AudioSource>();
        soundSource.ignoreListenerPause = true;
        sound.PlayAudio(soundString);
    }

    public void PlayMusic() {
        GameObject audioObject = Instantiate(musicPrefab);
        Music audio = audioObject.GetComponent<Music>();
        audio.PlayAudio("BGM");
    }

    public void PlayLoopingAudio(GameObject audio) {
        LoopingAudio loopingAudio = audio.GetComponent<LoopingAudio>();
        loopingAudio.PlayAudio();
    }

    public void PauseLoopingAudio(GameObject audio) {
        LoopingAudio loopingAudio = audio.GetComponent<LoopingAudio>();
        loopingAudio.PauseAudio();
    } 
}
