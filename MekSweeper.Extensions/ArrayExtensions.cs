using System.Collections.Generic;

namespace MekSweeper.Extensions
{
    public static class ArrayExtensions
    {
        public static IEnumerable<T> GetNeighbors<T>(this T[,] array, int x, int y)
        {
            if (TryGetTop(array, x, y, out var top))
            {
                yield return top;
            }

            if (TryGetTopLeft(array, x, y, out var topLeft))
            {
                yield return topLeft;
            }

            if (TryGetTopRight(array, x, y, out var topRight))
            {
                yield return topRight;
            }

            if (TryGetLeft(array, x, y, out var left))
            {
                yield return left;
            }

            if (TryGetRight(array, x, y, out var right))
            {
                yield return right;
            }

            if (TryGetBottom(array, x, y, out var bottom))
            {
                yield return bottom;
            }

            if (TryGetBottomLeft(array, x, y, out var bottomLeft))
            {
                yield return bottomLeft;
            }

            if (TryGetBottomRight(array, x, y, out var bottomRight))
            {
                yield return bottomRight;
            }
        }

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

        public static IEnumerable<T> GetNeighbors<T>(this List<List<T>> array, int x, int y)
        {
            if (TryGetTop(array, x, y, out var top))
            {
                yield return top;
            }

            if (TryGetTopLeft(array, x, y, out var topLeft))
            {
                yield return topLeft;
            }

            if (TryGetTopRight(array, x, y, out var topRight))
            {
                yield return topRight;
            }

            if (TryGetLeft(array, x, y, out var left))
            {
                yield return left;
            }

            if (TryGetRight(array, x, y, out var right))
            {
                yield return right;
            }

            if (TryGetBottom(array, x, y, out var bottom))
            {
                yield return bottom;
            }

            if (TryGetBottomLeft(array, x, y, out var bottomLeft))
            {
                yield return bottomLeft;
            }

            if (TryGetBottomRight(array, x, y, out var bottomRight))
            {
                yield return bottomRight;
            }
        }

        public static bool TryGetTop<T>(this List<List<T>> array, int x, int y, out T element)
        {
            return TryGet(array, x, y + 1, out element);
        }

        public static bool TryGetTopLeft<T>(this List<List<T>> array, int x, int y, out T element)
        {
            return TryGet(array, x - 1, y + 1, out element);
        }

        public static bool TryGetTopRight<T>(this List<List<T>> array, int x, int y, out T element)
        {
            return TryGet(array, x + 1, y + 1, out element);
        }

        public static bool TryGetLeft<T>(this List<List<T>> array, int x, int y, out T element)
        {
            return TryGet(array, x - 1, y, out element);
        }

        public static bool TryGetRight<T>(this List<List<T>> array, int x, int y, out T element)
        {
            return TryGet(array, x + 1, y, out element);
        }
        
        public static bool TryGetBottom<T>(this List<List<T>> array, int x, int y, out T element)
        {
            return TryGet(array, x, y - 1, out element);
        }

        public static bool TryGetBottomLeft<T>(this List<List<T>> array, int x, int y, out T element)
        {
            return TryGet(array, x - 1, y - 1, out element);
        }

        public static bool TryGetBottomRight<T>(this List<List<T>> array, int x, int y, out T element)
        {
            return TryGet(array, x + 1, y - 1, out element);
        }

        public static bool TryGet<T>(this List<List<T>> array, int x, int y, out T element)
        {
            element = default;

            int columnCount = array.Count;
            if (x < 0 || x >= columnCount)
            {
                return false;
            }

            int rowCount = array[0].Count;
            if (y < 0 || y >= rowCount)
            {
                return false;
            }

            element = array[x][y];
            return true;
        }
    }
}
