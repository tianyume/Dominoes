using System;
using System.Collections.Generic;
    
public class Utility
{
    public static int GetSumOfHistoryDominoes(
        List<DominoController> horizontalDominoes,
        List<DominoController> verticalDominoes,
        DominoController spinner)
    {
        // Only 1 domino in history
        if (horizontalDominoes.Count == 1)
        {
            DominoController domino = horizontalDominoes[0];          
            if (domino.direction == DominoController.Direction.Horizontal)
            {
                return domino.leftValue + domino.rightValue;
            }
            else
            {
                return domino.upperValue + domino.lowerValue;
            }
        }
        int sum = 0;
        DominoController leftDomino = horizontalDominoes[0];
        if (leftDomino.direction == DominoController.Direction.Horizontal)
        {
            sum += leftDomino.leftValue;
        }
        else
        {
            sum += leftDomino.upperValue + leftDomino.lowerValue;
        }
        DominoController rightDomino = horizontalDominoes[horizontalDominoes.Count - 1];
        if (rightDomino.direction == DominoController.Direction.Horizontal)
        {
            sum += rightDomino.rightValue;
        }
        else
        {
            sum += rightDomino.upperValue + rightDomino.lowerValue;
        }
        // Have dominoes except spinner
        if (verticalDominoes.Count > 1)
        {
            DominoController upperDomino = verticalDominoes[0];
            if (upperDomino != spinner)
            {
                if (upperDomino.direction == DominoController.Direction.Vertical)
                {
                    sum += upperDomino.upperValue;
                }
                else
                {
                    sum += upperDomino.leftValue + upperDomino.rightValue;
                }
            }
            DominoController lowerDomino = verticalDominoes[verticalDominoes.Count - 1];
            if (lowerDomino != spinner)
            {
                if (lowerDomino.direction == DominoController.Direction.Vertical)
                {
                    sum += lowerDomino.lowerValue;
                }
                else
                {
                    sum += lowerDomino.leftValue + lowerDomino.rightValue;
                }
            }
        }
        return sum;
    }
    public static int GetOutsideNumber(DominoController domino, Model.PositionInHistory position)
    {
        switch (position)
        {
            case Model.PositionInHistory.Left:
                if (domino.direction == DominoController.Direction.Horizontal)
                {
                    return domino.leftValue;
                }
                else
                {
                    return domino.upperValue;
                }
            case Model.PositionInHistory.Right:
                if (domino.direction == DominoController.Direction.Horizontal)
                {
                    return domino.rightValue;
                }
                else
                {
                    return domino.upperValue;
                }
            case Model.PositionInHistory.Upper:
                if (domino.direction == DominoController.Direction.Vertical)
                {
                    return domino.upperValue;
                }
                else
                {
                    return domino.leftValue;
                }
            case Model.PositionInHistory.Lower:
                if (domino.direction == DominoController.Direction.Vertical)
                {
                    return domino.lowerValue;
                }
                else
                {
                    return domino.leftValue;
                }
        }
        return -1;
    }

    public static int GetNextOutsideNumber(DominoController chosenDomino, DominoController chosenPlace, Model.PositionInHistory position)
    {
        switch (position)
        {
            case Model.PositionInHistory.Left:
                if (chosenPlace.direction == DominoController.Direction.Horizontal)
                {
                    int value = chosenPlace.leftValue;
                    if (chosenDomino.upperValue == chosenDomino.lowerValue && chosenDomino.upperValue == value)
                    {
                        return value;
                    }
                    else if (chosenDomino.upperValue == value)
                    {
                        return chosenDomino.lowerValue;
                    }
                    else
                    {
                        return chosenDomino.upperValue;
                    }
                }
                else
                {
                    int value = chosenPlace.upperValue;
                    if (chosenDomino.upperValue == value)
                    {
                        return chosenDomino.lowerValue;
                    }
                    else
                    {
                        return chosenDomino.upperValue;
                    }
                }
            case Model.PositionInHistory.Right:
                if (chosenPlace.direction == DominoController.Direction.Horizontal)
                {
                    int value = chosenPlace.rightValue;
                    if (chosenDomino.upperValue == chosenDomino.lowerValue && chosenDomino.upperValue == value)
                    {
                        return value;
                    }
                    else if (chosenDomino.upperValue == value)
                    {
                        return chosenDomino.lowerValue;
                    }
                    else
                    {
                        return chosenDomino.upperValue;
                    }
                }
                else
                {
                    int value = chosenPlace.upperValue;
                    if (chosenDomino.upperValue == value)
                    {
                        return chosenDomino.lowerValue;
                    }
                    else
                    {
                        return chosenDomino.upperValue;
                    }
                }
            case Model.PositionInHistory.Upper:
                if (chosenPlace.direction == DominoController.Direction.Vertical)
                {
                    int value = chosenPlace.upperValue;
                    if (chosenDomino.upperValue == chosenDomino.lowerValue && chosenDomino.upperValue == value)
                    {
                        return value;
                    }
                    else if (chosenDomino.upperValue == value)
                    {
                        return chosenDomino.lowerValue;
                    }
                    else
                    {
                        return chosenDomino.upperValue;
                    }
                }
                else
                {
                    int value = chosenPlace.leftValue;
                    if (chosenDomino.upperValue == value)
                    {
                        return chosenDomino.lowerValue;
                    }
                    else
                    {
                        return chosenDomino.upperValue;
                    }
                }
            case Model.PositionInHistory.Lower:
                if (chosenPlace.direction == DominoController.Direction.Vertical)
                {
                    int value = chosenPlace.lowerValue;
                    if (chosenDomino.upperValue == chosenDomino.lowerValue && chosenDomino.upperValue == value)
                    {
                        return value;
                    }
                    else if (chosenDomino.upperValue == value)
                    {
                        return chosenDomino.lowerValue;
                    }
                    else
                    {
                        return chosenDomino.upperValue;
                    }
                }
                else
                {
                    int value = chosenPlace.leftValue;
                    if (chosenDomino.upperValue == value)
                    {
                        return chosenDomino.lowerValue;
                    }
                    else
                    {
                        return chosenDomino.upperValue;
                    }
                }
        }
        return -1;
    }

    public static DominoController GetLeftDomino(HistoryController history)
    {
        return history.horizontalDominoes.Count > 0 ? history.horizontalDominoes[0] : null;
    }

    public static DominoController GetRightDomino(HistoryController history)
    {
        return history.horizontalDominoes.Count > 0 ? history.horizontalDominoes[history.horizontalDominoes.Count - 1] : null;
    }

    public static DominoController GetUpperDomino(HistoryController history)
    {
        return history.verticalDominoes.Count > 0 ? history.verticalDominoes[0] : null;
    }

    public static DominoController GetLowerDomino(HistoryController history)
    {
        return history.verticalDominoes.Count > 0 ? history.verticalDominoes[history.verticalDominoes.Count - 1] : null;
    }

    public static Model.DominoModel DominoControllerToDominoModel(DominoController domino)
    {
        if (domino == null)
        {
            return null;
        }
        if (domino.direction == DominoController.Direction.Horizontal)
        {
            return new Model.DominoModel(domino.leftValue, domino.rightValue);
        }
        else
        {
            return new Model.DominoModel(domino.upperValue, domino.lowerValue);
        }
    }

}

