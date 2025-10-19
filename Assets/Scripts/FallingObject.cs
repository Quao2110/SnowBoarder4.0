using UnityEngine;

public class FallingObject : MonoBehaviour
{
    // T?c ?? r?i, b?n c� th? ch?nh trong Inspector
    public float fallSpeed = 15f;

    // H�m Update ???c g?i m?i khung h�nh
    void Update()
    {
        // Di chuy?n v?t th? n�y theo h??ng ?i xu?ng (Vector2.down)
        // Nh�n v?i fallSpeed v� Time.deltaTime ?? c� t?c ?? ?n ??nh v� m??t m�
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject); // Bi?n m?t khi ch?m tuy?t
        }
    }
}
