using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip clickSound;
    public void StartGame()
    {
        PermanentAudio.instance.PlaySFX(clickSound);
        SceneManager.LoadSceneAsync(1);
    }


    public void QuitGame()
    {
        PermanentAudio.instance.PlaySFX(clickSound);
        Application.Quit();
    }


    public void OpenMenu()
    {
        PermanentAudio.instance.PlaySFX(clickSound);
        SceneManager.LoadSceneAsync(0);
    }


    public void OpenCredits()
    {
        PermanentAudio.instance.PlaySFX(clickSound);
        SceneManager.LoadSceneAsync(4);
    }
}
