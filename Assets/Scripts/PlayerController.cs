using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameController gameController;
    public List<DominoController> dominoControllers;
    public TileController tileController;



    public HistoryController historyController;
    private DominoController chosenDomino;
    private DominoController chosenPlace;


    public int startPosition1 = -8;
    public int startPosition2 = 8;
    public string playerName;
    private int cnt;


	void Start()
    {
        dominoControllers = new List<DominoController>(28);
//        tileController.Shuffle();
//        dominoControllers.Add(tileController.Deal());
//        dominoControllers[0].GetComponent<Transform>().transform.position.x = new Vector3(0, 0, 0);

	}

    void Update()
    {



    }




    public void DominoOnClick(DominoController clickedDomino)
    {
        Debug.Log(clickedDomino.upperValue);
        Vector3 temp = clickedDomino.transform.position;
        //set clicked domino
        foreach (DominoController domino in dominoControllers)
        {
            if (domino.upperValue == clickedDomino.upperValue && domino.lowerValue == clickedDomino.lowerValue)
            {
                chosenDomino = clickedDomino;
                break;
            }
        }



       
        //move clicked card
        if(playerName=="player1")
        {

            if (clickedDomino.isClicked)
            {
                temp.y = temp.y + (float)0.50;
                clickedDomino.transform.position = temp;
            }
            else
            {
                temp.y = temp.y - (float)0.50;
                clickedDomino.transform.position = temp;
            }
        }
        else
        {
            if (clickedDomino.isClicked)
            {
                temp.y = temp.y - (float)0.50;
                clickedDomino.transform.position = temp;
            }
            else
            {
                temp.y = temp.y + (float)0.50;
                clickedDomino.transform.position = temp;
            }
        }


    }

    // For Tile
    public void AddDomino()
    {
        // TOFIX
        dominoControllers.AddRange(tileController.Deal());

        if (playerName == "player1")
        {
            cnt = 0;
            foreach (DominoController domino in dominoControllers)
            {
                // TOFIX
                domino.transform.position = new Vector3(cnt++, startPosition1, 0);
                domino.onClick = DominoOnClick;
            }
        }
        else if(playerName == "player2")
        {
            cnt = 0;
            foreach (DominoController domino in dominoControllers)
            {
                // TOFIX
                domino.transform.position = new Vector3(cnt++, startPosition2, 0);
                domino.onClick = DominoOnClick;
            }
        }
           

    }

    // For Game
    public void PlayDomino()
    {
        //ToFix
        if (chosenDomino != null && chosenPlace != null)
        {
            if (playerName == "player1")
            {
                registerDomino();
                gameController.PlayerPlayDomino(this, chosenDomino, chosenPlace);
            }
            else if (playerName == "player2")
            {
                registerDomino();
                gameController.PlayerPlayDomino(this, chosenDomino, chosenPlace);
            }            
        }
                     
    }
       
    public void registerDomino()
    {
        int horizontalLen = historyController.horizontalDominoes.Count;
        int VerticalLen = historyController.verticalDominoes.Count;
        if (horizontalLen != 0)
        {
            historyController.horizontalDominoes[0].onClick = DominoOnClick;
            historyController.horizontalDominoes[horizontalLen-1].onClick = DominoOnClick;
        }
        if (VerticalLen != 0)
        {
            historyController.verticalDominoes[0].onClick = DominoOnClick;
            historyController.verticalDominoes[horizontalLen-1].onClick = DominoOnClick;
        }
    }

    public void DrawDomino(DominoController dominoController)
    {
//        tileController = new TileController();
//        if (tileController.IsDrawable())
//        {
//            
//        }
    }
}
