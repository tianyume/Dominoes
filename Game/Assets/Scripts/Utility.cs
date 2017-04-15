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
}

