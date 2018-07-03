using System;
using System.Collections.Generic;

namespace Sorting
{
    public static class Utilities
    {
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
            while (count > 0)
            {
                randomList.Add(random.Next(maxValue));
                count--;
            }

            return randomList;
        }
    }
}
