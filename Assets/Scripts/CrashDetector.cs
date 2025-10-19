using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] float loadDelay = 1f;
    [SerializeField] ParticleSystem crashEffect;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] GameObject gravePrefab;
    bool hasCrashed;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu đã va chạm rồi thì bỏ qua
        if (hasCrashed) return;

        // Kiểm tra có va chạm với Death hoặc Ground
        if (other.CompareTag("Death") || other.CompareTag("Ground"))
        {
            var player = FindObjectOfType<PlayerController>();

            // 🛡️ Nếu người chơi đang bất tử → bỏ qua chết
            if (player != null && player.IsInvincible)
            {
                Debug.Log("🛡️ Player is invincible — ignore crash reload.");
                return;
            }

            // Hiệu ứng va chạm và âm thanh
            if (crashEffect != null) crashEffect.Play();
            var audio = GetComponent<AudioSource>();
            if (audio != null && crashSFX != null)
                audio.PlayOneShot(crashSFX);

            hasCrashed = true;

            // Giảm điểm và lưu dữ liệu
            ScoreManager.Instance.AddScore(-50);
            CrashCounter.CrashCount++;
            player?.DisableControls();

            // Tạo mộ tại vị trí chết
            Vector3 gravePosition = transform.position;
            gravePosition.y = other.ClosestPoint(transform.position).y;
            Instantiate(gravePrefab, gravePosition, Quaternion.identity);

            // Lưu mộ theo màn chơi
            string currentScene = SceneManager.GetActiveScene().name;
            GraveRegistry.AddGrave(currentScene, gravePosition);

            // Dừng thời gian nếu có timer
            if (GameTimerManager.Instance != null)
                GameTimerManager.Instance.StopTimer();

            // Gọi reload scene sau delay
            Invoke(nameof(ReloadScene), loadDelay);
        }
    }

    void ReloadScene()
    {
        // Kiểm tra lại bất tử 1 lần nữa trước khi reload
        var player = FindObjectOfType<PlayerController>();
        if (player != null && player.IsInvincible)
        {
            Debug.Log("🛡️ Player is invincible — skip reload in ReloadScene()");
            return;
        }

        if (MusicManager.Instance != null)
            MusicManager.Instance.RestartMusic();

        // Tùy chọn reset timer/score nếu bạn muốn
        //if (GameTimerManager.Instance != null)
        //{
        //    GameTimerManager.Instance.ResetTimer();
        //    GameTimerManager.Instance.StartTimer();
        //}
        //if (ScoreManager.Instance != null)
        //{
        //    ScoreManager.Instance.ResetScore();
        //}

        // 🔁 Reload scene
        SceneManager.LoadScene(1);
    }
}
