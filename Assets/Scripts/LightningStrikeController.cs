using UnityEngine;
using System.Collections; // nh? khai báo ?? dùng Coroutine

public class LightningStrikeController : MonoBehaviour
{
    public GameObject lightningPrefab;
    public float spawnInterval = 2f;
    public float minX = -10f;
    public float maxX = 10f;
    public float spawnY = 5f;
    public GameObject flashPanel; // gán Panel này trong Inspector
    public AudioSource audioSource;
    public AudioClip lightningSound;

    void Start()
    {
        InvokeRepeating(nameof(SpawnLightning), 1f, spawnInterval);
    }

    void SpawnLightning()
    {
        float screenMinX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenMaxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float screenTopY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + 1f;
        StartCoroutine(FlashScreen());
        if (audioSource != null && lightningSound != null)
        {
            audioSource.PlayOneShot(lightningSound);
        }
        float randomX = Random.Range(screenMinX, screenMaxX);
        Vector3 spawnPos = new Vector3(randomX, screenTopY, 0f);
        Instantiate(lightningPrefab, spawnPos, Quaternion.identity);

    }
    IEnumerator FlashScreen()
    {
        flashPanel.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flashPanel.SetActive(false);

    }
}