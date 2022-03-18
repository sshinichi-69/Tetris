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
        public Node head;
        public Node tail;
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
    string goal;
    public int score;
    public int speed = 1;
    public float fallTime = 1f;
    int blockLeft;
    public int maxBlockLeft;
    public bool isWin = false;
    Color blue = new Color(0, 0, 255);
    int combo = 0;
    // Start is called before the first frame update
    void Start()
    {
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
        GetComponent<Set_Level>().setGrid(ref grid, ref gridLink, ref goal);
    }

    public void CheckForLines()
    {
        for (int j = height - 1; j >= 0; j--)
        {
            if (HasLine(j))
            {
                switch (goal)
                {
                    case "blue":
                        AddScore(j);
                        break;
                    case "x2":
                        combo++;
                        AddScore(j);
                        break;
                }
                DeleteLine(j);
                StartCoroutine(_RowDown(j));
                switch (goal)
                {
                    case "brick1":
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
            // Destroy tetromino or brick in the row
            if (tag != "Wall")
            {
                Destroy(grid[a, j].gameObject);
                if (tag == "Horizontal")
                {
                    StartCoroutine(HorizontalBonus(a - 1, a + 1, j));
                }
                grid[a, j] = null;
            }
            // Destroy brick which is adjacency the destroyed tetromino
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
        }
        a++;
        // b
        if (grid[b, j] != null)
        {
            string tag = grid[b, j].tag;
            // Destroy tetromino or brick in the row
            if (tag != "Wall")
            {
                Destroy(grid[b, j].gameObject);
                grid[b, j] = null;
            }
            // Destroy brick which is adjacency the destroyed tetromino
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
        }
        b--;
        if (a <= b)
        {
            StartCoroutine(DeleteBlock(a, b, j));
        }
    }

    IEnumerator HorizontalBonus(int a, int b, int j)
    {
        yield return new WaitForSeconds(0.1f);
        if (a > -1)
        {
            if (grid[a, j] != null)
            {
                string tag = grid[a, j].tag;
                if (tag != "Wall")
                {
                    Destroy(grid[a, j].gameObject);
                    if (tag == "Horizontal")
                    {
                        StartCoroutine(HorizontalBonus(a - 1, a + 1, j));
                    }
                    grid[a, j] = null;
                }
            }
        }
        a--;
        if (b < width)
        {
            if (grid[b, j] != null)
            {
                string tag = grid[b, j].tag;
                if (tag != "Wall")
                {
                    Destroy(grid[b, j].gameObject);
                    if (tag == "Horizontal")
                    {
                        StartCoroutine(HorizontalBonus(b - 1, b + 1, j));
                    }
                    grid[b, j] = null;
                }
            }
        }
        b++;
        if (a > -1 || b < width)
        {
            StartCoroutine(HorizontalBonus(a - 1, b + 1, j));
        }
    }

    IEnumerator _RowDown(int j)
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < gridLink.Length; i++)
        {
            RowDown(gridLink[i], j);
        }
    }

    void RowDown(Link gl, int j)
    {
        Node cur = gl.head;
        Node next = gl.head;
        while (cur != null)
        {
            while (next != null)
            {
                if (next.y != j)
                {
                    break;
                }
                next = next.next;
            }
            if (cur != next)
            {
                if (next == null)
                {
                    grid[cur.x, cur.y] = null;
                }
                else
                {
                    if (grid[cur.x, cur.y] != null)
                    {
                        break;
                    }
                    if (grid[next.x, next.y] != null)
                    {
                        if (grid[next.x, next.y].CompareTag("Wall")
                            || grid[next.x, next.y].CompareTag("BrickA"))
                        {
                            break;
                        }
                        grid[cur.x, cur.y] = grid[next.x, next.y];
                        grid[next.x, next.y] = null;
                        grid[cur.x, cur.y].transform.position += new Vector3(cur.x - next.x, cur.y - next.y, 0);
                    }
                }
            }
            cur = cur.next;
            if (next != null)
            {
                next = next.next;
            }
        }
    }

    void AddScore(int j)
    {
        if (goal == "blue")
        {
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
        }
        else if (goal == "x2")
        {
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
        }
    }

    IEnumerator AddScore()
    {
        yield return new WaitForSeconds(1);
        if (goal == "brick1") {
            score = GameObject.FindGameObjectsWithTag("BrickA").Length;
            GetComponent<Set_Data>().SetScore(score);
            if (score == 0)
            {
                GetComponent<Set_Data>().SetVictory();
                isWin = true;
            }
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
            speed++;
            fallTime -= Mathf.Pow(10, -((speed - 1) / 10 + 1));
            GetComponent<Set_Data>().setSpeed(speed);
            blockLeft = maxBlockLeft;
        }
        GetComponent<Set_Data>().setTetroLeft(blockLeft);
    }
}
