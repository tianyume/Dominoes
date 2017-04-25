using System;

namespace Model
{
    public class DominoModel
    {
        // When stored, number1 should be smaller than number2
        private int number1;
        private int number2;
        
        public DominoModel(int number1, int number2)
        {
            this.number1 = Math.Min(number1, number2);
            this.number2 = Math.Max(number1, number2);
        }

        public override int GetHashCode()
        {
            int hash = 0;
            hash = hash * 33 + number1;
            hash = hash * 33 + number2;
            return hash;
        }

        public override bool Equals(object obj)
        {
            DominoModel item = obj as DominoModel;
            if (item == null)
            {
                return false;
            }
            return number1 == item.number1 && number2 == item.number2;
        }
    }
}

