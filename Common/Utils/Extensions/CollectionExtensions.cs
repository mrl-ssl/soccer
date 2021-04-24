

namespace MRL.SSL.Common.Utils.Extensions
{
    public static class CollectionExtensions
    {
        public static T[] Populate<T>(this T[] array)
            where T : new()
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = new T();
            return array;
        }
        public static T[,] Populate<T>(this T[,] array)
           where T : new()
        {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    array[i, j] = new T();

            return array;
        }
    }
}