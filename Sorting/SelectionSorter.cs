using System.Collections.Generic;

namespace Sorting
{
    /// <summary>
    /// Selection sort implementation.
    /// Iterate through the list, comparing an item to all other items after it. Swap (or select) the minimum item found with the current item.
    /// Best-case:      O(n^2)
    /// Average-case:   O(n^2)
    /// Worst-case:     O(n^2)
    /// </summary>
    public class SelectionSorter : Sorter
    {
        protected override void HandleSort(List<int> list, int version)
        {
            Sort(list);
        }

        private void Sort(List<int> list)
        {
            int count = list.Count;
            for (int i = 0; i < count - 1; i++)
            {
                int iMin = i;
                for (int j = i + 1; j < count; j++)
                    if (list[j] < list[iMin])
                        iMin = j;

                if (iMin != i)
                {
                    list.Swap(i, iMin);
                    SwapCallback?.Invoke(i, iMin);
                }
            }
        }
    }
}
