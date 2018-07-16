using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sorting
{
    public static class Utilities
    {
        public static void Swap<T>(this ObservableCollection<T> collection, int i, int j)
        {
            T tmp = collection[i];
            collection[i] = collection[j];
            collection[j] = tmp;
        }
        public static void Swap<T>(this List<T> list, int i, int j)
        {
            T tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }

        public static List<int> GenerateRandomList(int count, int maxValue)
        {
            Random random = new Random(DateTime.Now.Millisecond);

            List<int> randomList = new List<int>();
            while (0 < count--)
                randomList.Add(random.Next(maxValue));

            return randomList;
        }
    }
}
