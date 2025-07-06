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
                targetKomponen = 70;
                starThresholds = new int[] { 10, 20, 30 };
                break;
            case "Level2":
                targetKomponen = 100;
                starThresholds = new int[] { 15, 17, 20 };
                break;
            case "Level3":
                targetKomponen = 105;
                starThresholds = new int[] { 15, 18, 20 };
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
            HandleTimeoutGameEnd();
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
        if (isGameEnded) return;

        komponen += amount;
        UpdateKomponenDisplay();
        UpdateStarRating();
        UpdateStarDisplay(gameplayStars);

        if (currentStarRating == 3)
        {
            WinGame(); // Menang otomatis jika dapat bintang 3
        }
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

    private void WinGame()
    {
        isGameEnded = true;
        ResetStars(gameplayStars);
        ResetStars(winStars);
        UpdateStarDisplay(winStars);

        winText.text = "YOU WIN";
        winPanel?.SetActive(true);
        retryButton?.SetActive(false);
        gameOverPanel?.SetActive(false);

        nextLevelButton?.SetActive(true);
        SaveProgress(SceneManager.GetActiveScene().name);
        CheckAndResetAllProgress();

        Time.timeScale = 0;
    }

    private void HandleTimeoutGameEnd()
    {
        if (isGameEnded) return;

        isGameEnded = true;
        ResetStars(gameplayStars);
        ResetStars(winStars);
        UpdateStarDisplay(winStars);

        if (currentStarRating < 3)
        {
            winText.text = "TRY AGAIN";
            winPanel?.SetActive(true);
            retryButton?.SetActive(true);
            gameOverPanel?.SetActive(false);
        }

        nextLevelButton?.SetActive(false);

        // Tambahkan ini agar tetap menyimpan bintang
        SaveProgress(SceneManager.GetActiveScene().name);

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
        }

        if (currentStarRating == 3)
        {
            if (levelName == "stone")
                PlayerPrefs.SetInt("Level2_Unlocked", 1);
            else if (levelName == "Level2")
                PlayerPrefs.SetInt("Level3_Unlocked", 1);
        }

        PlayerPrefs.Save();
    }

    private void CheckAndResetAllProgress()
    {
        string[] levelNames = { "stone", "Level2", "Level3" };
        bool allPerfect = true;

        foreach (string level in levelNames)
        {
            int stars = PlayerPrefs.GetInt("Level_" + level + "_Stars", 0);
            if (stars < 3)
            {
                allPerfect = false;
                break;
            }
        }

        if (allPerfect)
        {
            Debug.Log("Semua level sudah bintang 3. Reset semua progress.");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("stone_Unlocked", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void TriggerGameOverByFall()
    {
        if (isGameEnded) return;
        isGameEnded = true;
        SaveProgress(SceneManager.GetActiveScene().name); // Tambahan juga di sini
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
