using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    private float previousTime;
    public float fallTime = 1f;
    public float downTime = 0.01f;
    public static int height = 20;
    public static int width = 11;
    public Vector3 rotationPoint;
    private static Transform[,] grid = new Transform[width, height];
    int angle = 0;
    static int level;
    static int speed;
    static int score;
    static int goal;
    bool pause = false;
    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<SetData>().start)
        {
            FindObjectOfType<SetData>().BeStarted();
            level = FindObjectOfType<GetData>().level;
            CreateLevel(level);
            speed = FindObjectOfType<GetData>().speed;
            score = 0;
            if (speed == 10)
            {
                goal = -1;
            }
            else
            {
                goal = 5 * speed + 15;
            }
            FindObjectOfType<SetData>().SetLevel(level);
            FindObjectOfType<SetData>().SetSpeed(speed);
            FindObjectOfType<SetData>().SetScore(score, goal);
        }
        fallTime = 1f - 0.1f * (speed - 1);
    }

    // Update is called once per frame
    void Update()
    {
        // Left right
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) && !pause)
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        else if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) && !pause)
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        // Up
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && !pause)
        {
            if (gameObject.tag == "Tetromino1")
            {
                if (angle % 2 == 0)
                {
                    RotateClockwise();
                }
                else
                {
                    RotateCounterClockwise();
                }
            }
            else
            {
                RotateClockwise();
            }
        }

        // Fall down
        if (Time.time - previousTime > (((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !pause) ? downTime : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position += new Vector3(0, 1, 0);
                if (AddToGrid())
                {
                    CheckForLines();
                    enabled = false;
                    FindObjectOfType<SpawnTetromino>().NewTetromino();
                }
                else
                {
                    CheckForLines();
                    enabled = false;
                }
            }
            previousTime = Time.time;
        }
        // pause
        if (Input.GetKeyDown(KeyCode.P)) {
            if (pause)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
            pause = !pause;
            FindObjectOfType<SetData>().SetPause(pause);
        }
    }

    void CheckForLines()
    {
        for (int j = height - 1; j >= 0; j--)
        {
            if (HasLine(j))
            {
                DeleteLine(j);
                RowDown(j);
                AddScore();
            }
        }
    }

    bool HasLine(int j)
    {
        for (int i = 0; i < width; i++)
        {
            if (grid[i, j] == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int j)
    {
        for (int i = 0; i < width; i++)
        {
            Destroy(grid[i, j].gameObject);
            grid[i, j] = null;
        }
    }

    void RowDown(int j)
    {
        for (int y = j; y < height; y++)
        {
            for (int i = 0; i < width; i++)
            {
                if (grid[i, y] != null)
                {
                    grid[i, y - 1] = grid[i, y];
                    grid[i, y] = null;
                    grid[i, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x + 17.5f);
            int roundedY = Mathf.RoundToInt(children.transform.position.y + height / 2 - 0.5f);
            if (roundedX < 0 || roundedX >= width || roundedY < 0)
            {
                return false;
            }
            if (roundedY < height)
            {
                if (grid[roundedX, roundedY] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void RotateClockwise()
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        angle++;
        if (!ValidRotate(transform.position))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            angle--;
        }
    }

    void RotateCounterClockwise()
    {
        transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        angle++;
        if (!ValidRotate(transform.position))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            angle--;
        }
    }

    bool AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x + 17.5f);
            int roundedY = Mathf.RoundToInt(children.transform.position.y + height / 2 - 0.5f);
            if (roundedY >= height)
            {
                FindObjectOfType<SetData>().SetGameOver();
                return false;
            }
            grid[roundedX, roundedY] = children;
        }
        return true;
    }

    bool ValidRotate(Vector3 pos)
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x + 17.5f);
            int roundedY = Mathf.RoundToInt(children.transform.position.y + height / 2 - 0.5f);
            if (roundedY < 0)
            {
                return false;
            }
            if (roundedX < 0)
            {
                transform.position += new Vector3(1, 0, 0);
                return ValidRotate(pos);
            }
            if (roundedX >= width)
            {
                transform.position += new Vector3(-1, 0, 0);
                return ValidRotate(pos);
            }
            if (roundedY < height)
            {
                if (grid[roundedX, roundedY] != null)
                {
                    transform.position = pos;
                    return false;
                }
            }
        }
        return true;
    }

    void CreateLevel(int level)
    {
        for (int j = 0; j < level - 1; j++)
        {
            for (int i = 0; i < width; i++)
            {
                int create = Random.Range(0, 2);
                if (create == 1)
                {
                    float x = i - 17.5f;
                    float y = j - height / 2 + 0.5f;
                    GameObject block = Instantiate(FindObjectOfType<SetData>().GetBlock(), new Vector3(x, y, 0), Quaternion.identity);
                    grid[i, j] = block.transform;
                }
            }
        }
    }

    void AddScore()
    {
        score++;
        if (score == goal)
        {
            speed++;
            score = 0;
            if (speed == 10)
            {
                goal = -1;
            }
            else
            {
                goal += 5;
            }
            FindObjectOfType<SetData>().SetSpeed(speed);
        }
        FindObjectOfType<SetData>().SetScore(score, goal);
    }
}
