using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    private AudioSource audioSource;

    bool hasHit = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit) return;

        if (collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();

            if (player != null && player.IsInvincible)
            {
                Debug.Log("🛡️ Player bất tử - không trừ điểm");
                return; // Bỏ qua va chạm khi bất tử
            }

            hasHit = true;
            ScoreManager.Instance.AddScore(-15);
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
    }

}
