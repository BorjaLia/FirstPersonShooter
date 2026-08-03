using UnityEngine;

public class GameSettingsManager
{
    public float MasterVolume => PlayerPrefs.GetFloat(AudioManager.MIXER_MASTER, 1f);
    public float MusicVolume => PlayerPrefs.GetFloat(AudioManager.MIXER_MUSIC, 1f);
    public float SFXVolume => PlayerPrefs.GetFloat(AudioManager.MIXER_SFX, 1f);
    public void SetMasterVolume(float value)
    {
        PlayerPrefs.SetFloat(AudioManager.MIXER_MASTER, value);
        ServiceLocator.Get<AudioManager>()?.SetVolume(AudioManager.MIXER_MASTER, value);
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(AudioManager.MIXER_MUSIC, value);
        ServiceLocator.Get<AudioManager>()?.SetVolume(AudioManager.MIXER_MUSIC, value);
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat(AudioManager.MIXER_SFX, value);
        ServiceLocator.Get<AudioManager>()?.SetVolume(AudioManager.MIXER_SFX, value);
    }

    public void SaveAll()
    {
        PlayerPrefs.Save();
    }
}