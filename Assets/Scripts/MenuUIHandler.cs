using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(1500)]
public class MenuUIHandler : MonoBehaviour
{
    public InputField PlayerName;
    public Text best;
    // Start is called before the first frame update
    void Start()
    {
        if (ScoreManager.Instanse.player.playerName != "")
        {
            PlayerName.text = ScoreManager.Instanse.player.playerName;
            //Debug.Log("aready read.");
        }
        if(ScoreManager.Instanse.bestPlayerScore.playerName != "")
        {
            best.text = $"Best Score:  {ScoreManager.Instanse.bestPlayerScore.playerName} : {ScoreManager.Instanse.bestPlayerScore.score}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNew()
    {
        if (PlayerName.text != "")
        {
            ScoreManager.Instanse.player.playerName = PlayerName.text;
        }
        else
        {
            ScoreManager.Instanse.player.playerName = "Player";
        }
        ScoreManager.Instanse.RefreshName();
        SceneManager.LoadScene("main");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
