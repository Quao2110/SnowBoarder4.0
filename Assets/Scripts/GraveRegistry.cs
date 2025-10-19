using System.Collections.Generic;
using UnityEngine;

public static class GraveRegistry
{
    // Key = Scene name, Value = List of positions
    public static Dictionary<string, List<Vector3>> GravePositionsByScene = new Dictionary<string, List<Vector3>>();

    public static void AddGrave(string sceneName, Vector3 position)
    {
        if (!GravePositionsByScene.ContainsKey(sceneName))
        {
            GravePositionsByScene[sceneName] = new List<Vector3>();
        }
        GravePositionsByScene[sceneName].Add(position);
    }

    public static List<Vector3> GetGravesForScene(string sceneName)
    {
        if (GravePositionsByScene.ContainsKey(sceneName))
        {
            return GravePositionsByScene[sceneName];
        }
        return new List<Vector3>();
    }

    public static void Clear()
    {
        GravePositionsByScene.Clear();
    }
}
