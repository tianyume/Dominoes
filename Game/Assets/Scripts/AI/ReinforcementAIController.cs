using System.Collections.Generic;
using UnityEngine;
using System;

using Model;

namespace AI
{
    public class ReinforcementAIController : PlayerController
    {
        enum NextState
        {
            Wait, Draw, Play
        }

        private int myScore;
        private int opponentScore;
        private NextState nextState;
        private Dictionary<DominoController, List<DominoController>> placesToPlay = null;
        private Dictionary<PlayActionModel, WinLoseStatusModel> winLoseDict = new Dictionary<PlayActionModel, WinLoseStatusModel>();
        private List<WinLoseStatusModel> winLoseStatusList = new List<WinLoseStatusModel>();

        private void Update()
        {
            switch (nextState)
            {
                case NextState.Wait:
                    return;
                case NextState.Draw:
                    if (history.horizontalDominoes.Count > 0)
                    {
                        placesToPlay = PlacesToPlay();
                        if (placesToPlay.Count == 0)
                        {
                            base.DrawDomino();
                            placesToPlay = PlacesToPlay();
                            if (placesToPlay.Count == 0)
                            {
                                nextState = NextState.Wait;
                                gameController.PlayerIsBlocked(this);
                                return;
                            }
                        }
                    }
                    nextState = NextState.Play;
                    break;
                case NextState.Play:
                    List<ChosenWayToPlay> waysToPlay = new List<ChosenWayToPlay>();
                    if (history.horizontalDominoes.Count == 0)
                    {
                        foreach (DominoController domino in dominoControllers)
                        {
                            waysToPlay.Add(new ChosenWayToPlay(domino, null));
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<DominoController, List<DominoController>> entry in placesToPlay)
                        {
                            List<DominoController> list = entry.Value;
                            foreach (DominoController chosenPlace in list)
                            {
                                ChosenWayToPlay chosenWayToPlay = new ChosenWayToPlay(entry.Key, chosenPlace);
                                waysToPlay.Add(chosenWayToPlay);
                            }
                        }
                    }
                    int historyDominoesNum = history.horizontalDominoes.Count + history.verticalDominoes.Count - (history.spinner == null ? 0 : 1);
                    GameStatusModel gameStatus = GetGameStatus();
                    // From small to large
                    waysToPlay.Sort(delegate (ChosenWayToPlay x, ChosenWayToPlay y)
                        {
                            if (historyDominoesNum <= 18)
                            {
                                int xScore = GetScoreOfChosenWay(x);
                                int yScore = GetScoreOfChosenWay(y);
                                return xScore - yScore;
                            }

                            ChosenWayToPlayModel xModel = new ChosenWayToPlayModel(Utility.DominoControllerToDominoModel(x.chosenDomino), Utility.DominoControllerToDominoModel(x.chosenPlace));
                            PlayActionModel xPlayActionModel = new PlayActionModel(gameStatus, xModel);
                            WinLoseStatusModel xWinLoseStatus;
                            if (winLoseDict.ContainsKey(xPlayActionModel))
                            {
                                xWinLoseStatus = winLoseDict[xPlayActionModel];
                            }
                            else
                            {
                                xWinLoseStatus = new WinLoseStatusModel();
                                xWinLoseStatus.wins = 1;
                                xWinLoseStatus.loses = 1;
                            }
                            double xWinRate = (double)xWinLoseStatus.wins / (xWinLoseStatus.wins + xWinLoseStatus.loses);

                            ChosenWayToPlayModel yModel = new ChosenWayToPlayModel(Utility.DominoControllerToDominoModel(y.chosenDomino), Utility.DominoControllerToDominoModel(y.chosenPlace));
                            PlayActionModel yPlayActionModel = new PlayActionModel(gameStatus, yModel);
                            WinLoseStatusModel yWinLoseStatus;
                            if (winLoseDict.ContainsKey(yPlayActionModel))
                            {
                                yWinLoseStatus = winLoseDict[xPlayActionModel];
                            }
                            else
                            {
                                yWinLoseStatus = new WinLoseStatusModel();
                                yWinLoseStatus.wins = 1;
                                yWinLoseStatus.loses = 1;
                            }
                            double yWinRate = (double)yWinLoseStatus.wins / (yWinLoseStatus.wins + yWinLoseStatus.loses);

                            if (xWinRate < yWinRate)
                            {
                                return -1;
                            }
                            else if (xWinRate == yWinRate)
                            {
                                return 0;
                            }

                            return 1;
                        });

                    ChosenWayToPlay bestWayToPlay = waysToPlay[waysToPlay.Count - 1];
                    PlaceDomino(bestWayToPlay.chosenDomino, bestWayToPlay.chosenPlace, history);
                    dominoControllers.Remove(bestWayToPlay.chosenDomino);
                    if (historyDominoesNum > 18)
                    {
                        ChosenWayToPlayModel bestPlayModel = new ChosenWayToPlayModel(Utility.DominoControllerToDominoModel(bestWayToPlay.chosenDomino), Utility.DominoControllerToDominoModel(bestWayToPlay.chosenPlace));
                        PlayActionModel bestPlayActionModel = new PlayActionModel(gameStatus, bestPlayModel);
                        if (winLoseDict.ContainsKey(bestPlayActionModel))
                        {
                            winLoseStatusList.Add(winLoseDict[bestPlayActionModel]);
                        }
                        else
                        {
                            WinLoseStatusModel winLoseStatus = new WinLoseStatusModel();
                            winLoseStatus.wins = 1;
                            winLoseStatus.loses = 1;
                            winLoseDict.Add(bestPlayActionModel, winLoseStatus);
                            winLoseStatusList.Add(winLoseStatus);
                        }
                    }

                    // Debug
                    Debug.Log("Chosen Domino: " + bestWayToPlay.chosenDomino.leftValue + ", " + bestWayToPlay.chosenDomino.rightValue + ", " + bestWayToPlay.chosenDomino.upperValue + ", " + bestWayToPlay.chosenDomino.lowerValue);
                    if (bestWayToPlay.chosenPlace != null)
                    {
                        Debug.Log("Chosen Place: " + bestWayToPlay.chosenPlace.leftValue + ", " + bestWayToPlay.chosenPlace.rightValue + ", " + bestWayToPlay.chosenPlace.upperValue + ", " + bestWayToPlay.chosenPlace.lowerValue);
                    }
                    Debug.Log(Environment.StackTrace);

                    nextState = NextState.Wait;
                    gameController.PlayerPlayDomino(this, bestWayToPlay.chosenDomino, bestWayToPlay.chosenPlace);
                    break;
            }
        }

        public override void AddDomino()
        {
            base.AddDomino();
            // TOFIX
            int myIncrement = gameController.scoreOfPlayer2 - myScore;
            int opponentIncrement = gameController.scoreOfPlayer1 - opponentScore;
            if (myIncrement >= 0 && opponentIncrement >= 0)
            {
                if (myIncrement >= opponentIncrement)
                {
                    foreach (WinLoseStatusModel winLoseStatusModel in winLoseStatusList)
                    {
                        winLoseStatusModel.wins++;
                    }
                }
                else
                {
                    foreach (WinLoseStatusModel winLoseStatusModel in winLoseStatusList)
                    {
                        winLoseStatusModel.loses++;
                    }
                }
            }
            // TOFIX
            myScore = gameController.scoreOfPlayer2;
            opponentScore = gameController.scoreOfPlayer1;
            winLoseStatusList.Clear();
        }

        public override void PlayDomino()
        {
            nextState = NextState.Draw;
        }

        private Dictionary<DominoController, List<DominoController>> PlacesToPlay()
        {
            Dictionary<DominoController, List<DominoController>> placesToPlay = new Dictionary<DominoController, List<DominoController>>(dominoControllers.Count * 4);
            foreach (DominoController domino in dominoControllers)
            {
                // Add places can be played for each domino
                List<DominoController> places = base.ListOfValidPlaces(domino);
                if (places == null)
                {
                    continue;
                }
                placesToPlay.Add(domino, places);
            }
            return placesToPlay;
        }

        private int GetScoreOfChosenWay(ChosenWayToPlay wayToPlay)
        {
            int score = 0;
            // If history has no domino
            if (history.horizontalDominoes.Count == 0)
            {
                if (wayToPlay.chosenDomino.direction == DominoController.Direction.Horizontal)
                {
                    int value = wayToPlay.chosenDomino.leftValue + wayToPlay.chosenDomino.rightValue;
                    score = (value % 5 == 0) ? value : 0;
                }
                else
                {
                    int value = wayToPlay.chosenDomino.upperValue + wayToPlay.chosenDomino.lowerValue;
                    score = (value % 5 == 0) ? value : 0;
                }
                return score;
            }
            // Else that history has at least 1 domino
            DominoController copiedDomino = Instantiate<DominoController>(wayToPlay.chosenDomino);
            HistoryController copiedHistory = Instantiate<HistoryController>(history);
            // Simulate to place a domino and then calculate the sum
            PlaceDomino(copiedDomino, wayToPlay.chosenPlace, copiedHistory);
            copiedHistory.Add(copiedDomino, wayToPlay.chosenPlace);
            score = Utility.GetSumOfHistoryDominoes(copiedHistory.horizontalDominoes, copiedHistory.verticalDominoes, copiedHistory.spinner);
            score = score % 5 == 0 ? score : 0;
            Destroy(copiedDomino.gameObject);
            Destroy(copiedHistory.gameObject);
            return score;
        }

        private void PlaceDomino(DominoController chosenDomino, DominoController chosenPlace, HistoryController history)
        {
            DominoController clickedDomino = chosenPlace;
            int horizontalLen = history.horizontalDominoes.Count;
            int verticalLen = history.verticalDominoes.Count;

            if (chosenDomino != null)
            {
                if (chosenPlace != null)
                {
                    if (chosenPlace == history.horizontalDominoes[0])
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
                                return;
                            }
                        }
                        else
                        {
                            if (chosenDomino.upperValue == clickedDomino.leftValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                            {
                                chosenPlace = clickedDomino;
                                return;
                            }
                            else if (chosenDomino.upperValue == clickedDomino.leftValue || chosenDomino.lowerValue == clickedDomino.leftValue)
                            {
                                chosenPlace = clickedDomino;
                                if (chosenDomino.upperValue == clickedDomino.leftValue)
                                    chosenDomino.SetLeftRightValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                                else
                                    chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                                return;
                            }
                        }
                    }
                    if (clickedDomino == history.horizontalDominoes[horizontalLen - 1])
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
                                return;
                            }
                        }
                        else
                        {
                            if (chosenDomino.upperValue == clickedDomino.rightValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                            {
                                chosenPlace = clickedDomino;
                                return;
                            }
                            else if (chosenDomino.upperValue == clickedDomino.rightValue || chosenDomino.lowerValue == clickedDomino.rightValue)
                            {
                                chosenPlace = clickedDomino;
                                if (chosenDomino.upperValue == clickedDomino.rightValue)
                                    chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                                else
                                    chosenDomino.SetLeftRightValues(chosenDomino.lowerValue, chosenDomino.upperValue);
                                return;
                            }
                        }
                    }
                    if (verticalLen > 0 && clickedDomino == history.verticalDominoes[0])
                    {
                        if (clickedDomino.leftValue == -1)
                        {
                            if (chosenDomino.upperValue == clickedDomino.upperValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                            {
                                chosenPlace = clickedDomino;
                                chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                                return;
                            }
                            else if (chosenDomino.upperValue == clickedDomino.upperValue || chosenDomino.lowerValue == clickedDomino.upperValue)
                            {
                                chosenPlace = clickedDomino;
                                if (chosenDomino.upperValue == clickedDomino.upperValue)
                                    chosenDomino.SetUpperLowerValues(chosenDomino.lowerValue, chosenDomino.upperValue);
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
                                return;
                            }
                        }
                    }
                    if (verticalLen > 0 && clickedDomino == history.verticalDominoes[verticalLen - 1])
                    {
                        if (clickedDomino.leftValue == -1)
                        {
                            if (chosenDomino.upperValue == clickedDomino.lowerValue && chosenDomino.upperValue == chosenDomino.lowerValue)
                            {
                                chosenPlace = clickedDomino;
                                chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                                return;
                            }
                            else if (chosenDomino.upperValue == clickedDomino.lowerValue || chosenDomino.lowerValue == clickedDomino.lowerValue)
                            {
                                chosenPlace = clickedDomino;
                                if (chosenDomino.lowerValue == clickedDomino.lowerValue)
                                    chosenDomino.SetUpperLowerValues(chosenDomino.lowerValue, chosenDomino.upperValue);
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
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (chosenDomino.upperValue != chosenDomino.lowerValue)
                        chosenDomino.SetLeftRightValues(chosenDomino.upperValue, chosenDomino.lowerValue);
                }
            }
        }

        private GameStatusModel GetGameStatus()
        {
            HashSet<DominoModel> historySet = new HashSet<DominoModel>();
            foreach (DominoController domino in history.horizontalDominoes)
            {
                historySet.Add(Utility.DominoControllerToDominoModel(domino));
            }
            foreach (DominoController domino in history.verticalDominoes)
            {
                historySet.Add(Utility.DominoControllerToDominoModel(domino));
            }
            HashSet<DominoModel> playerSet = new HashSet<DominoModel>();
            foreach (DominoController domino in dominoControllers)
            {
                playerSet.Add(Utility.DominoControllerToDominoModel(domino));
            }
            DominoModel left = null;
            DominoModel right = null;
            DominoModel upper = null;
            DominoModel lower = null;
            if (history.horizontalDominoes.Count > 0)
            {
                DominoController domino = history.horizontalDominoes[0];
                left = Utility.DominoControllerToDominoModel(domino);
            }
            if (history.horizontalDominoes.Count > 0)
            {
                DominoController domino = history.horizontalDominoes[history.horizontalDominoes.Count - 1];
                right = Utility.DominoControllerToDominoModel(domino);
            }
            if (history.verticalDominoes.Count > 0)
            {
                DominoController domino = history.verticalDominoes[0];
                upper = Utility.DominoControllerToDominoModel(domino);
            }
            if (history.verticalDominoes.Count > 0)
            {
                DominoController domino = history.verticalDominoes[history.verticalDominoes.Count - 1];
                lower = Utility.DominoControllerToDominoModel(domino);
            }
            return new Model.GameStatusModel(historySet, playerSet, left, right, upper, lower);
        }
    }
}

