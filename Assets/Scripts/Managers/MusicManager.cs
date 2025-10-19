using UnityEngine;
using UnityEngine.SceneManagement; // Thêm dòng này
[System.Serializable]
public class SceneMusic
{
    public string sceneName;
    public AudioClip musicClip;
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioClip startSceneMusic;
    //public AudioClip levelMusic;
    public SceneMusic[] levelMusicList;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UpdateMusicForScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        UpdateMusicForScene(scene.name);
    }

    private void UpdateMusicForScene(string sceneName)
    {
        if (sceneName == "StartScene")
        {
            SwitchTo(startSceneMusic);
        }
        else
        {
            AudioClip clipToPlay = null;
            foreach (var sceneMusic in levelMusicList)
            {
                // LỖI: Biến 'scene' không tồn tại. 
                // Ở đây bạn chỉ có biến 'sceneName' là một chuỗi (string).
                if (sceneMusic.sceneName == sceneName)
                {
                    clipToPlay = sceneMusic.musicClip;
                    break;
                }
            }
            SwitchTo(clipToPlay);
        }
    }

    private void SwitchTo(AudioClip clip)
    {
        if (clip == null || audioSource.clip == clip) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopMusic() => audioSource.Stop();
    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
    public void PauseMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }
    public void ResumeMusic()
    {
        if (!audioSource.isPlaying)
            audioSource.UnPause();
    }
    public void RestartMusic()
    {
        audioSource.Stop();
        audioSource.time = 0f;
        audioSource.Play();
    }
}
