using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetData : MonoBehaviour
{
    public GameObject[] blocks;
    public Text TextLevel;
    public Text TextSpeed;
    public Text TextScore;
    public Text TextGameOver;
    public Text TextPause;
    public bool start = true;
    public GameObject GetBlock()
    {
        return blocks[Random.Range(0, blocks.Length)];
    }

    public void SetLevel(int level)
    {
        TextLevel.text = "Level: " + level.ToString();
    }

    public void SetSpeed(int speed)
    {
        TextSpeed.text = "Speed: " + speed.ToString();
    }

    public void SetScore(int score, int goal)
    {
        if (goal == -1)
        {
            TextScore.text = "Score: " + score;
        }
        else
        {
            TextScore.text = "Score: " + score + "/" + goal;
        }
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
}
