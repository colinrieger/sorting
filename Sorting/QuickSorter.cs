using System.Collections.Generic;

namespace Sorting
{
    /// <summary>
    /// Quick sort implementation.
    /// Divide and conquer. Split the list into sublists using a pivot. Sort the list so that all items less than pivot come before it and all items greater than pivot come after, putting the pivot in its final position. Then repeat on low and high sublists.
    /// Best-case:      O(n log n)
    /// Average-case:   O(n log n)
    /// Worst-case:     O(n^2)
    /// </summary>
    public class QuickSorter : Sorter
    {
        protected override void HandleSort(List<int> list, int version)
        {
            Sort(list, 0, list.Count - 1);
        }

        private int Partition(List<int> list, int low, int high)
        {
            int p = list[high];
            int i = low - 1;
            for (int j = low; j <= high - 1; j++)
                if (list[j] <= p)
                {
                    i++;
                    list.Swap(i, j);
                    SwapCallback?.Invoke(i, j);
                }

            list.Swap(i + 1, high);
            SwapCallback?.Invoke(i + 1, high);

            return i + 1;
        }

        private void Sort(List<int> list, int low, int high)
        {
            if (low >= high)
                return;

            int p = Partition(list, low, high);

            Sort(list, low, p - 1);
            Sort(list, p + 1, high);
        }
    }
}
