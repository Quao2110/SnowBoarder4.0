using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public float floatSpeed = 1f;      
    public float floatHeight = 0.4f;  

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);
    }
}
