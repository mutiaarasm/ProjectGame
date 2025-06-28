using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenLoader : MonoBehaviour
{
    [SerializeField] private float delay = 3f; // Durasi splash (detik)
    [SerializeField] private string nextScene = "MainMenu"; // Scene berikutnya

    private void Start()
    {
        Invoke(nameof(LoadNextScene), delay);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
