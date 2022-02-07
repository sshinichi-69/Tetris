using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    GameObject NextTetromino;
    // Start is called before the first frame update
    void Start()
    {
        NextTetromino = Tetrominoes[Random.Range(0, Tetrominoes.Length)];
        NewTetromino();
    }

    public void NewTetromino()
    {
        Instantiate(NextTetromino);
        int i = Random.Range(0, Tetrominoes.Length);
        NextTetromino = Tetrominoes[i];
        FindObjectOfType<NextTetromino>().ShowNextTetromino(i);
    }
}
