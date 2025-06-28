using UnityEngine;

public class komponen : MonoBehaviour
{
    [SerializeField] private int value;  // Nilai default 1, bisa diubah di Inspector
    private bool hasTriggered;

    private KomponenManager komponenManager;

    private void Start()
    {
        komponenManager = KomponenManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            komponenManager.ChangeKomponen(value);
            Destroy(gameObject);
        }
    }
}
