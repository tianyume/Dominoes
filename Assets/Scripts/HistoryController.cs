using System;
using System.Collections.Generic;
using UnityEngine;

public class HistoryController : MonoBehaviour
{
    public float interval;
    public DominoController spinner;
    public List<DominoController> horizontalDominoes;
    public List<DominoController> verticalDominoes;
    bool isSpinnerPlaced;
    DominoController center;

    void Start()
    {
        interval = 0.2f;
        horizontalDominoes = new List<DominoController>(28);
        verticalDominoes = new List<DominoController>(28);
        isSpinnerPlaced = false;
    }

    void SetPlayedDominoPosition(DominoController playedDomino)
    {
        playedDomino.transform.position = new Vector3(center.offsetHorizontal+playedDomino.offsetHorizontal, center.offsetVertical+playedDomino.offsetVertical);
    }

    public void Add(DominoController playedDomino, DominoController historyDomino)
    {
        if (playedDomino == null)
        {
            throw new ArgumentNullException("playedDomino");
        }
        // tofix: there are two cases here
        // SetLeftRightValues(playedDomino.upperValue, playedDomino.lowerValue);
        // or SetLeftRightValues(playedDomino.lowerValue, playedDomino.upperValue);
        if (playedDomino.upperValue != playedDomino.lowerValue)
        {
            playedDomino.SetLeftRightValues(playedDomino.upperValue, playedDomino.lowerValue);
        }

        if (historyDomino == null)
        {
            center = playedDomino;
            center.offsetHorizontal = 0f;
            center.offsetVertical = 0f;
            // Set Spinner                
            if (playedDomino.direction == DominoController.Direction.Vertical)
            {
                isSpinnerPlaced = true;
                spinner = playedDomino;
                horizontalDominoes.Add(playedDomino);
                verticalDominoes.Add(playedDomino);
            }
            else
            {
                horizontalDominoes.Add(playedDomino);
            }
            SetPlayedDominoPosition(playedDomino);
            return;
        }

        // if historyDomino not null

        if (horizontalDominoes.Count > 0 && historyDomino.Equals(horizontalDominoes[0]))
        {
            horizontalDominoes.Insert(0, playedDomino);
            // set spinner
            if (!isSpinnerPlaced && playedDomino.direction == DominoController.Direction.Vertical)
            {
                spinner = playedDomino;
                verticalDominoes.Add(playedDomino);
                isSpinnerPlaced = true;
            }
            // calculate offset
            // both horizontal
            if (historyDomino.direction == playedDomino.direction)
            {
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal - (interval + Constants.dominoHeight);
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal - (interval + 0.5f * (Constants.dominoHeight+Constants.dominoWidth));
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }

            SetPlayedDominoPosition(playedDomino);
        }


        else if (horizontalDominoes.Count > 1 && historyDomino.Equals(horizontalDominoes[horizontalDominoes.Count - 1]))
        {
            horizontalDominoes.Add(playedDomino);
            // set spinner
            if (!isSpinnerPlaced && playedDomino.direction == DominoController.Direction.Vertical)
            {
                spinner = playedDomino;
                verticalDominoes.Add(playedDomino);
                isSpinnerPlaced = true;
            }
            // calculate offset
            // both horizontal
            if (historyDomino.direction == playedDomino.direction)
            {
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal + (interval + Constants.dominoHeight);
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal + (interval + 0.5f * (Constants.dominoHeight+Constants.dominoWidth));
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }

            SetPlayedDominoPosition(playedDomino);
        }


        //VERTICAL

        else if (verticalDominoes.Count > 0 && historyDomino.Equals(verticalDominoes[0]))
        {
            verticalDominoes.Insert(0, playedDomino);

            // both vertical
            if (historyDomino.direction == playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical + (interval + Constants.dominoHeight);
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical + (interval + 0.5f*(Constants.dominoHeight+Constants.dominoWidth));
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }

            SetPlayedDominoPosition(playedDomino);
        }


        else if (verticalDominoes.Count > 1 && historyDomino.Equals(verticalDominoes[verticalDominoes.Count - 1]))
        {
            verticalDominoes.Add(playedDomino);

            // both vertical
            if (historyDomino.direction == playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical - (interval + Constants.dominoHeight);
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical - (interval + 0.5f*(Constants.dominoHeight+Constants.dominoWidth));
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }

            SetPlayedDominoPosition(playedDomino);
        }


        else
        {
            throw new ArgumentException("No valid historyDomino", "historyDomino");
        }
    }

    public void Reset()
    {
        spinner = null;
        horizontalDominoes.Clear();
        verticalDominoes.Clear();
        isSpinnerPlaced = false;
        center = null;
    }
}
