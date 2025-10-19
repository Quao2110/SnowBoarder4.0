using UnityEngine;
using System.Collections;

public class LightningFlashController : MonoBehaviour
{
    public Light lightning;
    public AudioSource thunderSound;

    // C�c th�ng s? n�y s? ???c l?y t? Inspector
    public float minIntensity = 0f;
    public float maxIntensity = 10f; // Gi? gi� tr? t? Inspector
    public float flashDuration = 0.1f; // Gi? gi� tr? t? Inspector
    public float timeBetweenFlashesMin = 1f; // Gi? gi� tr? t? Inspector
    public float timeBetweenFlashesMax = 15f; // Gi? gi� tr? t? Inspector

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
        // V�ng l?p v� t?n ?? s?m s�t l?p l?i m�i m�i
        while (true)
        {
            // Ch? m?t kho?ng th?i gian ng?u nhi�n gi?a c�c ??t s?m s�t
            float delay = Random.Range(timeBetweenFlashesMin, timeBetweenFlashesMax);
            yield return new WaitForSeconds(delay);

            // M?t ??t s?m s�t c� th? c� t? 1 ??n 3 c� ch?p nhanh
            int numberOfFlashes = Random.Range(1, 4);
            for (int i = 0; i < numberOfFlashes; i++)
            {
                // B?t ?�n
                lightning.intensity = Random.Range(minIntensity, maxIntensity);

                // Ph�t �m thanh ? c� ch?p ??u ti�n c?a ??t
                if (i == 0 && thunderSound != null)
                {
                    thunderSound.Play();
                }

                // Ch? m?t kho?ng r?t ng?n (th?i gian ch?p s�ng)
                yield return new WaitForSeconds(flashDuration);

                // T?t ?�n
                lightning.intensity = minIntensity;

                // Ch? m?t kho?ng si�u ng?n gi?a c�c c� ch?p trong c�ng m?t ??t
                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            }
        }
    }
}