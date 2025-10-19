using UnityEngine;

public class FallingObject : MonoBehaviour
{
    // T?c ?? r?i, b?n có th? ch?nh trong Inspector
    public float fallSpeed = 15f;

    // Hàm Update ???c g?i m?i khung hình
    void Update()
    {
        // Di chuy?n v?t th? này theo h??ng ?i xu?ng (Vector2.down)
        // Nhân v?i fallSpeed và Time.deltaTime ?? có t?c ?? ?n ??nh và m??t mà
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
