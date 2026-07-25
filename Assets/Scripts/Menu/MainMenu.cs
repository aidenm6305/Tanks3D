using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject optionsMenuUI;

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
}
