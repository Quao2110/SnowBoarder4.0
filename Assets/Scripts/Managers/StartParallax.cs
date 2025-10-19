using UnityEngine;

public class StartParallax : MonoBehaviour
{
    public float offsetMultiplier = 0.3f;
    public float smoothTime = 0.3f;

    private Vector2 startPos;
    private Vector3 velocity;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (Camera.main == null)
            return;

        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // Trừ 0.5f để chuột ở giữa thì offset = (0,0)
        offset -= new Vector2(0.5f, 0.5f);

        if (float.IsNaN(offset.x) || float.IsNaN(offset.y))
            return;

        Vector3 targetPos = startPos + (Vector2)(offset * offsetMultiplier);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
        );
    }
}
