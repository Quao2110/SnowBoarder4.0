using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class LeaderboardStorage
{
    private static string filePath => Path.Combine(Application.persistentDataPath, "leaderboard.json");

    public static void Save(LeaderboardData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public static LeaderboardData Load()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<LeaderboardData>(json);
        }
        return new LeaderboardData();
    }

    public static void AddEntry(LeaderboardEntry entry)
    {
        LeaderboardData data = Load();
        data.entries.Add(entry);
        data.entries = data.entries.OrderByDescending(e => e.score).Take(10).ToList(); // Keep top 10
        Save(data);
    }
}
