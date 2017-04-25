using System;

namespace Model
{
    public class PlayActionModel
    {
        private GameStatusModel gameStatusModel;
        private ChosenWayToPlayModel chosenWayToPlay;

        public PlayActionModel(GameStatusModel gameStatusModel, ChosenWayToPlayModel chosenWayToPlay)
        {
            this.gameStatusModel = gameStatusModel;
            this.chosenWayToPlay = chosenWayToPlay;
        }

        public override int GetHashCode()
        {
            return gameStatusModel.GetHashCode() * 33 + chosenWayToPlay.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            PlayActionModel item = obj as PlayActionModel;
            if (item == null)
            {
                return false;
            }
            return gameStatusModel.Equals(item.gameStatusModel) && chosenWayToPlay.Equals(item.chosenWayToPlay);
        }
    }
}

