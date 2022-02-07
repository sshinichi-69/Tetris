using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetData : MonoBehaviour
{
    public int level;
    public int speed;
    public Text LevelText;
    public Text SpeedText;
    public GameObject Gameplay;
    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        speed = 1;
    }

    public void IncreaseLevel()
    {
        if (level == 10)
        {
            level = 1;
        }
        else
        {
            level++;
        }
        LevelText.text = level.ToString();
    }

    public void DescreaseLevel()
    {
        if (level == 1)
        {
            level = 10;
        }
        else
        {
            level--;
        }
        LevelText.text = level.ToString();
    }

    public void IncreaseSpeed()
    {
        if (speed == 10)
        {
            speed = 1;
        }
        else
        {
            speed++;
        }
        SpeedText.text = speed.ToString();
    }

    public void DescreaseSpeed()
    {
        if (speed == 1)
        {
            speed = 10;
        }
        else
        {
            speed--;
        }
        SpeedText.text = speed.ToString();
    }

    public void Play()
    {
        Destroy(GameObject.Find("Menu"));
        Instantiate(Gameplay);
    }
}
