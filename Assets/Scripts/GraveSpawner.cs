using UnityEngine;
using UnityEngine.SceneManagement;

public class GraveSpawner : MonoBehaviour
{
    [SerializeField] GameObject gravePrefab;

    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        var graves = GraveRegistry.GetGravesForScene(currentScene);

        foreach (Vector3 pos in graves)
        {
            Instantiate(gravePrefab, pos, Quaternion.identity);
        }
    }
}
