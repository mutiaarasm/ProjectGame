using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class KomponenManager : MonoBehaviour
{
    public static KomponenManager instance;

    private int komponen;
    private int targetKomponen;

    [SerializeField] private TMP_Text komponenDisplay;
    [SerializeField] private TMP_Text timerDisplay;
    [SerializeField] private TMP_Text winText;

    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject retryButton;

    [Header("Star Displays")]
    [SerializeField] private GameObject[] gameplayStars;
    [SerializeField] private GameObject[] winStars;

    [Header("Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Timer")]
    [SerializeField] private float timeLimit = 120f;
    private float timer;
    private bool isGameEnded = false;

    private int currentStarRating = 0;
    private int[] starThresholds = { 10, 30, 50 };

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        winPanel?.SetActive(false);
        gameOverPanel?.SetActive(false);

        ResetStars(gameplayStars);
        ResetStars(winStars);
    }

    private void Start()
{
    Time.timeScale = 1;
    timer = timeLimit;
    UpdateKomponenDisplay();

    string currentLevel = SceneManager.GetActiveScene().name;

    switch (currentLevel)
    {
        case "stone":
            targetKomponen = 70; // Target kemenangan
            starThresholds = new int[] { 20, 40, 50 }; // Bintang 1,2,3
            break;

        case "Level2":
            targetKomponen = 100;
            starThresholds = new int[] { 25, 45, 75 };
            break;

        case "Level3":
            targetKomponen = 105;
            starThresholds = new int[] { 30, 45, 80 };
            break;

        default:
            targetKomponen = 50;
            starThresholds = new int[] { 10, 30, 50 };
            break;
    }
}

    private void Update()
    {
        if (isGameEnded) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
            EndGame();
        }

        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        if (timerDisplay != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerDisplay.text = $"{minutes:00}:{seconds:00}";
        }
    }

    private void UpdateKomponenDisplay()
    {
        if (komponenDisplay != null)
            komponenDisplay.text = komponen.ToString();
    }

    public void ChangeKomponen(int amount)
    {
        komponen += amount;
        UpdateKomponenDisplay();
        UpdateStarRating();
        UpdateStarDisplay(gameplayStars);
        SaveProgress(SceneManager.GetActiveScene().name); 
    }

    private void UpdateStarRating()
    {
        int newRating = 0;
        for (int i = 0; i < starThresholds.Length; i++)
        {
            if (komponen >= starThresholds[i])
                newRating = i + 1;
        }
        currentStarRating = newRating;
    }

    private void UpdateStarDisplay(GameObject[] stars)
    {
        if (stars == null) return;

        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] != null)
                stars[i].SetActive(i < currentStarRating);
        }
    }

    private void ResetStars(GameObject[] stars)
    {
        if (stars == null) return;

        foreach (var star in stars)
        {
            if (star != null)
                star.SetActive(false);
        }
    }

    private void EndGame()
    {
        isGameEnded = true;
        UpdateStarRating();
        ResetStars(gameplayStars);
        ResetStars(winStars);
        UpdateStarDisplay(winStars);

        winPanel?.SetActive(false);
        gameOverPanel?.SetActive(false);
        nextLevelButton?.SetActive(false);
        retryButton?.SetActive(false);

        SaveProgress(SceneManager.GetActiveScene().name);

        if (komponen >= targetKomponen)
        {
            winText.text = "YOU WIN";
            winPanel?.SetActive(true);
            if (currentStarRating == 3)
            {
                nextLevelButton?.SetActive(true);

                // Reset progress jika level terakhir selesai dengan 3 bintang
                if (SceneManager.GetActiveScene().name == "Level3")
                {
                    PlayerPrefs.DeleteAll();
                    PlayerPrefs.Save();
                    Debug.Log("Progress di-reset karena Level3 selesai dengan 3 bintang!");
                }
            }
        }
        else if (komponen >= 10)
        {
            winText.text = "TRY AGAIN";
            winPanel?.SetActive(true);
            retryButton?.SetActive(true);
        }
        else
        {
            gameOverPanel?.SetActive(true);
        }

        Time.timeScale = 0;
    }

    public void SaveProgress(string levelName)
    {
        string starKey = "Level_" + levelName + "_Stars";
        string komponenKey = "Level_" + levelName + "_Komponen";

        int savedStars = PlayerPrefs.GetInt(starKey, 0);

        if (currentStarRating > savedStars)
        {
            PlayerPrefs.SetInt(starKey, currentStarRating);
            PlayerPrefs.SetInt(komponenKey, komponen);
            PlayerPrefs.Save();
        }
    }

    public void TriggerGameOverByFall()
    {
        if (isGameEnded) return;
        isGameEnded = true;
        Time.timeScale = 0;
        gameOverPanel?.SetActive(true);
    }

    public void OnRetryButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnCloseButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public int GetKomponenCount() => komponen;
    public int GetCurrentStarRating() => currentStarRating;
}
