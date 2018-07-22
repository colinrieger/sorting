using System.Collections.Generic;

namespace Sorting
{
    /// <summary>
    /// Merge sort implementation.
    /// Divide and conquer. Split the list into n sublists then merge the sublists, comparing their elements.
    /// Best-case:      O(n log n)
    /// Average-case:   O(n log n)
    /// Worst-case:     O(n log n)
    /// </summary>
    public class MergeSorter : Sorter
    {
        protected override void HandleSort(List<int> list, int version)
        {
            Sort(list, 0, list.Count - 1);
        }

        private void Merge(List<int> list, int left, int middle, int right)
        {
            List<int> leftList = list.GetRange(left, middle - left + 1);
            List<int> rightList = list.GetRange(middle + 1, right - middle);

            int listIndex = left;
            int leftIndex = 0;
            int rightIndex = 0;
            while (leftIndex < leftList.Count && rightIndex < rightList.Count)
            {
                if (leftList[leftIndex] <= rightList[rightIndex])
                {
                    list[listIndex] = leftList[leftIndex];
                    SetCallback?.Invoke(listIndex, leftList[leftIndex]);
                    leftIndex++;
                }
                else
                {
                    list[listIndex] = rightList[rightIndex];
                    SetCallback?.Invoke(listIndex, rightList[rightIndex]);
                    rightIndex++;
                }
                listIndex++;
            }

            while (leftIndex < leftList.Count)
            {
                list[listIndex] = leftList[leftIndex];
                SetCallback?.Invoke(listIndex, leftList[leftIndex]);
                leftIndex++;
                listIndex++;
            }

            while (rightIndex < rightList.Count)
            {
                list[listIndex] = rightList[rightIndex];
                SetCallback?.Invoke(listIndex, rightList[rightIndex]);
                rightIndex++;
                listIndex++;
            }
        }

        private void Sort(List<int> list, int left, int right)
        {
            if (left >= right)
                return;

            int middle = (left / 2) + (right / 2);
            Sort(list, left, middle);
            Sort(list, middle + 1, right);
            Merge(list, left, middle, right);
        }
    }
}
