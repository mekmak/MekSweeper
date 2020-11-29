using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekSweeper.Extensions
{
    public static class ArrayExtensions
    {
        public static bool TryGetTop<T>(this T[,] array, int x, int y, out T element)
        {
            return TryGet(array, x, y + 1, out element);
        }

        public static bool TryGetTopLeft<T>(this T[,] array, int x, int y, out T element)
        {
            return TryGet(array, x - 1, y + 1, out element);
        }

        public static bool TryGetTopRight<T>(this T[,] array, int x, int y, out T element)
        {
            return TryGet(array, x + 1, y + 1, out element);
        }

        public static bool TryGetLeft<T>(this T[,] array, int x, int y, out T element)
        {
            return TryGet(array, x - 1, y, out element);
        }

        public static bool TryGetRight<T>(this T[,] array, int x, int y, out T element)
        {
            return TryGet(array, x + 1, y, out element);
        }
        
        public static bool TryGetBottom<T>(this T[,] array, int x, int y, out T element)
        {
            return TryGet(array, x, y - 1, out element);
        }

        public static bool TryGetBottomLeft<T>(this T[,] array, int x, int y, out T element)
        {
            return TryGet(array, x - 1, y - 1, out element);
        }

        public static bool TryGetBottomRight<T>(this T[,] array, int x, int y, out T element)
        {
            return TryGet(array, x + 1, y - 1, out element);
        }

        public static bool TryGet<T>(this T[,] array, int x, int y, out T element)
        {
            element = default;

            int columnCount = array.GetLength(0);
            if (x < 0 || x >= columnCount)
            {
                return false;
            }

            int rowCount = array.GetLength(1);
            if (y < 0 || y >= rowCount)
            {
                return false;
            }

            element = array[x, y];
            return true;
        }
    }
}
