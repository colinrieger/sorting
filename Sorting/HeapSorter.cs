using System.Collections.Generic;

namespace Sorting
{
    /// <summary>
    /// Heap sort implementation.
    /// Heapify the list then move the heap's root (largest) item to the end of the list. Repeat until the heap portion of the list is empty.
    /// Best-case:      O(n log n)
    /// Average-case:   O(n log n)
    /// Worst-case:     O(n log n)
    /// </summary>
    public class HeapSorter : Sorter
    {
        protected override void HandleSort(List<int> list, int version)
        {
            Sort(list);
        }

        private void Heapify(List<int> list, int heapSize, int index)
        {
            int largest = index; // parent
            int left = (2 * index) + 1; // left child
            int right = (2 * index) + 2; // right child

            if (left < heapSize && list[left] > list[largest])
                largest = left;
            if (right < heapSize && list[right] > list[largest])
                largest = right;
            if (largest != index)
            {
                list.Swap(index, largest);
                SwapCallback?.Invoke(index, largest);
                Heapify(list, heapSize, largest);
            }
        }

        private void Sort(List<int> list)
        {
            int heapSize = list.Count;

            // build the heap
            for (int i = heapSize / 2; i >= 0; i--)
                Heapify(list, heapSize, i);

            for (int i = heapSize - 1; i > 0; i--)
            {
                list.Swap(0, i); // we know the root is the next largest item. move it to the end
                SwapCallback?.Invoke(0, i);
                Heapify(list, i, 0); // rebuild the heap
            }
        }
    }
}
