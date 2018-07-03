using System.Collections.Generic;

namespace Sorting
{
    public class BubbleSorter : Sorter
    {
        protected override void HandleSort(List<int> list, int version = 3)
        {
            switch (version)
            {
                case 1: Sort1(list); break;
                case 2: Sort2(list); break;
                case 3: Sort3(list); break;
            }
        }

        private void Sort1(List<int> list)
        {
            bool swapped = true;
            int end = list.Count;
            while (swapped)
            {
                swapped = false;
                for (int i = 1; i < end; i++)
                    if (list[i - 1] > list[i])
                    {
                        list.Swap(i, i - 1);
                        swapped = true;
                    }
            }
        }

        private void Sort2(List<int> list)
        {
            bool swapped = true;
            int end = list.Count;
            while (swapped)
            {
                swapped = false;
                for (int i = 1; i < end; i++)
                    if (list[i - 1] > list[i])
                    {
                        list.Swap(i, i - 1);
                        swapped = true;
                    }
                end--; // at the end of each pass, we know the element at end is in its final position
            }
        }

        private void Sort3(List<int> list)
        {
            int end = list.Count;
            while (end > 0)
            {
                int newEnd = 0; // keep track of the highest index that's swapped
                for (int i = 1; i < end; i++)
                    if (list[i - 1] > list[i])
                    {
                        list.Swap(i, i - 1);
                        newEnd = i;
                    }
                end = newEnd; // we know everything beyond newEnd is in its final position
            }
        }
    }
}
