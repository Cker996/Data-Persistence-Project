using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
        LoadScoreList();
    }

    public void LoadScoreList()
    {
        string path = Application.persistentDataPath + "/SaveScore.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            scoreList = JsonUtility.FromJson<List<PlayerScore>>(json);
        }
        else
        {
            player.playerName = "First";
            player.score = 0;
            scoreList.Add(new PlayerScore() { score = player.score, playerName = player.playerName });
        }
    }

    public void RefreshName()
    {
        if(scoreList.Count == 1)
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
        player.score = 0;
        scoreList.Insert(0, new PlayerScore() { score = player.score, playerName = player.playerName });
        bestPlayerScore.playerName = scoreList[1].playerName;
        bestPlayerScore.score = scoreList[1].score;
    }

    public void SaveScoreList()
    {
        string json = JsonUtility.ToJson(scoreList);
        //File.WriteAllText(Application.persistentDataPath + "/SaveScore.json",json);
    }

    [System.Serializable]
    public class PlayerScore
    {
        public int score;
        public string playerName;
    }
}
