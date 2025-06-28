using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource bgMusicSource;
    private bool isMuted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            bgMusicSource = GetComponent<AudioSource>();

            // Load setting dari PlayerPrefs
            isMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
            ApplyMuteState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleMusic()
    {
        isMuted = !isMuted;
        ApplyMuteState();

        // Simpan preferensi
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ApplyMuteState()
    {
        if (bgMusicSource != null)
        {
            bgMusicSource.mute = isMuted;
        }
    }

    public bool IsMuted()
    {
        return isMuted;
    }
}
