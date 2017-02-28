using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameController gameController;
    //public DominoController dominoController;
    public string playerName;
    public List<DominoController> dominoControllers;
    public TileController tileController;
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




    public void DominoOnClick(DominoController dominoController)
    {
        //dominoControllers.Add(dominoController);
       // Debug.Log("click!!");
        Debug.Log(dominoController.upperValue);
        dominoController.transform.position.y++;

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
                domino.transform.position = new Vector3(cnt++, -4, 0);
                domino.onClick = DominoOnClick;
            }
        }
        else if(playerName == "player2")
        {
            cnt = 0;
            foreach (DominoController domino in dominoControllers)
            {
                // TOFIX
                domino.transform.position = new Vector3(cnt++, 4, 0);
                domino.onClick = DominoOnClick;
            }
        }

         


    }

    // For Game
    public void PlayDomino()
    {
         
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
