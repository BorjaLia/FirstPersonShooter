using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [Header("Audio Sliders")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private GameObject previousPanel;


    private GameSettingsManager _settingsManager;

    private void OnEnable()
    {
        _settingsManager = ServiceLocator.Get<GameSettingsManager>();

        if (masterVolumeSlider) masterVolumeSlider.value = _settingsManager.MasterVolume;
        if (musicVolumeSlider) musicVolumeSlider.value = _settingsManager.MusicVolume;
        if (sfxVolumeSlider) sfxVolumeSlider.value = _settingsManager.SFXVolume;

        if (masterVolumeSlider) masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        if (musicVolumeSlider) musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        if (sfxVolumeSlider) sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnDisable()
    {
        if (masterVolumeSlider) masterVolumeSlider.onValueChanged.RemoveAllListeners();
        if (musicVolumeSlider) musicVolumeSlider.onValueChanged.RemoveAllListeners();
        if (sfxVolumeSlider) sfxVolumeSlider.onValueChanged.RemoveAllListeners();

        _settingsManager?.SaveAll();
    }

    private void OnMasterVolumeChanged(float value) => _settingsManager.SetMasterVolume(value);
    private void OnMusicVolumeChanged(float value) => _settingsManager.SetMusicVolume(value);
    private void OnSFXVolumeChanged(float value) => _settingsManager.SetSFXVolume(value);

    public void ExitSettings()
    {
        gameObject.SetActive(false);

        if (previousPanel != null)
        {
            previousPanel.SetActive(true);
        }
    }
}