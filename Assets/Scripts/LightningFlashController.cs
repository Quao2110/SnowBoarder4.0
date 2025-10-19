using UnityEngine;
using System.Collections;

public class LightningFlashController : MonoBehaviour
{
    public Light lightning;
    public AudioSource thunderSound;

    // Các thông s? này s? ???c l?y t? Inspector
    public float minIntensity = 0f;
    public float maxIntensity = 10f; // Gi? giá tr? t? Inspector
    public float flashDuration = 0.1f; // Gi? giá tr? t? Inspector
    public float timeBetweenFlashesMin = 1f; // Gi? giá tr? t? Inspector
    public float timeBetweenFlashesMax = 15f; // Gi? giá tr? t? Inspector

    void Start()
    {
        if (lightning == null)
        {
            lightning = GetComponent<Light>();
        }
        if (thunderSound == null)
        {
            thunderSound = GetComponent<AudioSource>();
        }

        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        // Vòng l?p vô t?n ?? s?m sét l?p l?i mãi mãi
        while (true)
        {
            // Ch? m?t kho?ng th?i gian ng?u nhiên gi?a các ??t s?m sét
            float delay = Random.Range(timeBetweenFlashesMin, timeBetweenFlashesMax);
            yield return new WaitForSeconds(delay);

            // M?t ??t s?m sét có th? có t? 1 ??n 3 cú ch?p nhanh
            int numberOfFlashes = Random.Range(1, 4);
            for (int i = 0; i < numberOfFlashes; i++)
            {
                // B?t ?èn
                lightning.intensity = Random.Range(minIntensity, maxIntensity);

                // Phát âm thanh ? cú ch?p ??u tiên c?a ??t
                if (i == 0 && thunderSound != null)
                {
                    thunderSound.Play();
                }

                // Ch? m?t kho?ng r?t ng?n (th?i gian ch?p sáng)
                yield return new WaitForSeconds(flashDuration);

                // T?t ?èn
                lightning.intensity = minIntensity;

                // Ch? m?t kho?ng siêu ng?n gi?a các cú ch?p trong cùng m?t ??t
                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            }
        }
    }
}