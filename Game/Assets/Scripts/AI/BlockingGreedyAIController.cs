using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace AI
{
    public class BlockingGreedyAIController : PlayerController
    {
        enum NextState
        {
            Wait, Draw, Play
        }

        public PlayerController opponent;

        private NextState nextState;
        private Dictionary<DominoController, List<DominoController>> placesToPlay = null;
        private int lastNumberOfOpponentDominoes;
        private DominoController lastLeft;
        private DominoController lastRight;
        private DominoController lastUpper;
        private DominoController lastLower;

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
                                UpdateOpponentData();
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
                    // From small to large
                    waysToPlay.Sort(delegate (ChosenWayToPlay x, ChosenWayToPlay y)
                        {
                            int xScore = GetScoreOfChosenWay(x);
                            int yScore = GetScoreOfChosenWay(y);
                            if (xScore != yScore)
                            {
                                return xScore - yScore;
                            }

                            bool canXBlock = CanPlayDominoBlockOpponent(x);
                            bool canYBlock = CanPlayDominoBlockOpponent(y);

                            if (canXBlock && !canYBlock)
                            {
                                return 1;
                            }
                            else if (!canXBlock && canYBlock)
                            {
                                return -1;
                            }

                            return 0;
                        });

                    ChosenWayToPlay bestWayToPlay = waysToPlay[waysToPlay.Count - 1];
                    PlaceDomino(bestWayToPlay.chosenDomino, bestWayToPlay.chosenPlace, history);
                    dominoControllers.Remove(bestWayToPlay.chosenDomino);

                    // Debug
                    Debug.Log("Chosen Domino: " + bestWayToPlay.chosenDomino.leftValue + ", " + bestWayToPlay.chosenDomino.rightValue + ", " + bestWayToPlay.chosenDomino.upperValue + ", " + bestWayToPlay.chosenDomino.lowerValue);
                    if (bestWayToPlay.chosenPlace != null)
                    {
                        Debug.Log("Chosen Place: " + bestWayToPlay.chosenPlace.leftValue + ", " + bestWayToPlay.chosenPlace.rightValue + ", " + bestWayToPlay.chosenPlace.upperValue + ", " + bestWayToPlay.chosenPlace.lowerValue);
                    }
                    Debug.Log(Environment.StackTrace);

                    nextState = NextState.Wait;
                    UpdateOpponentData();
                    gameController.PlayerPlayDomino(this, bestWayToPlay.chosenDomino, bestWayToPlay.chosenPlace);
                    break;
            }
        }

        public override void PlayDomino()
        {
            nextState = NextState.Draw;
        }

        public override void AddDomino()
        {
            base.AddDomino();
            UpdateOpponentData();
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

        private void UpdateOpponentData()
        {
            lastNumberOfOpponentDominoes = opponent.dominoControllers.Count;
            if (history.horizontalDominoes.Count == 0)
            {
                lastLeft = null;
                lastRight = null;
                lastUpper = null;
                lastLower = null;
            }
            else
            {
                lastLeft = history.horizontalDominoes[0];
                lastRight = history.horizontalDominoes[history.horizontalDominoes.Count - 1];
                if (history.verticalDominoes.Count == 0)
                {
                    lastUpper = null;
                    lastLower = null;
                }
                else
                {
                    lastUpper = history.verticalDominoes[0];
                    lastLower = history.verticalDominoes[history.verticalDominoes.Count - 1];
                }
            }
        }

        private bool CanPlayDominoBlockOpponent(ChosenWayToPlay wayToPlay)
        {
            if (opponent.dominoControllers.Count < lastNumberOfOpponentDominoes || wayToPlay.chosenPlace == null)
            {
                return false;
            }
            HashSet<int> missingNumber = GetOpponentMissingNumber();
            if (missingNumber.Count > 0)
            {
                if (history.horizontalDominoes.Count > 0)
                {
                    if (history.horizontalDominoes[0] == wayToPlay.chosenPlace)
                    {
                        return missingNumber.Contains(Utility.GetNextOutsideNumber(wayToPlay.chosenDomino, wayToPlay.chosenPlace, Model.PositionInHistory.Left));
                    }
                    else if (history.horizontalDominoes[history.horizontalDominoes.Count - 1] == wayToPlay.chosenPlace)
                    {
                        return missingNumber.Contains(Utility.GetNextOutsideNumber(wayToPlay.chosenDomino, wayToPlay.chosenPlace, Model.PositionInHistory.Right));
                    }
                    else if (history.verticalDominoes.Count > 0)
                    {
                        if (history.verticalDominoes[0] == wayToPlay.chosenPlace)
                        {
                            return missingNumber.Contains(Utility.GetNextOutsideNumber(wayToPlay.chosenDomino, wayToPlay.chosenPlace, Model.PositionInHistory.Upper));
                        }
                        else if (history.verticalDominoes[history.verticalDominoes.Count - 1] == wayToPlay.chosenPlace)
                        {
                            return missingNumber.Contains(Utility.GetNextOutsideNumber(wayToPlay.chosenDomino, wayToPlay.chosenPlace, Model.PositionInHistory.Lower));
                        }
                    }
                }
            }
            return false;
        }

        private HashSet<int> GetOpponentMissingNumber()
        {
            HashSet<int> missingNumber = new HashSet<int>();
            if (history.horizontalDominoes.Count > 0)
            {
                if (history.horizontalDominoes[0] != lastLeft)
                {
                    missingNumber.Add(Utility.GetOutsideNumber(history.horizontalDominoes[history.horizontalDominoes.Count - 1], Model.PositionInHistory.Right));
                    if (history.verticalDominoes.Count > 0)
                    {
                        missingNumber.Add(Utility.GetOutsideNumber(history.verticalDominoes[0], Model.PositionInHistory.Upper));
                        missingNumber.Add(Utility.GetOutsideNumber(history.verticalDominoes[history.verticalDominoes.Count - 1], Model.PositionInHistory.Lower));
                    }
                }
                else if (history.horizontalDominoes[history.horizontalDominoes.Count - 1] != lastRight)
                {
                    missingNumber.Add(Utility.GetOutsideNumber(history.horizontalDominoes[0], Model.PositionInHistory.Left));
                    if (history.verticalDominoes.Count > 0)
                    {
                        missingNumber.Add(Utility.GetOutsideNumber(history.verticalDominoes[0], Model.PositionInHistory.Upper));
                        missingNumber.Add(Utility.GetOutsideNumber(history.verticalDominoes[history.verticalDominoes.Count - 1], Model.PositionInHistory.Lower));
                    }
                }
                else if (history.verticalDominoes.Count > 0)
                {
                    if (history.verticalDominoes[0] != lastUpper)
                    {
                        missingNumber.Add(Utility.GetOutsideNumber(history.horizontalDominoes[0], Model.PositionInHistory.Left));
                        missingNumber.Add(Utility.GetOutsideNumber(history.horizontalDominoes[history.horizontalDominoes.Count - 1], Model.PositionInHistory.Right));
                        missingNumber.Add(Utility.GetOutsideNumber(history.verticalDominoes[history.verticalDominoes.Count - 1], Model.PositionInHistory.Lower));
                    }
                    else if (history.verticalDominoes[0] != lastLower)
                    {
                        missingNumber.Add(Utility.GetOutsideNumber(history.horizontalDominoes[0], Model.PositionInHistory.Left));
                        missingNumber.Add(Utility.GetOutsideNumber(history.horizontalDominoes[history.horizontalDominoes.Count - 1], Model.PositionInHistory.Right));
                        missingNumber.Add(Utility.GetOutsideNumber(history.verticalDominoes[0], Model.PositionInHistory.Upper));
                    }
                }
            }
            return missingNumber;
        }
    }
}

