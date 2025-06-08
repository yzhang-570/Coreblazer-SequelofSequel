using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
[System.Serializable]
public class SFXClipData
{
    public string clipName;
    public AudioClip clip;
    public float volume = 1f;
    public float pitchMin = 1f;
    public float pitchMax = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource1;
    //public AudioSource sfxSource2;

    [Header("BGM Clips")]
    public List<SFXClipData> bgmClips;

    [Header("SFX Clips")]
    public List<SFXClipData> sfxClips;

    private Dictionary<string, SFXClipData> bgmDict;
    private Dictionary<string, SFXClipData> sfxDict;

    public DialogueRunner dialogueRunner;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            BuildDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        dialogueRunner.AddCommandHandler<string>("play_bgm", PlayBGM);
    }
    private void BuildDictionaries()
    {
        bgmDict = new Dictionary<string, SFXClipData>();
        foreach (var item in bgmClips)
            bgmDict[item.clipName] = item;

        sfxDict = new Dictionary<string, SFXClipData>();
        foreach (var item in sfxClips)
            sfxDict[item.clipName] = item;
    }

    public void PlayBGM(string clipName)
    {
        if (clipName == "Silence")
        {
            bgmSource.Stop();
            bgmSource.clip = null;
            return;
        }

        if (!bgmDict.TryGetValue(clipName, out var data) || data.clip == null) return;

        bgmSource.clip = data.clip;
        bgmSource.volume = data.volume;
        bgmSource.loop = true;
        bgmSource.Play();
    }


    public void PlaySFX(string clipName)
    {
        if (!sfxDict.TryGetValue(clipName, out var data) || data.clip == null) return;

        float pitch = Random.Range(data.pitchMin, data.pitchMax);
        sfxSource1.pitch = pitch;
        sfxSource1.PlayOneShot(data.clip, data.volume);
    }
}
