using System;

namespace Model
{
    public class ChosenWayToPlayModel
    {
        private DominoModel chosenDomino;
        private DominoModel chosenPlace;

        public ChosenWayToPlayModel(DominoModel chosenDomino, DominoModel chosenPlace)
        {
            this.chosenDomino = chosenDomino;
            this.chosenPlace = chosenPlace;
        }

        public override int GetHashCode()
        {
            return chosenDomino.GetHashCode() * 33 + chosenPlace.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            ChosenWayToPlayModel item = obj as ChosenWayToPlayModel;
            if (item == null)
            {
                return false;
            }
            return chosenDomino.Equals(item.chosenDomino) && chosenPlace.Equals(item.chosenPlace);
        }
    }
}

