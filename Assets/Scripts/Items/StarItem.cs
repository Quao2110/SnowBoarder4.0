using UnityEngine;
using UnityEngine.SceneManagement;

public class StarItem : MonoBehaviour
{
    public int scoreValue = 10;
    [SerializeField] private AudioClip collectSound;
    private AudioSource audioSource;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(scoreValue);
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            Destroy(gameObject);
        }
    }
}
