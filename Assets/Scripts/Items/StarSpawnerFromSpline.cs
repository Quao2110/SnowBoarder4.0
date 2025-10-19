using UnityEngine;
using UnityEngine.U2D;

public class StarSpawnerFromSpline : MonoBehaviour
{
    public GameObject starPrefab;
    public float heightAboveGround = 1.7f;
    public int spawnEveryNPoints = 1;
    public int starsPerPoint = 3;

    void Start()
    {
        SpriteShapeController spriteShape = GetComponent<SpriteShapeController>();

        if (spriteShape == null || starPrefab == null)
        {
            return;
        }

        var spline = spriteShape.spline;
        int pointCount = spline.GetPointCount();
        int skipFirstNPoints = 3;

        //for (int i = skipFirstNPoints; i < pointCount - 4; i += spawnEveryNPoints)
        //{
        //    Vector3 localPos = spline.GetPosition(i);
        //    Vector3 worldPos = transform.TransformPoint(localPos);

        //    Vector3 spawnPos = new Vector3(worldPos.x, worldPos.y + heightAboveGround, 0f);

        //    float spacing = 1.5f;
        //    int starsThisPoint = Random.Range(2, 5);

        //    for (int j = 0; j < starsThisPoint; j++)
        //    {
        //        Vector3 offset = new Vector3(j * spacing - spacing, 0f, 0f);
        //        Instantiate(starPrefab, spawnPos + offset, Quaternion.identity);
        //    }  
        //}

        for (int i = skipFirstNPoints; i < pointCount - 4; i += spawnEveryNPoints)
        {
            Vector3 localPos = spline.GetPosition(i);
            Vector3 worldPos = transform.TransformPoint(localPos);

            int starsThisPoint = Random.Range(2, 5);

            float extraHeight = (starsThisPoint == 4) ? 2f : 0f;
            Vector3 basePos = new Vector3(worldPos.x, worldPos.y + heightAboveGround + extraHeight, 0f);

            float spacing = 1.5f;

            for (int j = 0; j < starsThisPoint; j++)
            {
                float xOffset = (j - (starsThisPoint - 1) / 2f) * spacing;

                float heightCurve = Mathf.Sin((j / (float)(starsThisPoint - 1)) * Mathf.PI) * 1.0f;

                Vector3 offset = new Vector3(xOffset, heightCurve, 0f);
                Instantiate(starPrefab, basePos + offset, Quaternion.identity);
            }
        }
    }
}
