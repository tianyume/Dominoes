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
    private bool readytoplay = false;


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
        Debug.Log(clickedDomino.leftValue);
        bool preflag = false;
        readytoplay = false;

        //set clicked domino
        foreach (DominoController domino in dominoControllers)
        {
            if (domino == clickedDomino)
            {           
                preflag = true;    
                //move clicked card
                Selecteffect(clickedDomino);
                registerDomino();


                if (chosenDomino == null)
                {
                    chosenDomino = clickedDomino;
                }
                else if (clickedDomino == chosenDomino)
                {
                    chosenDomino = null;
                }
                else if (clickedDomino != chosenDomino)
                {
                    chosenDomino.isClicked = false;
                    Selecteffect(chosenDomino);
                    chosenDomino = clickedDomino;
                }
                break;
            }
        }
        if (!preflag && chosenDomino != null)
        {
            readytoplay = true;
            Debug.Log(playerName);
        }

        if (chosenDomino != null)
        {
            int horizontalLen = historyController.horizontalDominoes.Count;
            int verticalLen = historyController.verticalDominoes.Count;
            if (horizontalLen == 0 && verticalLen == 0)
            {
                chosenPlace = null;
                readytoplay = true;
                if (chosenDomino.upperValue != chosenDomino.lowerValue)
                    chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                PlayDomino();
                chosenDomino = null;
            }
            else if(readytoplay)
            {
                if (clickedDomino == historyController.horizontalDominoes[0])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (chosenDomino.upperValue == clickedDomino.upperValue || chosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.upperValue)
                                chosenDomino.SetLeftRightValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            else
                                chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_left,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (chosenDomino.upperValue == clickedDomino.leftValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                        {
                            chosenPlace = clickedDomino;
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_left,h_horizontal,p_special");
                            return;
                        }
                        else if (chosenDomino.upperValue == clickedDomino.leftValue || chosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.leftValue)
                                chosenDomino.SetLeftRightValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            else
                                chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_left,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.horizontalDominoes[horizontalLen-1])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (chosenDomino.upperValue == clickedDomino.upperValue || chosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.upperValue)
                                chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            else
                                chosenDomino.SetLeftRightValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_right,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (chosenDomino.upperValue == clickedDomino.rightValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                        {
                            chosenPlace = clickedDomino;
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_right,h_horizontal,p_special");
                            return;
                        }
                        else if (chosenDomino.upperValue == clickedDomino.rightValue || chosenDomino.lowerValue == clickedDomino.rightValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.rightValue)
                                chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            else
                                chosenDomino.SetLeftRightValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_right,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.verticalDominoes[0])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if(chosenDomino.upperValue == clickedDomino.upperValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                        {
                            chosenPlace = clickedDomino;
                            chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_top,h_vertical,p_special");
                            return;
                        }
                        else if (chosenDomino.upperValue == clickedDomino.upperValue || chosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.upperValue)
                                chosenDomino.SetUpperLowerValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_top,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (chosenDomino.upperValue == clickedDomino.leftValue || chosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.upperValue == clickedDomino.leftValue)
                                chosenDomino.SetUpperLowerValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_top,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.verticalDominoes[verticalLen-1])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if(chosenDomino.upperValue == clickedDomino.lowerValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                        {
                            chosenPlace = clickedDomino;
                            chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_bottom,h_vertical,p_special");
                            return;
                        }
                        else if (chosenDomino.upperValue == clickedDomino.lowerValue || chosenDomino.lowerValue == clickedDomino.lowerValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.lowerValue == clickedDomino.lowerValue)
                                chosenDomino.SetUpperLowerValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_bottom,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (chosenDomino.upperValue == clickedDomino.leftValue || chosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            chosenPlace = clickedDomino;
                            if (chosenDomino.lowerValue == clickedDomino.leftValue)
                                chosenDomino.SetUpperLowerValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                            PlayDomino();
                            chosenDomino = null;
                            chosenPlace = null;
                            Debug.Log("excute h_bottom,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
            }
                

        }

    }

    public void Selecteffect(DominoController selected)
    {
        Vector3 temp = selected.transform.position;
        if(playerName=="player1")
        {

            if (selected.isClicked)
            {
                temp.y = temp.y + (float)0.50;
                selected.transform.position = temp;
            }
            else
            {
                temp.y = temp.y - (float)0.50;
                selected.transform.position = temp;
            }
        }
        else
        {
            if (selected.isClicked)
            {
                temp.y = temp.y - (float)0.50;
                selected.transform.position = temp;
            }
            else
            {
                temp.y = temp.y + (float)0.50;
                selected.transform.position = temp;
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
        if (chosenDomino != null && readytoplay == true)
        {
//            registerDomino();
            if (playerName == "player1")
            {
                
                gameController.PlayerPlayDomino(this, chosenDomino, chosenPlace);

            }
            else if (playerName == "player2")
            {
//                registerDomino();
                gameController.PlayerPlayDomino(this, chosenDomino, chosenPlace);
            }
            dominoControllers.Remove(chosenDomino);
        }
                     
    }
       
    public void registerDomino()
    {
        int horizontalLen = historyController.horizontalDominoes.Count;
        int verticalLen = historyController.verticalDominoes.Count;
        if (horizontalLen != 0)
        {
            historyController.horizontalDominoes[0].onClick = DominoOnClick;
            historyController.horizontalDominoes[horizontalLen-1].onClick = DominoOnClick;
        }
        if (verticalLen != 0)
        {
            historyController.verticalDominoes[0].onClick = DominoOnClick;
            historyController.verticalDominoes[verticalLen-1].onClick = DominoOnClick;
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
