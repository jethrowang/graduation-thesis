using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class menu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioMixer audioMixer;
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale=0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale=1f;
    }

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("mainVolume",value);
    }

    public void Setting()
    {

    }

    public void Knowledge()
    {

    }

    public void Skip()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}