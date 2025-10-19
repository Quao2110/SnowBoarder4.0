using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public Transform scoreListContainer;
    public Transform scoreListTemplate;
    public float templateHeight = 100f;

    private void Awake()
    {       
        scoreListTemplate.gameObject.SetActive(false);
        LeaderboardData data = LeaderboardStorage.Load();

        if (data.entries.Count == 0)
        {
            Debug.Log("Leaderboard is empty. No entries to display.");
            return;
        }
        else
        {
            Debug.Log("Loading Data...");
            for (int i = 0; i < data.entries.Count; i++)
            {
                Transform entryTransform = Instantiate(scoreListTemplate, scoreListContainer);
                RectTransform entryRecTransfrom = entryTransform.GetComponent<RectTransform>();
                entryRecTransfrom.anchoredPosition = new Vector2(0, -templateHeight * i);
                entryRecTransfrom.gameObject.SetActive(true);

                LeaderboardEntry entry = data.entries[i];

                entryTransform.Find("NameText").GetComponent<TMPro.TextMeshProUGUI>().text = entry.playerName;
                entryTransform.Find("ScoreText").GetComponent<TMPro.TextMeshProUGUI>().text = entry.score.ToString();
                int seconds = Mathf.FloorToInt(entry.timeInSeconds);
                entryTransform.Find("TimeText").GetComponent<TMPro.TextMeshProUGUI>().text = seconds.ToString() + " seconds";
                entryTransform.Find("DeathText").GetComponent<TMPro.TextMeshProUGUI>().text = entry.deathCount.ToString();
            }
        }
    }    
}
