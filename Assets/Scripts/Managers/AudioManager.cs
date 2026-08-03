using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public const string MIXER_MASTER = "MasterVolume";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    [Header("Mixer")]
    [Tooltip("The main AudioMixer of the project")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [System.Serializable]
    public struct SoundEffect
    {
        public string soundName;
        public AudioClip clip;
    }

    [Header("Audio Library")]
    [Tooltip("List of sound effects with their string names")]
    [SerializeField] private List<SoundEffect> sfxLibrary;

    [Tooltip("Background music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip ambientSound;

    private void Awake()
    {
        ServiceLocator.Register<AudioManager>(this);
    }

    private void Start()
    {
        GameSettingsManager settings = ServiceLocator.Get<GameSettingsManager>();

        SetVolume(MIXER_MASTER, settings.MasterVolume);
        SetVolume(MIXER_MUSIC, settings.MusicVolume);
        SetVolume(MIXER_SFX, settings.SFXVolume);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<AudioManager>();
    }

    public void PlaySFX(string soundName)
    {
        AudioClip clipToPlay = null;

        foreach (var sfx in sfxLibrary)
        {
            if (sfx.soundName == soundName)
            {
                clipToPlay = sfx.clip;
                break;
            }
        }

        if (clipToPlay != null)
        {
            sfxSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.LogWarning($"[AudioManager] SFX '{soundName}' was not found.");
        }
    }

    public void PlaySFXOnce(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource.clip == musicClip) return;

        musicSource.Stop();

        musicSource.clip = musicClip;
        musicSource.Play();
    }

    public void SetVolume(string volumeName, float value)
    {
        float dB = (value > Mathf.Epsilon) ? Mathf.Log10(value) * 20f : -144.0f;
        mainMixer.SetFloat(volumeName, dB);
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayGameplayMusic()
    {
        PlayMusic(gameplayMusic);
    }

    public void PlayAmbientSounds()
    {
        PlayMusic(ambientSound);
    }
}