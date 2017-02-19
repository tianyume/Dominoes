using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public TileController tile;
    public PlayerController player1;
    public PlayerController player2;
    
    public DominoController exampleDomino;
    public DominoController Spinner;
    public LinkedList<DominoController> HorizontalDominoes;
    public LinkedList<DominoController> VerticalDominoes;

    public delegate void TileDrawAction(int index);
    public delegate void PlayerPlayDominoAction();

    public TileDrawAction tileShouldDraw;
    public PlayerPlayDominoAction player1ShouldPlayDomino;
    public PlayerPlayDominoAction player2ShouldPlayDomino;

    void TileDidDealt(DominoController domino)
    {
    }

    void TileDidReset()
    {
    }

    void PlayerDidPlayedDomino(Player player, DominoController dominoController, DominoController anotherDominoController)
    {
        
    }
}
