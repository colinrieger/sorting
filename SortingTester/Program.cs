using Sorting;
using System;
using System.Collections.Generic;

namespace SortingTester
{
    class Program
    {
        static void Main(string[] args)
        {
            TestBubbleSorter();

            Console.ReadKey();
        }

        static void TestBubbleSorter()
        {
            BubbleSorter sorter = new BubbleSorter();
            List<int> list = Utilities.GenerateRandomList(10000, 10000);
            List<int> list2 = new List<int>(list);
            List<int> list3 = new List<int>(list);
            sorter.Sort(list);
            Console.WriteLine("Bubble Sort 1\nTime Elapsed: " + sorter.TimeElapsed);
            sorter.Sort(list2, 2);
            Console.WriteLine("Bubble Sort 2\nTime Elapsed: " + sorter.TimeElapsed);
            sorter.Sort(list3, 3);
            Console.WriteLine("Bubble Sort 3\nTime Elapsed: " + sorter.TimeElapsed);
        }
    }
}
