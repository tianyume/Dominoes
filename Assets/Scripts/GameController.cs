using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public DominoController Spinner;
    public LinkedList<DominoController> HorizontalDominoes;
    public LinkedList<DominoController> VerticalDominoes;
//    private Game game;

	void Start()
    {
//        game = new Game();
    }

    void PlayDomino(Player player, DominoController dominoController, DominoController anotherDominoController)
    {
        
    }
}
