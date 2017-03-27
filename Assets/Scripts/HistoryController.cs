using System;
using System.Collections.Generic;
using UnityEngine;

public class HistoryController : MonoBehaviour
{
    float interval = 0.2f;
    float dominoeScale = 1.5f;
    float startPositionX = 0.0f;
    float startPositionY = 0.0f;
    public DominoController spinner;
    public List<DominoController> horizontalDominoes;
    public List<DominoController> verticalDominoes;
    bool isSpinnerPlaced;
    DominoController center;
    PutPosition putPosition;
    int numberLeft, numberRight, numberUp, numberDown;
    float generalHorizontalOffset = 0.0f;
    float generalVerticalOffset = 0.0f;

    enum PutPosition
    {
        Left,
        Right,
        Up,
        Down,
        NA
    }

    void Start()
    {
        horizontalDominoes = new List<DominoController>(28);
        verticalDominoes = new List<DominoController>(28);
        isSpinnerPlaced = false;
        numberLeft = 0;
        numberRight = 0;
        numberUp = 0;
        numberDown = 0;
    }

    void SetPlayedDominoPosition(DominoController playedDomino)
    {
        playedDomino.transform.position = new Vector3(startPositionX + generalHorizontalOffset + playedDomino.offsetHorizontal, startPositionY + generalVerticalOffset + playedDomino.offsetVertical);
        playedDomino.transform.localScale = new Vector3(dominoeScale, dominoeScale, 0);
    }

    public void Add(DominoController playedDomino, DominoController historyDomino)
    {
        putPosition = PutPosition.NA;
        if (playedDomino == null)
        {
            throw new ArgumentNullException("playedDomino");
        }

        if (historyDomino == null)
        {
            center = playedDomino;
            center.offsetHorizontal = 0.0f;
            center.offsetVertical = 0.0f;
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
        if (playedDomino.direction == DominoController.Direction.Horizontal)
        {
//            Debug.Log("p horizontal");
        }
        if (playedDomino.direction == DominoController.Direction.Vertical)
        {
//            Debug.Log("p vertical");
        }
        if (historyDomino.direction == DominoController.Direction.Horizontal)
        {
//            Debug.Log("h horizontal");
        }
        if (historyDomino.direction == DominoController.Direction.Vertical)
        {
//            Debug.Log("h vertical");
        }

//         if historyDomino not null

        if (horizontalDominoes.Count > 0 && (historyDomino.Equals(horizontalDominoes[0]) || historyDomino.Equals(horizontalDominoes[horizontalDominoes.Count - 1])))
        {
            if (playedDomino.direction == DominoController.Direction.Horizontal && historyDomino.direction == DominoController.Direction.Horizontal)
            {
                if (playedDomino.rightValue == historyDomino.leftValue)
                {
                    putPosition = PutPosition.Left;
                }
                if (playedDomino.leftValue == historyDomino.rightValue)
                {
                    putPosition = PutPosition.Right;
                }
            }
            if (playedDomino.direction == DominoController.Direction.Horizontal && historyDomino.direction == DominoController.Direction.Vertical)
            {
                if (playedDomino.rightValue == historyDomino.upperValue)
                {
                    putPosition = PutPosition.Left;
                }
                if (playedDomino.leftValue == historyDomino.upperValue)
                {
                    putPosition = PutPosition.Right;
                }
            }
            if (playedDomino.direction == DominoController.Direction.Vertical && historyDomino.direction == DominoController.Direction.Horizontal)
            {
                if (playedDomino.upperValue == historyDomino.leftValue)
                {
                    putPosition = PutPosition.Left;
                }
                if (playedDomino.upperValue == historyDomino.rightValue)
                {
                    putPosition = PutPosition.Right;
                }
            }                       
        }
        else if (verticalDominoes.Count > 0 && (historyDomino.Equals(verticalDominoes[0]) || historyDomino.Equals(verticalDominoes[verticalDominoes.Count - 1])))
        //else
        {
            if (playedDomino.direction == DominoController.Direction.Vertical && historyDomino.direction == DominoController.Direction.Vertical)
            {
                if (playedDomino.lowerValue == historyDomino.upperValue)
                {
                    putPosition = PutPosition.Up;
                }
                if (playedDomino.upperValue == historyDomino.lowerValue)
                {
                    putPosition = PutPosition.Down;
                }
            }
            if (playedDomino.direction == DominoController.Direction.Horizontal && historyDomino.direction == DominoController.Direction.Vertical)
            {
                if (playedDomino.leftValue == historyDomino.upperValue)
                {
                    putPosition = PutPosition.Up;
                }
                if (playedDomino.leftValue == historyDomino.lowerValue)
                {
                    putPosition = PutPosition.Down;
                }
            }
            if (playedDomino.direction == DominoController.Direction.Vertical && historyDomino.direction == DominoController.Direction.Horizontal)
            {
                if (playedDomino.lowerValue == historyDomino.leftValue)
                {
                    putPosition = PutPosition.Up;
                }
                if (playedDomino.upperValue == historyDomino.leftValue)
                {
                    putPosition = PutPosition.Down;
                }
            }                
        }            
        
        if (putPosition == PutPosition.Left)
        {
//            Debug.Log("place left");
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
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal - dominoeScale * (interval + Constants.dominoHeight);
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal - (interval + 0.5f * dominoeScale * (Constants.dominoHeight + Constants.dominoWidth));
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }
//            SetPlayedDominoPosition(playedDomino);
            numberLeft++;
        }
        else if (putPosition == PutPosition.Right)
        {
//            Debug.Log("place right");
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
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal + dominoeScale * (interval + Constants.dominoHeight);
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal + (interval + 0.5f * dominoeScale * (Constants.dominoHeight + Constants.dominoWidth));
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }
//            SetPlayedDominoPosition(playedDomino);
            numberRight++;
        }
        //VERTICAL
        else if (putPosition == PutPosition.Up)
        {
//            Debug.Log("place up");
            verticalDominoes.Insert(0, playedDomino);

            // both vertical
            if (historyDomino.direction == playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical + (interval + dominoeScale * Constants.dominoHeight);
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical + (interval + 0.5f * dominoeScale * (Constants.dominoHeight + Constants.dominoWidth));
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }
//            SetPlayedDominoPosition(playedDomino);
            numberUp++;
        }
        else if (putPosition == PutPosition.Down)
        {
//            Debug.Log("place down");
            verticalDominoes.Add(playedDomino);

            // both vertical
            if (historyDomino.direction == playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical - (interval + dominoeScale * Constants.dominoHeight);
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical - (interval + 0.5f * dominoeScale * (Constants.dominoHeight + Constants.dominoWidth));
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }
//            SetPlayedDominoPosition(playedDomino);
            numberDown++;
        }
        else
        {
            throw new ArgumentException("No valid historyDomino", "historyDomino");
        }
        // Set Dominoes Position
        generalHorizontalOffset = (float)(numberLeft - numberRight) * 0.5f * dominoeScale * (Constants.dominoHeight + interval);
        generalVerticalOffset = (float)(numberDown - numberUp) * 0.5f * dominoeScale * (Constants.dominoHeight + interval);
        foreach (DominoController domino in horizontalDominoes)
        {
            SetPlayedDominoPosition(domino);
        }
        foreach (DominoController domino in verticalDominoes)
        {
            SetPlayedDominoPosition(domino);
        }
    }

    public void ResetHand()
    {
        spinner = null;
        center = null;
        foreach (DominoController domino in horizontalDominoes)
        {
            Destroy(domino.gameObject);
        }
        foreach (DominoController domino in verticalDominoes)
        {
            Destroy(domino.gameObject);
        }
        horizontalDominoes.Clear();
        verticalDominoes.Clear();
        isSpinnerPlaced = false;
        putPosition = PutPosition.NA;
        generalHorizontalOffset = 0;
        generalVerticalOffset = 0;
        numberLeft = 0;
        numberRight = 0;
        numberUp = 0;
        numberDown = 0;
    }
}
