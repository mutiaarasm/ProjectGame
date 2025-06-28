using UnityEngine;

public class PersistentMusic : MonoBehaviour
{
    public static PersistentMusic instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Musik tidak hilang saat ganti scene
        }
        else
        {
            Destroy(gameObject); // Kalau sudah ada, hapus duplikat
        }
    }
}
