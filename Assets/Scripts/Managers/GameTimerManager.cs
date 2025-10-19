using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimerManager : MonoBehaviour
{
    public static GameTimerManager Instance;

    public TMPro.TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    private bool isRunning = true;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ lại khi load scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText == null)
        {
            // Cố tìm lại text mỗi frame nếu mất tham chiếu
            GameObject timerGO = GameObject.FindWithTag("TimerText");
            if (timerGO != null)
            {
                timerText = timerGO.GetComponent<TMPro.TextMeshProUGUI>();
            }
            else
            {
                return; // Không làm gì nếu vẫn không tìm thấy
            }
        }

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }
    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public float GetTime()
    {
        return elapsedTime;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (timerText == null)
        {
            GameObject timerGO = GameObject.FindWithTag("TimerText");
            if (timerGO != null)
            {
                timerText = timerGO.GetComponent<TMPro.TextMeshProUGUI>();
            }
        }

        // Reset & start timer mỗi khi scene load lại
        //ResetTimer();
        StartTimer();
    }
}
