using System;
using System.Collections.Generic;

namespace Model
{
    public class GameStatusModel
    {
        private HashSet<DominoModel> historySet;
        private HashSet<DominoModel> playerSet;
        private DominoModel leftDomino;
        private DominoModel rightDomino;
        private DominoModel upperDomino;
        private DominoModel lowerDomino;

        public GameStatusModel(HashSet<DominoModel> historySet, HashSet<DominoModel> playerSet, DominoModel leftDomino, DominoModel rightDomino, DominoModel upperDomino, DominoModel lowerDomino)
        {
            this.historySet = historySet;
            this.playerSet = playerSet;
            this.leftDomino = leftDomino;
            this.rightDomino = rightDomino;
            this.upperDomino = upperDomino;
            this.lowerDomino = lowerDomino;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            hash = hash * 33 + historySet.GetHashCode();
            hash = hash * 33 + playerSet.GetHashCode();
            hash = hash * 33;
            if (leftDomino != null)
            {
                hash += leftDomino.GetHashCode();
            }
            hash = hash * 33;
            if (rightDomino != null)
            {
                hash += rightDomino.GetHashCode();
            }
            hash = hash * 33;
            if (upperDomino != null)
            {
                hash += upperDomino.GetHashCode();
            }
            hash = hash * 33;
            if (lowerDomino != null)
            {
                hash += lowerDomino.GetHashCode();
            }
            return hash;
        }

        public override bool Equals(object obj)
        {
            GameStatusModel item = obj as GameStatusModel;
            if (item == null)
            {
                return false;
            }
            return historySet.Equals(item.historySet) && playerSet.Equals(item.playerSet) && leftDomino.Equals(item.leftDomino) && rightDomino.Equals(item.rightDomino) && upperDomino.Equals(item.upperDomino) && lowerDomino.Equals(item.lowerDomino);
        }
    }
}

