using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instanse;
    public PlayerScore bestPlayerScore;
    public PlayerScore player;
    private static bool isLoaded = false;
    public List<PlayerScore> scoreList = new List<PlayerScore>();

    private void Awake()
    {
        if (!isLoaded)
        {
            Instanse = this;
            DontDestroyOnLoad(gameObject);
            isLoaded = true;
        }
        InitScoreList();
    }

    void InitScoreList()
    {

        try
        {
            string path = Application.persistentDataPath + "/SaveScore.json";
            string path1 = Application.persistentDataPath + "/SaveScore.bin";
            if (File.Exists(path))
            {
                LoadScoreListJson(path);
                player.playerName = scoreList[0].playerName;
                bestPlayerScore.playerName = scoreList[1].playerName;
                bestPlayerScore.score = scoreList[1].score;
            }else if (File.Exists(path1))
            {
                LoadScoreListBinary(path1);
                player.playerName = scoreList[0].playerName;
                bestPlayerScore.playerName = scoreList[1].playerName;
                bestPlayerScore.score = scoreList[1].score;
            }
        }
        finally
        {
            if (ScoreManager.Instanse.player.playerName == "")
            {
                player.playerName = "First";
                player.score = 0;
                scoreList.Add(new PlayerScore() { score = player.score, playerName = player.playerName });
                player.playerName = "";
            }
        }

    }

    public void RefreshName()
    {
        if (scoreList.Count == 1)
        {
            scoreList.Insert(0, new PlayerScore() { score = player.score, playerName = player.playerName });
        }
        else
        {
            if (scoreList[0].playerName.CompareTo(player.playerName) != 0)
            {
                UpdateScore();
            }
        }
        bestPlayerScore.playerName = scoreList[1].playerName;
        bestPlayerScore.score = scoreList[1].score;
    }

    void UpdateScore()
    {
        scoreList[0].score = player.score;
        scoreList[0].playerName = player.playerName;
    }

    public void RefreshScore()
    {
        UpdateScore();
        scoreList.Sort(delegate (PlayerScore x, PlayerScore y)
        {
            if (x.score < y.score) return 1;
            else if (x.score == y.score) return 0;
            else return -1;

        });
        for (int i = 11; i < scoreList.Count; i++)
        {
            scoreList.RemoveAt(i);
        }

        player.score = 0;
        scoreList.Insert(0, new PlayerScore() { score = player.score, playerName = player.playerName });
        bestPlayerScore.playerName = scoreList[1].playerName;
        bestPlayerScore.score = scoreList[1].score;
    }

    public void SaveScoreList()
    {
        string json = "";
        foreach (PlayerScore ps in scoreList)
        {
            json += JsonUtility.ToJson(ps) + "\n";
        }
        json = json.TrimEnd('\n');
        File.WriteAllText(Application.persistentDataPath + "/SaveScore.json", json);
    }

    private void LoadScoreListJson(string paths)
    {
        string json = File.ReadAllText(paths);
        //Debug.Log($"string: {json}");
        string[] jsonsubs = json.Split('\n');
        foreach (var sub in jsonsubs)
        {
            scoreList.Add(JsonUtility.FromJson<PlayerScore>(sub));
            //Debug.Log($"Substring: {sub}");
            
        }
    }

    public void SaveScoreListBinary()
    {
        string json = "";
        foreach (PlayerScore ps in scoreList)
        {
            json += JsonUtility.ToJson(ps) + "\n";
        }
        json = json.TrimEnd('\n');
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(Application.persistentDataPath + "/SaveScore.bin", FileMode.Create, FileAccess.Write, FileShare.None);
        formatter.Serialize(stream, json);
        stream.Close();
    }
    private void LoadScoreListBinary(string paths)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(paths, FileMode.Open, FileAccess.Read, FileShare.Read);
        string json = (string)formatter.Deserialize(stream);
        stream.Close();
        //Debug.Log($"string: {json}");
        string[] jsonsubs = json.Split('\n');
        foreach (var sub in jsonsubs)
        {
            scoreList.Add(JsonUtility.FromJson<PlayerScore>(sub));
            //Debug.Log($"Substring: {sub}");

        }
    }

    [System.Serializable]
    public class PlayerScore
    {
        public int score;
        public string playerName;
    }
}
