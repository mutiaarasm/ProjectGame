using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void paused()
    {
        Time.timeScale = 0;
    }

    public void resume()
    {
        Time.timeScale = 1;
    }

    // Fungsi untuk Restart Level saat ini
    public void RestartScene()
    {
        Time.timeScale = 1; // Pastikan game tidak dalam keadaan pause
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Fungsi untuk kembali ke Main Menu
    public void ExitToMainMenu()
    {
        Time.timeScale = 1; // Pastikan game tidak dalam keadaan pause
        SceneManager.LoadScene("MainMenu"); // Ganti jika nama scene MainMenu berbeda
    }
}
