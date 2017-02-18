using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public DominoController exampleDomino;
    public DominoController Spinner;
    public LinkedList<DominoController> HorizontalDominoes;
    public LinkedList<DominoController> VerticalDominoes;
//    private Game game;

	void Start()
    {
//        game = new Game();
//        DominoController dominoController = Instantiate<DominoController>(exampleDomino);
//        dominoController.SetValues(new DominoController.Values(4, 6));
    }

    void PlayDomino(Player player, DominoController dominoController, DominoController anotherDominoController)
    {
        
    }
}
