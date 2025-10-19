using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject enterNamePanel;
    public GameObject leaderboardPanel;
    public GameObject instructionPanel;

    [Header("Name Input")]
    public TMP_InputField nameInputField;
    public Button continueButton;
    public TextMeshProUGUI errorMessageText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enterNamePanel.SetActive(false);
        instructionPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        if (errorMessageText != null) errorMessageText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPlayButtonClicked()
    {
        enterNamePanel.SetActive(true);
        if (errorMessageText != null) errorMessageText.gameObject.SetActive(false);
        nameInputField.text = "";
    }
    public void OnContinueButtonClicked()
    {
        string playerName = nameInputField.text;
        if (string.IsNullOrWhiteSpace(playerName))
        {
            if (errorMessageText != null)
            {
                errorMessageText.text = "NAME IS REQUIRED";
                errorMessageText.gameObject.SetActive(true);
            }
            return;
        }
        GameStateManager.instance.CurrentPlayerName = playerName;
        SceneManager.LoadScene("Level 1");
    }
    public void ShowInstructions()
    {
        instructionPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionPanel.SetActive(false);
    }
    public void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
    public void HideNamePanel()
    {
        enterNamePanel.SetActive(false );
    }
    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
    }
    public void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }
}
