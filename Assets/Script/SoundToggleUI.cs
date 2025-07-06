using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundToggleUI : MonoBehaviour
{
    public Sprite soundOnIcon;       // Icon untuk kondisi ON
    public Sprite soundOffIcon;      // Icon untuk kondisi OFF
    public Image iconImage;          // Gambar UI yang akan diganti
    public TMP_Text labelText;       // Teks ON/OFF (opsional)

    private void OnEnable()
    {
        UpdateSoundUI(); // Update tampilan saat panel aktif
    }

    // Fungsi ini dipanggil saat tombol diklik
    public void ToggleSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ToggleMusic(); // Ubah status mute
            UpdateSoundUI(); // Perbarui UI-nya
        }
        else
        {
            Debug.LogWarning("AudioManager tidak ditemukan!");
        }
    }

    // Fungsi ini hanya memperbarui icon dan teks
    public void UpdateSoundUI()
    {
        if (iconImage == null || soundOnIcon == null || soundOffIcon == null)
        {
            Debug.LogWarning("Ada komponen icon/sprite yang belum diisi!");
            return;
        }

        bool isMuted = AudioManager.instance != null && AudioManager.instance.IsMuted();

        iconImage.sprite = isMuted ? soundOffIcon : soundOnIcon;

        if (labelText != null)
        {
            labelText.text = isMuted ? "OFF" : "ON";
        }
    }
}
