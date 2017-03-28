using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIController : PlayerController {

    private const int kNumberOfCardsToDraw = 7;

//    public GameController gameController;
//    public List<DominoController> dominoControllers;
//    public TileController tileController;



//    public HistoryController historyController;
    private DominoController AIchosenDomino;
    private DominoController AIchosenplace;
//    private bool readytoplay = false;

//    public Text turnText;


//    public int startPosition1 = -8;
//    public int startPosition2 = 8;
//    public string playerName;
//    private int cnt;
//    private bool isFirstDeal = true;


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

    public void AIPlay()
    {

            int horizontalLen = historyController.horizontalDominoes.Count;
            int verticalLen = historyController.verticalDominoes.Count;
            //there is no cards on play zone(the first card to play)
            if (horizontalLen == 0 && verticalLen == 0)
            {
                AIchosenplace = null;
                AIchosenDomino = dominoControllers[0];
                return;
            }
            else
            {
                List<DominoController> validPlaces = new List<DominoController>();
                if (horizontalLen != 0)
                {
                    validPlaces.Add(historyController.horizontalDominoes[0]);
                    validPlaces.Add(historyController.horizontalDominoes[horizontalLen-1]);
                }
                if (verticalLen != 0)
                {
                    validPlaces.Add(historyController.verticalDominoes[0]);
                    validPlaces.Add(historyController.verticalDominoes[verticalLen-1]);
                }
                foreach (DominoController playingDomino in dominoControllers)
                {
                    if (validPlaces.Count != 0)
                    {
                        foreach (DominoController toplaceDomino in validPlaces)
                        {
                            if (horizontalLen != 0)
                            {
                                if (toplaceDomino == historyController.horizontalDominoes[0])
                                {
                                    //vertical toplaceDomino
                                    if (toplaceDomino.leftValue == -1)
                                    {
                                        if (playingDomino.upperValue == toplaceDomino.upperValue || playingDomino.lowerValue == toplaceDomino.upperValue)
                                        {
                                            AIchosenDomino = playingDomino;
                                            AIchosenplace = toplaceDomino;
                                            return; 
                                            
                                        }
                                    }
                                    //horizontal toplaceDomino
                                    else
                                    {
                                        if (playingDomino.upperValue == toplaceDomino.leftValue || playingDomino.lowerValue == toplaceDomino.leftValue)
                                        {

                                            AIchosenDomino = playingDomino;
                                            AIchosenplace = toplaceDomino;
                                            return; 
                                        }

                                    }
                                }
                                if (toplaceDomino == historyController.horizontalDominoes[horizontalLen-1])
                                {
                                    //vertival topalceDomino
                                    if (toplaceDomino.leftValue == -1)
                                    {
                                        if (toplaceDomino.upperValue == playingDomino.upperValue || playingDomino.lowerValue == toplaceDomino.upperValue)
                                        {
                                            AIchosenDomino = playingDomino;
                                            AIchosenplace = toplaceDomino;
                                            return;
                                        }
                                    }
                                    //horizontal topalceDomino
                                    else
                                    {
                                        if (playingDomino.upperValue == toplaceDomino.rightValue || playingDomino.lowerValue == toplaceDomino.rightValue)
                                        {
                                            AIchosenDomino = playingDomino;
                                            AIchosenplace = toplaceDomino;
                                            return;
                                        }

                                    }
                                } 
                            }

                            if (verticalLen != 0)
                            {
                                if (toplaceDomino == historyController.verticalDominoes[0])
                                {
                                    //vertical topalceDomino
                                    if (toplaceDomino.leftValue == -1)
                                    {

                                        if (playingDomino.upperValue == toplaceDomino.upperValue || playingDomino.lowerValue == toplaceDomino.upperValue)
                                        {
                                            AIchosenDomino = playingDomino;
                                            AIchosenplace = toplaceDomino;
                                            return;
                                        }
                                    }
                                    //horizontal toplaceDomino
                                    else
                                    {
                                        if (playingDomino.upperValue == toplaceDomino.leftValue || playingDomino.lowerValue == toplaceDomino.leftValue)
                                        {
                                            AIchosenDomino = playingDomino;
                                            AIchosenplace = toplaceDomino;
                                            return;
                                        }
                                    }
                                }
                                if (toplaceDomino == historyController.verticalDominoes[verticalLen-1])
                                {
                                    //vertical toplaceDomino
                                    if (toplaceDomino.leftValue == -1)
                                    {
                                        if(playingDomino.upperValue == toplaceDomino.lowerValue || playingDomino.lowerValue == toplaceDomino.lowerValue)
                                        {

                                            AIchosenDomino = playingDomino;
                                            AIchosenplace = toplaceDomino;
                                            return;
                                        }

                                    }
                                    //horizontal toplaceDomino
                                    else
                                    {
                                        if (playingDomino.upperValue == toplaceDomino.leftValue || playingDomino.lowerValue == toplaceDomino.leftValue)
                                        {

                                            AIchosenDomino = playingDomino;
                                            AIchosenplace = toplaceDomino;
                                            return;
                                        }
                                    }
                                }
                            }

                        }
                    }

                }
            }

        
    }


    public void AIPlaceDomino()
    {
        DominoController clickedDomino = AIchosenplace;
        int horizontalLen = historyController.horizontalDominoes.Count;
        int verticalLen = historyController.verticalDominoes.Count;

        if (AIchosenDomino != null)
        {
            if (clickedDomino != null)
            {
                if (clickedDomino == historyController.horizontalDominoes[0])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.upperValue || AIchosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.upperValue)
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
                            else
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_left,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.leftValue && AIchosenDomino.upperValue == AIchosenDomino.lowerValue)
                        {
                            AIchosenplace = clickedDomino;
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_left,h_horizontal,p_special");
                            return;
                        }
                        else if (AIchosenDomino.upperValue == clickedDomino.leftValue || AIchosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.leftValue)
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
                            else
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_left,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.horizontalDominoes[horizontalLen - 1])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.upperValue || AIchosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.upperValue)
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
                            else
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_right,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.rightValue && AIchosenDomino.upperValue == AIchosenDomino.lowerValue)
                        {
                            AIchosenplace = clickedDomino;
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_right,h_horizontal,p_special");
                            return;
                        }
                        else if (AIchosenDomino.upperValue == clickedDomino.rightValue || AIchosenDomino.lowerValue == clickedDomino.rightValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.rightValue)
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
                            else
                                AIchosenDomino.SetLeftRightValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_right,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.verticalDominoes[0])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.upperValue && AIchosenDomino.upperValue == AIchosenDomino.lowerValue)
                        {
                            AIchosenplace = clickedDomino;
                            AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_top,h_vertical,p_special");
                            return;
                        }
                        else if (AIchosenDomino.upperValue == clickedDomino.upperValue || AIchosenDomino.lowerValue == clickedDomino.upperValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.upperValue)
                                AIchosenDomino.SetUpperLowerValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_top,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.leftValue || AIchosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.upperValue == clickedDomino.leftValue)
                                AIchosenDomino.SetUpperLowerValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_top,h_horizontal,p_normal");
                            return;
                        }
                    }
                }
                if (clickedDomino == historyController.verticalDominoes[verticalLen - 1])
                {
                    if (clickedDomino.leftValue == -1)
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.lowerValue && AIchosenDomino.upperValue == AIchosenDomino.lowerValue)
                        {
                            AIchosenplace = clickedDomino;
                            AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_bottom,h_vertical,p_special");
                            return;
                        }
                        else if (AIchosenDomino.upperValue == clickedDomino.lowerValue || AIchosenDomino.lowerValue == clickedDomino.lowerValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.lowerValue == clickedDomino.lowerValue)
                                AIchosenDomino.SetUpperLowerValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_bottom,h_vertical,p_normal");
                            return;
                        }
                    }
                    else
                    {
                        if (AIchosenDomino.upperValue == clickedDomino.leftValue || AIchosenDomino.lowerValue == clickedDomino.leftValue)
                        {
                            AIchosenplace = clickedDomino;
                            if (AIchosenDomino.lowerValue == clickedDomino.leftValue)
                                AIchosenDomino.SetUpperLowerValues(AIchosenDomino.lowerValue, AIchosenDomino.upperValue);
//                            PlayDomino();
//                            AIchosenDomino = null;
//                            AIchosenplace = null;
//                            Debug.Log("excute h_bottom,h_horizontal,p_normal");
                            return;
                        }
                    }
                }


            }
            else
            {
                if (AIchosenDomino.upperValue != AIchosenDomino.lowerValue)
                    AIchosenDomino.SetLeftRightValues(AIchosenDomino.upperValue, AIchosenDomino.lowerValue);
            }
        }
    }

    public void AIPlayDomino()
    {
        string playerturn = "";
        if (turnText.text.Equals("Player1's turn"))
        {
            playerturn = "player1";
        }
        else
        {
            playerturn = "player2";
        }

        if (!playerturn.Equals(playerName))
        {
            return;
        }
        AIPlay();
        AIPlaceDomino();
    }

    public new void PlayDomino()
    {
        AIPlayDomino();
        dominoControllers.Remove(AIchosenDomino);
        AddDomino();
        DominoController tcd, tcp;
        tcd = AIchosenDomino;
        tcp = AIchosenplace;
        AIchosenDomino = null;
        AIchosenplace = null;
        gameController.PlayerPlayDomino(this, tcd, tcp);



    }


}
