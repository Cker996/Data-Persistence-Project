using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreUIHander : MonoBehaviour
{
    public Text nameS;
    public Text score;

    private void Update()
    {
        string nameoutput = "";
        string scoreoutput = "";
        for (int i = 1; i < ScoreManager.Instanse.scoreList.Count; i++)
        {
            nameoutput += string.Format("{0:D2}. {1}\n", i,ScoreManager.Instanse.scoreList[i].playerName);
            scoreoutput += string.Format("{0}\n", ScoreManager.Instanse.scoreList[i].score);
        }
        nameS.text = nameoutput;
        score.text = scoreoutput;
    }

    public void BackToGame()
    {
        SceneManager.LoadScene("main");
    }
}
