using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTetromino : MonoBehaviour
{
    public Sprite[] NextTetrominoes;
    public void ShowNextTetromino(int i)
    {
        GetComponent<SpriteRenderer>().sprite = NextTetrominoes[i];
    }
}
