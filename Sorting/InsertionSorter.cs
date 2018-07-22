using System.Collections.Generic;

namespace Sorting
{
    /// <summary>
    /// Insertion sort implementation.
    /// Iterate through the list, comparing an item to the ones before it and insert it at its lowest position.
    /// Best-case:      O(n)
    /// Average-case:   O(n^2)
    /// Worst-case:     O(n^2)
    /// </summary>
    public class InsertionSorter : Sorter
    {
        protected override void HandleSort(List<int> list, int version)
        {
            Sort(list);
        }

        private void Sort(List<int> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                int value = list[i];
                int j = i - 1;
                while (j >= 0 && list[j] > value)
                {
                    list[j + 1] = list[j];
                    SetCallback?.Invoke(j + 1, list[j]);
                    j--;
                }
                list[j + 1] = value;
                SetCallback?.Invoke(j + 1, value);
            }
        }
    }
}
