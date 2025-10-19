using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class GameWinnerManager : MonoBehaviour
    {
        public TextMeshProUGUI playerNameText;
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI crashCountText;
        public GameObject winnerMenuUI;
        void Start()
        {
            ShowResult();
        }

        void ShowResult()
        {
            if (GameStateManager.instance != null)
                playerNameText.text = $"Name: {GameStateManager.instance.CurrentPlayerName}";

            if (GameTimerManager.Instance != null)
            {
                float elapsed = GameTimerManager.Instance.GetTime();
                int minutes = Mathf.FloorToInt(elapsed / 60f);
                int seconds = Mathf.FloorToInt(elapsed % 60f);
                timeText.text = $"Time: {minutes:00}:{seconds:00}";
            }

            if (ScoreManager.Instance != null)
                scoreText.text = $"Score: {ScoreManager.Instance.GetScore()}";

            crashCountText.text = $"Death: {CrashCounter.CrashCount}";
        }

        public void MainMenu()
        {
            LeaderboardEntry entry = new LeaderboardEntry
            {
                playerName = GameStateManager.instance?.CurrentPlayerName ?? "Unknown",
                score = ScoreManager.Instance?.GetScore() ?? 0,
                timeInSeconds = GameTimerManager.Instance?.GetTime() ?? 0f,
                deathCount = CrashCounter.CrashCount
            };

            LeaderboardStorage.AddEntry(entry);

            // Reset
            Destroy(GameStateManager.instance?.gameObject);
            Destroy(ScoreManager.Instance?.gameObject);
            Destroy(MusicManager.Instance?.gameObject);
            Destroy(GameTimerManager.Instance?.gameObject);
            CrashCounter.CrashCount = 0;

            SceneManager.LoadScene(0);
        }

    }
}
