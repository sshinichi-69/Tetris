using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Set_Data : MonoBehaviour
{
    public Text TextScore;
    public Text TextGameOver;
    public Text TextPause;
    public Text TextTetroLeft;
    public Text TextSpeed;
    public bool start = true;
    int lastScene = 7;
    public void SetScore(int score)
    {
        TextScore.text = score.ToString();
    }

    public void BeStarted()
    {
        start = false;
    }

    public void SetGameOver()
    {
        TextGameOver.text = "GAME OVER";
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }

    public void SetPause(bool pause)
    {
        if (pause)
        {
            TextPause.text = "PAUSE";
        }
        else
        {
            TextPause.text = "";
        }
    }

    public void SetVictory()
    {
        TextGameOver.text = "VICTORY";
        StartCoroutine(Victory());
    }

    IEnumerator Victory()
    {
        yield return new WaitForSeconds(5);
        int scene = SceneManager.GetActiveScene().buildIndex;
        if (scene == lastScene)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(scene + 1);
        }
    }

    public void setTetroLeft(int n)
    {
        TextTetroLeft.text = "Tetro Left: " + n;
    }

    public void setSpeed(int n)
    {
        TextSpeed.text = "Speed: " + n;
    }
}
