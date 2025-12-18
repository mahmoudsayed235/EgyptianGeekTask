using Unity.Burst.Intrinsics;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField]
    AudioClip flip, match, mismatch,gameOver,combo;

    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Play(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Flip:
                audioSource.clip = flip;
                break;

            case SoundType.Match:
                audioSource.clip = match;
                break;

            case SoundType.Mismatch:
                audioSource.clip = mismatch;
                break;

            case SoundType.Win:
                audioSource.clip = gameOver;
                break;
            case SoundType.Combo:
                audioSource.clip = combo;
                break;
        }
        audioSource.Play();
    }
}
public enum SoundType
{
    Flip,
    Match,
    Mismatch,
    Win,
    Combo
}