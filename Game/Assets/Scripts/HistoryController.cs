using System;
using System.Collections.Generic;
using UnityEngine;

public class HistoryController : MonoBehaviour
{
    public float interval = 0.08f;
    public float dominoScale = 1.2f;
    public float startPositionX = 0.0f;
    public float startPositionY = 0.0f;
    public DominoController spinner;
    public List<DominoController> horizontalDominoes;
    public List<DominoController> verticalDominoes;
    public bool isSpinnerPlaced;
    public DominoController center;
    public PutPosition putPosition;
    public int numberLeft, numberRight, numberUp, numberDown;
    public float generalHorizontalOffset = 0.0f;
    public float generalVerticalOffset = 0.0f;
    public float upperBound = 3f, lowerBound = -3f;
    public float leftBound = -9f, rightBound = 15f;
    public Vector3 leftMost = Vector3.zero, rightMost = Vector3.zero;
    public Vector3 upMost = Vector3.zero, downMost = Vector3.zero;

    // store origin position and rotations
    public List<Vector3> horizontalPositions;
    public List<Vector3> verticalPositions;
    public List<Quaternion> horizontalRotations;
    public List<Quaternion> verticalRotations;

    public enum PutPosition
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
        horizontalPositions = new List<Vector3>(28);
        verticalPositions = new List<Vector3>(28);
        horizontalRotations = new List<Quaternion>(28);
        verticalRotations = new List<Quaternion>(28);
        isSpinnerPlaced = false;
        generalHorizontalOffset = 0;
        generalVerticalOffset = 0;
        numberLeft = 0;
        numberRight = 0;
        numberUp = 0;
        numberDown = 0;
    }

    // calculate dominoes' original position
    Vector3 OriginalPosition(DominoController playedDomino)
    {
        Vector3 temp = new Vector3(startPositionX + playedDomino.offsetHorizontal + generalHorizontalOffset, startPositionY + playedDomino.offsetVertical + generalVerticalOffset);
        playedDomino.transform.position = temp;
        playedDomino.transform.localScale = new Vector3(dominoScale, dominoScale, 0); 
//        if (temp.x < leftBound && leftMost == Vector3.zero && playedDomino.direction!=DominoController.Direction.Vertical)
//        {
//            leftMost = temp;
//        }
//        else if (temp.x > rightBound && rightMost == Vector3.zero && playedDomino.direction!=DominoController.Direction.Vertical)
//        {
//            rightMost = temp;
//        }
//        else if (temp.y < lowerBound && downMost == Vector3.zero && playedDomino.direction!=DominoController.Direction.Horizontal)
//        {
//            downMost = temp;
//        }
//        else if (temp.y > upperBound && upMost == Vector3.zero && playedDomino.direction!=DominoController.Direction.Horizontal)
//        {
//            upMost = temp;
//        }
        return temp;
    }

    void findBoundaryPosition()
    {
        int len = horizontalPositions.Count;
        for (int i = 0; i < len; i++)
        {
            if (horizontalPositions[i].x > rightBound && rightMost == Vector3.zero && horizontalDominoes[i].direction != DominoController.Direction.Vertical)
            {
                rightMost = horizontalPositions[i];
            }
            if (horizontalPositions[len - 1 - i].x < leftBound && leftMost == Vector3.zero && horizontalDominoes[len - 1 - i].direction != DominoController.Direction.Vertical)
            {
                leftMost = horizontalPositions[len - 1 - i];
            }
        }
        len = verticalPositions.Count;
        for (int i = 0; i < len; i++)
        {
            if (verticalPositions[i].y < lowerBound && downMost == Vector3.zero && verticalDominoes[i].direction != DominoController.Direction.Horizontal)
            {
                downMost = verticalPositions[i];
            }
            if (verticalPositions[len - 1 - i].y > upperBound && upMost == Vector3.zero && verticalDominoes[len - 1 - i].direction != DominoController.Direction.Horizontal)
            {
                upMost = verticalPositions[len - 1 - i];
            }

        }
    }

    void SetPlayedDominoPosition(DominoController playedDomino, Vector3 pos, Quaternion rot)
    {
        playedDomino.transform.position = pos;              
        playedDomino.transform.rotation = rot;
    }

    void SetRotateDomino(DominoController playedDomino, Vector3 pos)
    {
        Debug.Log("rotate pos: " + pos.x + " " + pos.y);
        Debug.Log(playedDomino.leftValue + " " + playedDomino.rightValue);
        playedDomino.transform.RotateAround(pos, new Vector3(0, 0, 1), -90f);
        Debug.Log("after rotate: " + playedDomino.transform.position.x + " " + playedDomino.transform.position.y);
    }

    void countPositionNumber(PutPosition putPosition)
    {
        if (putPosition == PutPosition.Left)
        {
            numberLeft++;
        }
        if (putPosition == PutPosition.Right)
        {
            numberRight++;
        }
        if (putPosition == PutPosition.Up)
        {
            numberUp++;
        }
        if (putPosition == PutPosition.Down)
        {
            numberDown++;
        }
        if (putPosition == PutPosition.NA)
        {
        }
    }

    void setGeneralOffset()
    {
        generalHorizontalOffset = (float)(numberLeft - numberRight) * 0.6f * dominoScale * (Constants.dominoHeight + interval) + 1.2f;
        //generalVerticalOffset = (float)(numberDown - numberUp) * 0.3f * dominoScale * (Constants.dominoHeight + interval);
    }

    void resetBoundaryPosition()
    {
        leftMost = Vector3.zero;
        rightMost = Vector3.zero;
        upMost = Vector3.zero;
        downMost = Vector3.zero;
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
            Vector3 tempVector = OriginalPosition(playedDomino);
            if (playedDomino.direction == DominoController.Direction.Vertical)
            {
                isSpinnerPlaced = true;
                spinner = playedDomino;
                horizontalDominoes.Add(playedDomino);
                verticalDominoes.Add(playedDomino);               
                horizontalPositions.Add(new Vector3(playedDomino.transform.position.x, playedDomino.transform.position.y));
                verticalPositions.Add(new Vector3(playedDomino.transform.position.x, playedDomino.transform.position.y));
                horizontalRotations.Add(playedDomino.transform.rotation);
                verticalRotations.Add(playedDomino.transform.rotation);
            }
            else
            {
                horizontalDominoes.Add(playedDomino);
                horizontalPositions.Add(new Vector3(playedDomino.transform.position.x, playedDomino.transform.position.y));
                horizontalRotations.Add(playedDomino.transform.rotation);
            }
            return;
        }

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

        countPositionNumber(putPosition);
        setGeneralOffset();
        
        if (putPosition == PutPosition.Left)
        {
//            Debug.Log("place left");
            horizontalDominoes.Insert(0, playedDomino);


            // calculate offset
            // both horizontal
            if (historyDomino.direction == playedDomino.direction)
            {
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal - dominoScale * (interval + Constants.dominoHeight);
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal - (interval + 0.5f * dominoScale * (Constants.dominoHeight + Constants.dominoWidth));
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }
            // set spinner
            if (!isSpinnerPlaced && playedDomino.direction == DominoController.Direction.Vertical)
            {
                spinner = playedDomino;
                verticalDominoes.Add(playedDomino);
                verticalPositions.Add(OriginalPosition(playedDomino));
                verticalRotations.Add(playedDomino.transform.rotation);
                isSpinnerPlaced = true;
            }
            horizontalPositions.Insert(0, OriginalPosition(playedDomino));
            horizontalRotations.Insert(0, playedDomino.transform.rotation);
        }
        else if (putPosition == PutPosition.Right)
        {
//            Debug.Log("place right");
            horizontalDominoes.Add(playedDomino);

            // calculate offset
            // both horizontal
            if (historyDomino.direction == playedDomino.direction)
            {
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal + dominoScale * (interval + Constants.dominoHeight);
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal + (interval + 0.5f * dominoScale * (Constants.dominoHeight + Constants.dominoWidth));
                playedDomino.offsetVertical = historyDomino.offsetVertical;
            }
            // set spinner
            if (!isSpinnerPlaced && playedDomino.direction == DominoController.Direction.Vertical)
            {
                spinner = playedDomino;
                verticalDominoes.Add(playedDomino);
                verticalPositions.Add(OriginalPosition(playedDomino));
                verticalRotations.Add(playedDomino.transform.rotation);
                isSpinnerPlaced = true;
            }
            horizontalPositions.Add(OriginalPosition(playedDomino));
            horizontalRotations.Add(playedDomino.transform.rotation);
        }
        //VERTICAL
        else if (putPosition == PutPosition.Up)
        {
//            Debug.Log("place up");
            verticalDominoes.Insert(0, playedDomino);

            // both vertical
            if (historyDomino.direction == playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical + (interval + dominoScale * Constants.dominoHeight);
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical + (interval + 0.5f * dominoScale * (Constants.dominoHeight + Constants.dominoWidth));
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }
            verticalPositions.Insert(0, OriginalPosition(playedDomino));
            verticalRotations.Insert(0, playedDomino.transform.rotation);
        }
        else if (putPosition == PutPosition.Down)
        {
//            Debug.Log("place down");
            verticalDominoes.Add(playedDomino);

            // both vertical
            if (historyDomino.direction == playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical - (interval + dominoScale * Constants.dominoHeight);
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }
            // one horizontal one vertical
            if (historyDomino.direction != playedDomino.direction)
            {
                playedDomino.offsetVertical = historyDomino.offsetVertical - (interval + 0.5f * dominoScale * (Constants.dominoHeight + Constants.dominoWidth));
                playedDomino.offsetHorizontal = historyDomino.offsetHorizontal;
            }
            verticalPositions.Add(OriginalPosition(playedDomino));
            verticalRotations.Add(playedDomino.transform.rotation);
        }
        else
        {
            throw new ArgumentException("No valid historyDomino", "historyDomino");
        }

        for (int i = 0; i < horizontalDominoes.Count; i++)
        {
            horizontalPositions[i] = OriginalPosition(horizontalDominoes[i]);
        }
        for (int i = 0; i < verticalDominoes.Count; i++)
        {
            verticalPositions[i] = OriginalPosition(verticalDominoes[i]);
        }

        findBoundaryPosition();

        //Debug.Log("leftmost: " + leftMost.x);
        //Debug.Log("rightmost " + rightMost.x);


        for (int i = 0; i < horizontalDominoes.Count; i++)
        {
            SetPlayedDominoPosition(horizontalDominoes[i], horizontalPositions[i], horizontalRotations[i]);
            if (leftMost != Vector3.zero && horizontalPositions[i].x < leftMost.x)
            {
                SetRotateDomino(horizontalDominoes[i], new Vector3(leftMost.x - 0.5f * dominoScale * Constants.dominoWidth, leftMost.y));
            }
            if (rightMost != Vector3.zero && horizontalPositions[i].x > rightMost.x)
            {
                SetRotateDomino(horizontalDominoes[i], new Vector3(rightMost.x + 0.5f * dominoScale * Constants.dominoWidth, rightMost.y));
            }
        }
        for (int i = 0; i < verticalDominoes.Count; i++)
        {
            SetPlayedDominoPosition(verticalDominoes[i], verticalPositions[i], verticalRotations[i]);
            if (upMost != Vector3.zero && verticalPositions[i].y > upMost.y)
            {
                SetRotateDomino(verticalDominoes[i], new Vector3(upMost.x, upMost.y + 0.5f * dominoScale * Constants.dominoWidth));
            }
            if (downMost != Vector3.zero && verticalPositions[i].y < downMost.y)
            {
                SetRotateDomino(verticalDominoes[i], new Vector3(downMost.x, downMost.y - 0.5f * dominoScale * Constants.dominoWidth));
            }
        }

        resetBoundaryPosition();

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
        horizontalPositions.Clear();
        verticalPositions.Clear();
        horizontalRotations.Clear();
        verticalRotations.Clear();
        resetBoundaryPosition();
    }
}
