using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private const int maxScore = 150;

    public TileController tile;
    public HistoryController history;
    public PlayerController player1;
    public PlayerController player2;
    public Text scoreText1;
    public Text scoreText2;

    public int scoreOfPlayer1;
    public int scoreOfPlayer2;

    void Start()
    {
        scoreOfPlayer1 = 0;
        scoreOfPlayer2 = 0;
        tile.Shuffle();
        player1.AddDomino();
        player2.AddDomino();
        player1.PlayDomino();
    }

    public void PlayerPlayDomino(PlayerController player, DominoController domino, DominoController anotherDomino)
    {
        // Put the played domino into history
        history.Add(domino, anotherDomino);
        // Calculate the score of current play
        ScoreByCurrentPlay(player);

        // Ending a hand
        if (player.dominoControllers.Count == 0)
        {
            // Calculate the score by ending a hand
            ScoreByEndingHand(player);
            if (scoreOfPlayer1 >= maxScore || scoreOfPlayer2 >= maxScore)
            {
                return;
            }
            tile.Shuffle();
            player1.AddDomino();
            player2.AddDomino();
            if (player == player1)
            {
                player1.PlayDomino();
            }
            else
            {
                player2.PlayDomino();
            }
            return;
        }
        // Or ending a turn
        if (player == player1)
        {
            if (player2.HasCardToPlay())
            {
                player2.PlayDomino();
            }
            else
            {
                player2.DrawDomino();
                player2.PlayDomino();
            }

        }
        else
        {
            if (player1.HasCardToPlay())
            {
                player1.PlayDomino();
            }
            else
            {
                player1.DrawDomino();
                player1.PlayDomino();
            }
        }
    }

    void ScoreByCurrentPlay(PlayerController player)
    {
        int sum = GetSumOfHistoryDominoes();
        if (sum % 5 == 0)
        {
            if (player == player1)
            {
                scoreOfPlayer1 += sum;
            }
            else
            {
                scoreOfPlayer2 += sum;
            }
            UpdateScore();
        }
    }

    void ScoreByEndingHand(PlayerController player)
    {
        if (player == player1)
        {
            int sum = 0;
            foreach (DominoController domino in player2.dominoControllers)
            {
                sum += domino.upperValue + domino.lowerValue;
            }
            if (sum % 5 < 3)
            {
                scoreOfPlayer1 += sum / 5 * 5;
            }
            else
            {
                scoreOfPlayer1 += (sum / 5 + 1) * 5;
            }
            UpdateScore();
        }
        else
        {
            int sum = 0;
            foreach (DominoController domino in player1.dominoControllers)
            {
                sum += domino.upperValue + domino.lowerValue;
            }
            if (sum % 5 < 3)
            {
                scoreOfPlayer2 += sum / 5 * 5;
            }
            else
            {
                scoreOfPlayer2 += (sum / 5 + 1) * 5;
            }
            UpdateScore();
        }
    }

    int GetSumOfHistoryDominoes()
    {
        // Only 1 domino in history
        if (history.horizontalDominoes.Count == 1)
        {
            DominoController domino = history.horizontalDominoes[0];          
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
        DominoController leftDomino = history.horizontalDominoes[0];
        if (leftDomino.direction == DominoController.Direction.Horizontal)
        {
            sum += leftDomino.leftValue;
        }
        else
        {
            sum += leftDomino.upperValue + leftDomino.lowerValue;
        }
        DominoController rightDomino = history.horizontalDominoes[history.horizontalDominoes.Count - 1];
        if (rightDomino.direction == DominoController.Direction.Horizontal)
        {
            sum += rightDomino.rightValue;
        }
        else
        {
            sum += rightDomino.upperValue + rightDomino.lowerValue;
        }
        // Have dominoes except spinner
        if (history.verticalDominoes.Count > 1)
        {
            DominoController upperDomino = history.verticalDominoes[0];
            if (upperDomino != history.spinner)
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
            DominoController lowerDomino = history.verticalDominoes[history.verticalDominoes.Count - 1];
            if (lowerDomino != history.spinner)
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

    void UpdateScore()
    {
        Debug.Log(scoreText1.text);
        scoreText1.text = "Player1: " + scoreOfPlayer1;
        scoreText2.text = "Player2: " + scoreOfPlayer2;
    }
    
    void ResetGameTurn()
    {

    }
}
