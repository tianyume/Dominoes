using System;

namespace AI
{
    public struct ChosenWayToPlay
    {
        public DominoController chosenDomino { get; private set; }
        public DominoController chosenPlace { get; private set; }

        public ChosenWayToPlay(DominoController chosenDomino, DominoController chosenPlace)
        {
            this.chosenDomino = chosenDomino;
            this.chosenPlace = chosenPlace;
        }
    }
}

