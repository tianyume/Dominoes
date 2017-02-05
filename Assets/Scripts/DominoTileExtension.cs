using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileExtension
{
    public static class DominoTileExtension   
    {
        public static void Swap<T>(this List<T> list, int index1, int index2)
        {
            T temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }

        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                int j = (rnd.Next(0, n) % n);
                list.Swap(i, j);
            }
        }
    }
}
