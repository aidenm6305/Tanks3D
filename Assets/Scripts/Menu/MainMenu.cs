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

    private void Start()
    {
        if (mainMenuUI != null)
            mainMenuUI.SetActive(true);
        if (optionsMenuUI != null)
            optionsMenuUI.SetActive(false);
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
        float clampedValue = Mathf.Clamp(volumeSlider.value, 0.0001f, 1f);
        
        float volumeInDecibels = Mathf.Log10(clampedValue) * 20f;
        
        audioMixer.SetFloat("Volume", volumeInDecibels);
    }
}
