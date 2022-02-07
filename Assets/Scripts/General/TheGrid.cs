using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheGrid : MonoBehaviour
{
    // class
    public class Node
    {
        public int x;
        public int y;
        public Node next;
        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
            next = null;
        }
    }
    public class Link
    {
        Node head;
        Node tail;
        public Link()
        {
            head = null;
            tail = null;
        }
        public void addNode(int x, int y)
        {
            if (head == null)
            {
                head = new Node(x, y);
                tail = head;
            }
            else
            {
                Node node = new Node(x, y);
                tail.next = node;
                tail = node;
            }
        }
    }
    // data
    public int height = 20;
    public int width = 11;
    private Transform[,] grid;
    Link[] gridLink;
    public int score;
    public int speed = 1;
    int blockLeft;
    public int maxBlockLeft;
    int scene;
    public bool isWin = false;
    Color blue = new Color(0, 0, 255);
    int combo = 0;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene().buildIndex;
        grid = new Transform[width, height];
        if (GetComponent<Set_Data>().start)
        {
            GetComponent<Set_Data>().SetScore(score);
            GetComponent<Set_Data>().setTetroLeft(maxBlockLeft);
        }
        blockLeft = maxBlockLeft;
        StartCoroutine(setLevel());
    }

    IEnumerator setLevel()
    {
        yield return new WaitForSeconds(0.01f);
        GetComponent<Set_Level>().setGrid(ref grid);
        // GetComponent<Set_Level>().setLink(ref gridLink);
    }

    public void CheckForLines()
    {
        for (int j = height - 1; j >= 0; j--)
        {
            if (HasLine(j))
            {
                switch (scene)
                {
                    case 5:
                        AddScore(j);
                        break;
                    case 7:
                        combo++;
                        AddScore(j);
                        break;
                }
                DeleteLine(j);
                StartCoroutine(_RowDown(j));
                switch (scene)
                {
                    case 6:
                        StartCoroutine(AddScore());
                        break;
                }
            }
        }
        combo = 0;
        setBlockLeft();
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
        StartCoroutine(DeleteBlock(0, width - 1, j));
    }

    IEnumerator DeleteBlock(int a, int b, int j)
    {
        yield return new WaitForSeconds(0.1f);
        // a
        if (grid[a, j] != null)
        {
            string tag = grid[a, j].tag;
            if (tag != "Wall")
            {
                Destroy(grid[a, j].gameObject);
                grid[a, j] = null;
            }
            if (j > 0)
            {
                if (grid[a, j - 1] != null && tag == "Untagged")
                {
                    if (grid[a, j - 1].CompareTag("BrickA"))
                    {
                        Destroy(grid[a, j - 1].gameObject);
                        grid[a, j - 1] = null;
                    }
                }
            }
            if (j < height - 1)
            {
                if (grid[a, j + 1] != null && tag == "Untagged")
                {
                    if (grid[a, j + 1].CompareTag("BrickA"))
                    {
                        Destroy(grid[a, j + 1].gameObject);
                        grid[a, j + 1] = null;
                    }
                }
            }
            a++;
        }
        // b
        if (grid[b, j] != null)
        {
            string tag = grid[b, j].tag;
            if (tag != "Wall")
            {
                Destroy(grid[b, j].gameObject);
                grid[b, j] = null;
            }
            if (j > 0)
            {
                if (grid[b, j - 1] != null && tag == "Untagged")
                {
                    if (grid[b, j - 1].CompareTag("BrickA"))
                    {
                        Destroy(grid[b, j - 1].gameObject);
                        grid[b, j - 1] = null;
                    }
                }
            }
            if (j < height - 1)
            {
                if (grid[b, j + 1] != null && tag == "Untagged")
                {
                    if (grid[b, j + 1].CompareTag("BrickA"))
                    {
                        Destroy(grid[b, j + 1].gameObject);
                        grid[b, j + 1] = null;
                    }
                }
            }
            b--;
        }
        if (a <= b)
        {
            StartCoroutine(DeleteBlock(a, b, j));
        }
    }

    IEnumerator _RowDown(int j)
    {
        yield return new WaitForSeconds(1);
        RowDown(j);
    }

    void RowDown(int j)
    {
        for (int y = j; y < height - 1; y++)
        {
            for (int i = 0; i < width; i++)
            {
                if (grid[i, y + 1] != null)
                {
                    int[] nextBlockPos = {0, 0};
                    if (!grid[nextBlockPos[0], nextBlockPos[1]].CompareTag("Wall")
                        && !grid[nextBlockPos[0], nextBlockPos[1]].CompareTag("BrickA")
                        && grid[i, y] == null)
                    {
                        grid[i, y] = grid[nextBlockPos[0], nextBlockPos[1]];
                        grid[nextBlockPos[0], nextBlockPos[1]] = null;
                        //grid[i, y].transform.position += getNextVector(i, y);
                    }
                }
            }
        }
    }

    void AddScore(int j)
    {
        switch (scene)
        {
            case 5:
                for (int i = 0; i < width; i++)
                {
                    if (grid[i, j].gameObject.GetComponent<SpriteRenderer>().color == blue)
                    {
                        if (score > 0)
                        {
                            score--;
                            GetComponent<Set_Data>().SetScore(score);
                            if (score == 0)
                            {
                                GetComponent<Set_Data>().SetVictory();
                                isWin = true;
                                return;
                            }
                        }
                    }
                }
                break;
            case 7:
                if (combo == 2)
                {
                    combo = 0;
                    if (score > 0)
                    {
                        score--;
                        GetComponent<Set_Data>().SetScore(score);
                        if (score == 0)
                        {
                            GetComponent<Set_Data>().SetVictory();
                            isWin = true;
                            return;
                        }
                    }
                }
                break;
        }
    }

    IEnumerator AddScore()
    {
        yield return new WaitForSeconds(1);
        switch (scene)
        {
            case 6:
                score = GameObject.FindGameObjectsWithTag("BrickA").Length;
                GetComponent<Set_Data>().SetScore(score);
                if (score == 0)
                {
                    GetComponent<Set_Data>().SetVictory();
                    isWin = true;
                }
                break;
        }
    }

    public bool isNull(int i, int j)
    {
        if (grid[i, j] == null)
        {
            return true;
        }
        return false;
    }

    public void SetGrid(int i, int j, Transform t)
    {
        grid[i, j] = t.transform;
    }

    public void setBlockLeft()
    {
        blockLeft--;
        if (blockLeft == 0)
        {
            if (speed < 10)
            {
                speed++;
                GetComponent<Set_Data>().setSpeed(speed);
            }
            blockLeft = maxBlockLeft;
        }
        GetComponent<Set_Data>().setTetroLeft(blockLeft);
    }
}
