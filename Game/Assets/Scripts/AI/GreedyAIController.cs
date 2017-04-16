using System.Collections.Generic;
using UnityEngine;
using System;

namespace AI
{
    public class GreedyAIController : PlayerController
    {
        public override void PlayDomino()
        {
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
                Dictionary<DominoController, List<DominoController>> placesToPlay = PlacesToPlay();
                if (placesToPlay.Count == 0)
                {
                    // Draw dominoes until not blocked or no dominoes in tile
                    base.DrawDomino();
                    placesToPlay = PlacesToPlay();
                }

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
            waysToPlay.Sort(delegate (ChosenWayToPlay x, ChosenWayToPlay y) {
                int xScore = GetScoreOfChosenWay(x);
                int yScore = GetScoreOfChosenWay(y);
                return xScore - yScore;
            });
            if (waysToPlay.Count == 0)
            {
                gameController.PlayerIsBlocked(this);
                return;
            }

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

            gameController.PlayerPlayDomino(this, bestWayToPlay.chosenDomino, bestWayToPlay.chosenPlace);
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
    }
}

