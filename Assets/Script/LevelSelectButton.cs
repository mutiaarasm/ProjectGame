using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class LevelSelectButton : MonoBehaviour
{
    public string levelName;                 // Nama scene, contoh: "stone", "Level2", "Level3"
    public GameObject[] starImages;          // Isi dengan 3 GameObject bintang di Inspector
    public Button button;                    // Button level

    void Start()
    {
        ShowStars();
        SetButtonInteractable();
    }

    void ShowStars()
    {
        int starCount = PlayerPrefs.GetInt("Level_" + levelName + "_Stars", 0);

        for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] != null)
                starImages[i].SetActive(i < starCount); // Aktifkan sesuai jumlah bintang
        }
    }

    void SetButtonInteractable()
    {
        bool isUnlocked = false;

        if (levelName == "stone")
        {
            isUnlocked = true; // Level awal selalu terbuka
        }
        else
        {
            string previousLevel = GetPreviousLevelName(levelName);
            int prevStars = PlayerPrefs.GetInt("Level_" + previousLevel + "_Stars", 0);
            isUnlocked = prevStars >= 3; // ✅ Ubah ke 1 bintang agar level bisa dibuka meskipun belum perfect
        }

        if (button != null)
            button.interactable = isUnlocked;
    }

    public void LoadLevel()
    {
        if (!string.IsNullOrEmpty(levelName))
            SceneManager.LoadScene(levelName);
    }

    string GetPreviousLevelName(string currentLevel)
    {
        // Penanganan khusus: urutan level ditentukan manual
        if (currentLevel == "Level2") return "stone";
        if (currentLevel == "Level3") return "Level2";

        return ""; // Kalau tidak cocok
    }
}
