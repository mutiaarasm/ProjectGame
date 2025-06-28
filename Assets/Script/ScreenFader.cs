using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class ScreenFader : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 2f;
    public string nextScene = "MainMenu";

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1f - (t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;

        yield return new WaitForSeconds(1f); // jeda sebentar

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = t / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        SceneManager.LoadScene(nextScene);
    }
}
