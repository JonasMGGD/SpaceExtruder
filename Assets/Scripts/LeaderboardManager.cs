using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class ScoreEntry {
    public string playerName;
    public int score;
}

[System.Serializable]
public class ScoreList {
    public List<ScoreEntry> entries = new List<ScoreEntry>();
}

public class LeaderboardManager : MonoBehaviour {
    public static LeaderboardManager instance;

    public List<ScoreEntry> scores = new List<ScoreEntry>();
    private string _savePath;

    private void Awake() {
        if (!instance) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            _savePath = Application.persistentDataPath + "/scores.json";
            LoadScores();
        }
        else {
            Destroy(gameObject);
        }
    }

    public void AddScore(string playerName, int newScore) {
        var existing = scores.FirstOrDefault(s => s.playerName == playerName);
        if (existing != null) {
            if (newScore > existing.score)
                existing.score = newScore;
        }
        else {
            scores.Add(new ScoreEntry { playerName = playerName, score = newScore });
        }

        SaveScores();
    }

    public void ClearScores() {
        scores.Clear();
        File.Delete(_savePath);
    }

    private void SaveScores() {
        var data = new ScoreList { entries = scores };
        var json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_savePath, json);
    }

    private void LoadScores() {
        if (!File.Exists(_savePath)) return;
        var json = File.ReadAllText(_savePath);
        var data = JsonUtility.FromJson<ScoreList>(json);
        scores = data.entries ?? new List<ScoreEntry>();
    }
}