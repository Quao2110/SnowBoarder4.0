using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    [Header("Di chuy?n ngang")]
    [Tooltip("T?c ?? di chuy?n c?a con ma.")]
    public float moveSpeed = 2f;
    [Tooltip("Con ma s? bay sang m?i bên bao xa t? v? trí b?t ??u.")]
    public float moveRange = 5f;

    [Header("Bay theo ??a hình")]
    [Tooltip("?? cao con ma bay l? l?ng trên m?t ??t.")]
    public float hoverHeight = 1.5f;
    [Tooltip("Ch?n Layer c?a m?t ??t ?? con ma có th? 'nhìn' th?y.")]
    public LayerMask groundLayer;

    private Vector3 startPosition;
    private int moveDirection = 1;

    void Start()
    {
        startPosition = transform.position;
    }

    // S? d?ng FixedUpdate ?? các tính toán v?t lý ?n ??nh h?n
    void FixedUpdate()
    {
        HandleHorizontalMovement();
        HandleVerticalMovement();
    }

    // Hàm x? lý di chuy?n ngang
    void HandleHorizontalMovement()
    {
        transform.Translate(Vector3.right * moveSpeed * moveDirection * Time.deltaTime);

        if (transform.position.x >= startPosition.x + moveRange)
        {
            moveDirection = -1;
            FlipSprite();
        }
        else if (transform.position.x <= startPosition.x - moveRange)
        {
            moveDirection = 1;
            FlipSprite();
        }
    }

    // Hàm x? lý di chuy?n d?c theo ??a hình
    void HandleVerticalMovement()
    {
        // B?n m?t tia Raycast t? v? trí con ma th?ng xu?ng d??i
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, groundLayer);

        // N?u tia Raycast ch?m vào m?t v?t th? (có collider và thu?c groundLayer)
        if (hit.collider != null)
        {
            // C?p nh?t l?i v? trí Y c?a con ma = v? trí va ch?m + ?? cao l? l?ng
            // Gi? nguyên v? trí X và Z c?a nó
            transform.position = new Vector3(transform.position.x, hit.point.y + hoverHeight, transform.position.z);
        }
    }

    void FlipSprite()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

}