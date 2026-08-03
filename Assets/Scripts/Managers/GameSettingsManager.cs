using UnityEngine;

public class GameSettingsManager
{
    private const string MasterVolumeKey = "Master";
    private const string MusicVolumeKey = "Music";
    private const string SFXVolumeKey = "SFX";

    public float MasterVolume => PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
    public float MusicVolume => PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
    public float SFXVolume => PlayerPrefs.GetFloat(SFXVolumeKey, 1f);

    public void SetMasterVolume(float value)
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat(SFXVolumeKey, value);
        PlayerPrefs.Save();
    }
    public void SaveAll()
    {
        PlayerPrefs.Save();
    }
}