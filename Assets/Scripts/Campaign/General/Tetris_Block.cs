using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris_Block : MonoBehaviour
{
    private float previousTime;
    public float fallTime = 1;
    public float downTime = 0.01f;
    public Vector3 rotationPoint;
    public static int height = 20;
    public static int width = 11;
    int angle = 0;
    bool pause = false;
    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<Set_Data>().start)
        {
            FindObjectOfType<Set_Data>().BeStarted();
        }
        fallTime = FindObjectOfType<TheGrid>().fallTime;
        StartCoroutine(setColor());
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
                    FindObjectOfType<TheGrid>().CheckForLines();
                    StartCoroutine(NewTetromino());
                }
                enabled = false;
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
            FindObjectOfType<Set_Data>().SetPause(pause);
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
                if (!FindObjectOfType<TheGrid>().isNull(roundedX, roundedY))
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
                FindObjectOfType<Set_Data>().SetGameOver();
                return false;
            }
            FindObjectOfType<TheGrid>().SetGrid(roundedX, roundedY, children);
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
                if (!FindObjectOfType<TheGrid>().isNull(roundedX, roundedY))
                {
                    transform.position = pos;
                    return false;
                }
            }
        }
        return true;
    }

    IEnumerator NewTetromino()
    {
        yield return new WaitForSeconds(1.01f);
        if (!FindObjectOfType<TheGrid>().isWin)
        {
            FindObjectOfType<SpawnTetromino>().NewTetromino();
        }
    }

    IEnumerator setColor()
    {
        yield return new WaitForSeconds(0.01f);
        Color color = FindObjectOfType<Set_Level>().getColor();
        foreach (Transform children in transform)
        {
            children.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
