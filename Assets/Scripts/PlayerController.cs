using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float boostSpeed = 10f;

    [Header("Flip Settings")]
    [SerializeField] float flipDuration = 0.5f;
    [SerializeField] float flipAngle = 360f;
    [SerializeField] float jumpForce = 7f;

    [Header("Audio Settings")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip upJumpSound;
    [SerializeField] AudioClip downJumpSound;
    [SerializeField] AudioClip enterJumpSound;

    [Header("Debug / God Mode Settings")]
    [SerializeField] bool isInvincible = false;
    [SerializeField] AudioClip toggleInvincibleSound;
    [SerializeField] Color invincibleColor = Color.yellow;
    [SerializeField] float glowIntensity = 3f;

    [Header("Death Settings")]
    [SerializeField] float fallDeathY = -20f;
    [SerializeField] AudioClip deathSound;
    [SerializeField] GameObject deathEffect;

    AudioSource audioSource;
    Rigidbody2D rb2d;
    SurfaceEffector2D surfaceEffector2D;
    SpriteRenderer spriteRenderer;

    bool canMove = true;
    bool isFlipping = false;
    bool isDead = false;
    Color defaultColor;
    Vector3 spawnPoint;

    public bool IsInvincible => isInvincible;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();

        if (spriteRenderer != null)
            defaultColor = spriteRenderer.color;

        spawnPoint = transform.position;

        if (PlayerPrefs.HasKey("Invincible"))
            isInvincible = PlayerPrefs.GetInt("Invincible") == 1;

        EnableAuraEffect(isInvincible);
    }

    void Update()
    {
        if (isDead) return;

        if (isInvincible)
            EnableAuraEffect(true);
        else
            CheckFallDeath();

        if (!canMove) return;

        // ✅ Khi bật bất tử → Shift sẽ tăng tốc gấp 3 lần
        float currentSpeed = Input.GetKey(KeyCode.LeftShift)
            ? (isInvincible ? boostSpeed * 3f : boostSpeed)
            : moveSpeed;

        MovePlayerLeftRight(currentSpeed);
        RespondToBoost();
        RespondToFlipZ();
        RespondToFlipXCombo();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(enterJumpSound, 0.15f);
            StartCoroutine(SpinCombo());
        }

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.LeftShift))
            ToggleInvincibleMode();
    }

    void FixedUpdate()
    {
        if (isInvincible)
            KeepAboveGround();
    }

    void ToggleInvincibleMode()
    {
        isInvincible = !isInvincible;

        if (audioSource && toggleInvincibleSound)
            audioSource.PlayOneShot(toggleInvincibleSound, 0.5f);

        EnableAuraEffect(isInvincible);

        PlayerPrefs.SetInt("Invincible", isInvincible ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("🛡️ Invincible Mode: " + (isInvincible ? "ON" : "OFF"));
    }

    void EnableAuraEffect(bool enabled)
    {
        if (!spriteRenderer) return;

        if (enabled)
            spriteRenderer.color = invincibleColor * glowIntensity;
        else
            spriteRenderer.color = defaultColor;
    }

    void CheckFallDeath()
    {
        if (transform.position.y < fallDeathY && !isDead)
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        if (isInvincible)
        {
            Debug.Log("🛡️ Player is invincible — ignore death.");
            yield break;
        }

        if (isDead) yield break;
        isDead = true;
        canMove = false;

        if (audioSource && deathSound)
            audioSource.PlayOneShot(deathSound);

        if (deathEffect)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        Debug.Log("💀 Player Died - reloading scene...");

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DisableControls()
    {
        if (isInvincible) return;
        canMove = false;
    }

    void MovePlayerLeftRight(float currentSpeed)
    {
        float baseSpeed = currentSpeed;
        float horizontalInput = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
            RotateToLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
            RotateToRight();
        }
        else
        {
            RotateToRight();
        }

        float finalXSpeed = baseSpeed + (horizontalInput * currentSpeed * 0.5f);
        rb2d.linearVelocity = new Vector2(finalXSpeed, rb2d.linearVelocity.y);
    }

    void RotateToLeft()
    {
        Vector3 scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void RotateToRight()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void RespondToBoost()
    {
        // ✅ Khi bật bất tử → tăng tốc gấp 3 lần
        surfaceEffector2D.speed = Input.GetKey(KeyCode.LeftShift)
            ? (isInvincible ? boostSpeed * 3f : boostSpeed)
            : moveSpeed;
    }

    void RespondToFlipZ()
    {
        if (isFlipping) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            audioSource.PlayOneShot(upJumpSound, 0.9f);
            StartCoroutine(FlipZ(-1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            audioSource.PlayOneShot(downJumpSound, 0.75f);
            StartCoroutine(FlipZ(1));
        }
    }

    void RespondToFlipXCombo()
    {
        if (isFlipping) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.PlayOneShot(jumpSound, 0.42f);
            StartCoroutine(FlipXCombo());
        }
    }

    IEnumerator FlipZ(int direction)
    {
        isFlipping = true;
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.identity;

        float totalRotation = 0f;
        float rotationPerFrame = (flipAngle / flipDuration) * Time.deltaTime;

        while (totalRotation < flipAngle)
        {
            transform.Rotate(0f, 0f, rotationPerFrame * direction);
            totalRotation += rotationPerFrame;
            yield return null;
        }

        ScoreManager.Instance.AddScore(2);
        isFlipping = false;
    }

    IEnumerator FlipXCombo()
    {
        isFlipping = true;
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.identity;

        float direction = Input.GetKey(KeyCode.DownArrow) ? 1f : -1f;
        float totalRotation = 0f;
        float rotationPerFrame = (flipAngle / flipDuration) * Time.deltaTime;

        while (totalRotation < flipAngle)
        {
            transform.Rotate(rotationPerFrame * direction, 0f, 0f);
            totalRotation += rotationPerFrame;
            yield return null;
        }

        ScoreManager.Instance.AddScore(2);
        isFlipping = false;
    }

    IEnumerator SpinCombo()
    {
        isFlipping = true;
        rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.identity;

        float directionX = Input.GetKey(KeyCode.DownArrow) ? 1f : -1f;
        float totalRotation = 0f;
        float rotationPerFrame = (flipAngle / flipDuration) * Time.deltaTime;

        while (totalRotation < flipAngle)
        {
            transform.Rotate(rotationPerFrame * directionX, rotationPerFrame, 0f);
            totalRotation += rotationPerFrame;
            yield return null;
        }

        ScoreManager.Instance.AddScore(2);
        isFlipping = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvincible)
        {
            string tag = collision.gameObject.tag;

            if (tag == "Death" || tag == "Enemy" || tag == "Obstacle" || tag == "Hazard")
            {
                Collider2D[] playerCols = GetComponents<Collider2D>();
                Collider2D[] objectCols = collision.gameObject.GetComponents<Collider2D>();

                foreach (var pc in playerCols)
                {
                    foreach (var oc in objectCols)
                    {
                        Physics2D.IgnoreCollision(pc, oc, true);
                        StartCoroutine(ReenableCollision(pc, oc, 0.5f));
                    }
                }

                Debug.Log($"🛡️ Bất tử - xuyên qua {collision.gameObject.name}");
                return;
            }
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb2d.angularVelocity = 0f;
            rb2d.rotation = 0f;
        }

        if (!isInvincible && collision.gameObject.CompareTag("Death") && !isDead)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator ReenableCollision(Collider2D playerCol, Collider2D objCol, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerCol != null && objCol != null)
            Physics2D.IgnoreCollision(playerCol, objCol, false);
    }

    void KeepAboveGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Ground"));

        if (hit.collider != null && hit.distance < 0.8f)
        {
            Vector2 normal = hit.normal;
            Vector2 targetPos = hit.point + normal * 0.45f;

            rb2d.MovePosition(Vector2.Lerp(rb2d.position, targetPos, Time.fixedDeltaTime * 8f));

            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            rb2d.MoveRotation(Mathf.LerpAngle(rb2d.rotation, angle - 90f, Time.fixedDeltaTime * 8f));
        }
    }

    void OnGUI()
    {
        if (isInvincible)
        {
            GUI.color = Color.yellow;
            GUI.Label(new Rect(10, 10, 250, 30), "🛡️ Invincible Mode ON");
        }
    }
}
