using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Set_Level : MonoBehaviour
{
    int scene;
    int height = 20;
    int width = 11;
    public GameObject Block;
    public GameObject Brick1;
    public GameObject HorizontalBlock;
    // color: 0 - blue; 1 - purple; 2 - orange; 3 - green
    Color[] color;
    // Start is called before the first frame update
    void Start()
    {
        Color blue = new Color(0, 0, 255);
        Color green = new Color(0, 255, 0);
        Color orange = new Color(255, 127, 0);
        Color purple = new Color(127, 0, 255);
        scene = SceneManager.GetActiveScene().buildIndex;
        switch (scene)
        {
            case 5: case 6: case 7: case 8:
                color = new Color[4] { blue, green, orange, purple };
                break;
        }
    }

    public void setBlock(ref Transform[,] grid, int i, int j, GameObject obj, bool generate)
    {
        int k = Random.Range(0, 2);
        if (generate)
        {
            k = 1;
        }
        if (k == 1)
        {
            float x = i - 17.5f;
            float y = j - height / 2 + 0.5f;
            GameObject block = Instantiate(obj, new Vector3(x, y, 0), Quaternion.identity);
            grid[i, j] = block.transform;
        }
    }

    public Color getColor()
    {
        int i = Random.Range(0, color.Length);
        return color[i];
    }

    public void setGrid(ref Transform[,] grid, ref TheGrid.Link[] gridLink, ref string goal)
    {
        string name = (scene - 4).ToString();
        string address = @"D:\Unity Files\Tetris\Assets\Data\" + name + ".txt";
        string[] lines = System.IO.File.ReadAllLines(address);
        int counter = 0;
        int gridWidth, gridHeight = -1;
        int xStart = -1;
        int nLink = 0;
        foreach (string line in lines)
        {
            if (counter == 0)
            {
                string[] substr = line.Split(' ');
                gridWidth = int.Parse(substr[0]);
                gridHeight = int.Parse(substr[1]);
                xStart = (width - 1) / 2 - gridWidth / 2;
            }
            else if (counter <= gridHeight)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i])
                    {
                        case 'o':
                            setBlock(ref grid, xStart + i, gridHeight - counter, Block, false);
                            break;
                        case '1':
                            setBlock(ref grid, xStart + i, gridHeight - counter, Brick1, true);
                            break;
                        case '-':
                            setBlock(ref grid, xStart + i, gridHeight - counter, HorizontalBlock, true);
                            break;
                    }
                }
            }
            else if (counter == gridHeight + 1)
            {
                nLink = int.Parse(line);
                gridLink = new TheGrid.Link[nLink];
            }
            else if (counter < gridHeight + nLink + 2)
            {
                gridLink[counter - gridHeight - 2] = new TheGrid.Link();
                string[] substr = line.Split(' ');
                int[] headList = new int[4];
                for (int i = 0; i + 3 < substr.Length; i += 2)
                {
                    for (int j = 0; j < headList.Length; j++)
                    {
                        headList[j] = int.Parse(substr[i + j]);
                    }
                    if (headList[0] == headList[2])
                    {
                        if (headList[1] < headList[3])
                        {
                            for (int j = headList[1]; j <= headList[3]; j++)
                            {
                                gridLink[counter - gridHeight - 2].addNode(headList[0], j);
                            }
                        }
                        else
                        {
                            for (int j = headList[1]; j >= headList[3]; j--)
                            {
                                gridLink[counter - gridHeight - 2].addNode(headList[0], j);
                            }
                        }
                    }
                    else
                    {
                        if (headList[0] < headList[2])
                        {
                            for (int j = headList[0]; j <= headList[2]; j++)
                            {
                                gridLink[counter - gridHeight - 2].addNode(j, headList[1]);
                            }
                        }
                        else
                        {
                            for (int j = headList[0]; j >= headList[2]; j--)
                            {
                                gridLink[counter - gridHeight - 2].addNode(j, headList[1]);
                            }
                        }
                    }
                }
            }
            else
            {
                goal = line;
            }
            counter++;
        }
    }
}
