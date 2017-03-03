using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    private const int maxScore = 150;

    public TileController tile;
    public HistoryController history;
    public PlayerController player1;
    public PlayerController player2;

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

    void PlayerPlayDomino(PlayerController player, DominoController domino, DominoController anotherDomino)
    {
        history.Add(domino, anotherDomino);
        // TODO calculate the score of current play
        if (player.dominoControllers.Count == 0)
        {
            // TODO calculate the score
            if (scoreOfPlayer1 >= maxScore || scoreOfPlayer2 >= maxScore)
            {
                return;
            }
            tile.Shuffle();
            player1.AddDomino();
            player2.AddDomino();
        }
        if (player == player1)
        {
            player2.PlayDomino();
        }
        else
        {
            player1.PlayDomino();
        }
    }
}
