using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
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
        DontDestroyOnLoad(gameObject);
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

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource.clip == musicClip) return;

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