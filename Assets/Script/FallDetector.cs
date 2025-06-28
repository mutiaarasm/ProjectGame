using UnityEngine;

public class FallDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Pastikan player punya tag "Player"
        {
            if (KomponenManager.instance != null)
            {
                KomponenManager.instance.TriggerGameOverByFall();
            }
        }
    }
}
