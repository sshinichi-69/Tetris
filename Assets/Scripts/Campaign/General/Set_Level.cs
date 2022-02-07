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
            case 5: case 6: case 7:
                color = new Color[4] { blue, green, orange, purple };
                break;
        }
    }

    public void setGrid(ref Transform[,] grid)
    {
        switch (scene)
        {
            case 5:
                for (int i = 3; i < 8; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        setBlock(ref grid, i, j, Block, false);
                    }
                }
                break;
            case 6:
                for (int i = 2; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (j == 0)
                        {
                            if (i > 2 && i < 7)
                            {
                                setBlock(ref grid, i, j, Brick1, true);
                            }
                            else
                            {
                                setBlock(ref grid, i, j, Block, false);
                            }
                        }
                        else if (j > 4)
                        {
                            setBlock(ref grid, i, j, Block, false);
                        }
                        else
                        {
                            setBlock(ref grid, i, j, Brick1, true);
                        }
                    }
                }
                break;
            case 7:
                for (int i = 2; i < 9; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        if (i == 2)
                        {
                            if (j == 2)
                            {
                                setBlock(ref grid, i, j, Brick1, true);
                            }
                            else if (j == 1 || j == 3)
                            {
                                setBlock(ref grid, i, j, Block, false);
                            }
                        }
                        else if (i == 5)
                        {
                            if (j == 0)
                            {
                                setBlock(ref grid, i, j, Block, false);
                            }
                            else
                            {
                                setBlock(ref grid, i, j, Brick1, true);
                            }
                        }
                        else if (i == 8)
                        {
                            if (j > 0 && j < 4)
                            {
                                setBlock(ref grid, i, j, Block, false);
                            }
                        }
                        else if (i == 3 || i == 7)
                        {
                            if (j == 1)
                            {
                                setBlock(ref grid, i, j, Brick1, true);
                            }
                            else
                            {
                                setBlock(ref grid, i, j, Block, false);
                            }
                        }
                        else
                        {
                            if (j == 1 || j == 2 || j == 5)
                            {
                                setBlock(ref grid, i, j, Block, false);
                            }
                            else
                            {
                                setBlock(ref grid, i, j, Brick1, true);
                            }
                        }
                    }
                }
                break;
        }
    }

    public void setLink(ref TheGrid.Link[] gridLink)
    {
        switch (scene)
        {
            case 7:
                gridLink = new TheGrid.Link[width];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (!((i == 2 || i == 8) && j == 0))
                        {
                            gridLink[i].addNode(i, j);
                        }
                    }
                }
                break;
            default:
                gridLink = new TheGrid.Link[width];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        gridLink[i].addNode(i, j);
                    }
                }
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
}
