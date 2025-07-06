using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public string levelName;             // Nama level: "stone", "Level2", dst.
    public GameObject[] starImages;      // Gambar bintang di UI
    public Button button;                // Tombol level

    void Start()
    {
        ShowStars();
        SetButtonInteractable();
    }

    void ShowStars()
    {
        int starCount = PlayerPrefs.GetInt("Level_" + levelName + "_Stars", 0);
        Debug.Log($"Menampilkan {starCount} bintang untuk level {levelName}");

        for (int i = 0; i < starImages.Length; i++)
        {
            if (starImages[i] != null)
                starImages[i].SetActive(i < starCount);
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
            isUnlocked = (prevStars == 3); // Level terbuka HANYA jika sebelumnya dapat 3 bintang
        }

        if (button != null)
            button.interactable = isUnlocked;
    }

    public void LoadLevel()
    {
        if (!string.IsNullOrEmpty(levelName))
            SceneManager.LoadScene(levelName);
    }

    string GetPreviousLevelName(string current)
    {
        if (current == "Level2") return "stone";
        if (current == "Level3") return "Level2";
        if (current == "Level4") return "Level3";
        // Tambah level lainnya di sini jika perlu
        return "";
    }
}
