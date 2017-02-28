using System;
using System.Collections.Generic;
using UnityEngine;

public class HistoryController : MonoBehaviour
{
    public DominoController spinner;
    public List<DominoController> horizontalDominoes;
    public List<DominoController> verticalDominoes;

    void Start()
    {
        horizontalDominoes = new List<DominoController>(28);
        verticalDominoes = new List<DominoController>(28);
    }

    void Add(DominoController playedDomino, DominoController historyDomino)
    {
        if (playedDomino == null)
        {
            throw new ArgumentNullException("playedDomino");
        }
        if (historyDomino == null)
        {
            playedDomino.transform.position = new Vector3(0, 0);
//            // Vertical
//            if (playedDomino.values.value1 == playedDomino.values.value2)
//            {
//                playedDomino.transform.eulerAngles = new Vector3(0, 0, 0);
//            }
//            // Horizontal
//            else
//            {
//                playedDomino.transform.eulerAngles = new Vector3(0, 0, 90);
//            }
            return;
        }
        if (horizontalDominoes.Count > 0 && historyDomino.Equals(horizontalDominoes[0]))
        {
//            if (playedDomino.values.value1 == playedDomino.values.value2)
//            {
//                playedDomino.transform.eulerAngles = new Vector3(0, 0, 0);
//            }
//            else
//            {
//                playedDomino.transform.eulerAngles = new Vector3(0, 0, 90);
//            }
        }
        else if (horizontalDominoes.Count > 1 && historyDomino.Equals(horizontalDominoes[horizontalDominoes.Count - 1]))
        {
        }
        else if (verticalDominoes.Count > 0 && historyDomino.Equals(verticalDominoes[0]))
        {
        }
        else if (verticalDominoes.Count > 1 && historyDomino.Equals(verticalDominoes[verticalDominoes.Count - 1]))
        {
        }
        else
        {
            throw new ArgumentException("No valid historyDomino", "historyDomino");
        }
    }

    void Reset()
    {
        spinner = null;
        horizontalDominoes.Clear();
        verticalDominoes.Clear();
    }
}
