using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsMenuUI;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer audioMixer;

    private const string VolumePrefKey = "VolumePreference";

    private void Start()
    {
        if (mainMenuUI != null)
            mainMenuUI.SetActive(true);
        if (optionsMenuUI != null)
            optionsMenuUI.SetActive(false);

        if (PlayerPrefs.HasKey(VolumePrefKey))
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey);
            volumeSlider.value = savedVolume;
        }

        ApplyVolume(volumeSlider.value);
    }

    public void Play(string sceneName) 
    {         
        SceneManager.LoadScene(sceneName);
    }

    public void Quit() 
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    public void VolumeSlider()
    {
        ApplyVolume(volumeSlider.value);

        PlayerPrefs.SetFloat(VolumePrefKey, volumeSlider.value);
        PlayerPrefs.Save();
    }

    private void ApplyVolume(float sliderValue)
    {
        float clampedValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        
        float volumeInDecibels = Mathf.Log10(clampedValue) * 20f;
        
        audioMixer.SetFloat("Volume", volumeInDecibels);
    }
}
