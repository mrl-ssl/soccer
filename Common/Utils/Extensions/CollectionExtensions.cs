

using System;
using System.Collections.Generic;

namespace MRL.SSL.Common.Utils.Extensions
{
    public static class CollectionExtensions
    {
        public static T[] Populate<T>(this T[] array, Func<int, T> provider)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = provider(i);
            return array;
        }
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
        public static T PopBack<T>(this Queue<T> queue)
        {
            if (queue == null) throw new System.NullReferenceException("object quque is null");

            // Move all the items before the one to remove to the back
            for (int i = 0; i < queue.Count - 1; ++i)
            {
                queue.Enqueue(queue.Dequeue());
            }

            // Remove the item at the index
            var element = queue.Dequeue();

            for (int i = 0; i < queue.Count; ++i)
            {
                queue.Enqueue(queue.Dequeue());
            }

            return element;
        }
        public static void ReverseQ<T>(this Queue<T> queue)
        {
            if (queue == null) throw new System.NullReferenceException("object quque is null");

            // Move all the items before the one to remove to the back
            for (int i = 0; i < queue.Count - 1; ++i)
            {
                queue.Enqueue(queue.Dequeue());
            }

        }
    }
}