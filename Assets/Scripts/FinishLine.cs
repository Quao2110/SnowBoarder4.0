using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] float loadDelay = 1f;
    [SerializeField] ParticleSystem finishEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (finishEffect != null)
            {
                finishEffect.Play();
            }

            var audio = GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }

            Invoke("LoadNextLevel", loadDelay);
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Nếu là màn cuối thì chuyển sang WinnerScene
        if (currentSceneIndex == 3)
        {
            SceneManager.LoadScene("WinnerSence");
        }
        else
        {
            int nextSceneIndex = currentSceneIndex + 1;
            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
            }

            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
