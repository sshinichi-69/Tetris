using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlScenes : MonoBehaviour
{
    public int BackLevel = 0;
    int nLevel = 4;
    public bool[] isComplete;
    // Start is called before the first frame update
    void Start()
    {
        isComplete = new bool[nLevel];
        for (int i = 0; i < nLevel; i++)
        {
            isComplete[i] = false;
        }
        LoadLevel();
        //SaveSystem.SaveData(this);
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            showPassLevel();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(BackLevel);
            }
        }
    }

    public void GoToScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void SaveLevel(int scene)
    {
        isComplete[scene - 5] = true;
        SaveSystem.SaveData(this);
    }

    public void LoadLevel()
    {
        CompleteData data = SaveSystem.LoadData(this);
        isComplete = data.isComplete;
    }

    void showPassLevel()
    {
        for (int i = 0; i < nLevel; i++)
        {
            if (isComplete[i])
            {
                name = "Level " + (i + 1).ToString();
                GameObject.Find(name).transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}
