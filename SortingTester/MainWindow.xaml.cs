using SortingTester.Controls;
using System.Collections.ObjectModel;
using System.Windows;

namespace SortingTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AddSorter();
        }

        private void AddSorter()
        {
            int currentChildren = sortersGrid.Children.Count;
            if (currentChildren == 8)
                return;

            if (currentChildren < 4)
                sortersGrid.Columns += 1;
            else if (currentChildren == 4)
                sortersGrid.Rows += 1;

            SorterControl sorterControl = new SorterControl();
            sortersGrid.Children.Add(sorterControl);
        }

        private void OnAddSorter(object sender, RoutedEventArgs e)
        {
            AddSorter();
        }

        private void OnRandomizeAll(object sender, RoutedEventArgs e)
        {
            foreach (SorterControl sorterControl in sortersGrid.Children)
                sorterControl.VM.Randomize();
        }

        private void OnSortAll(object sender, RoutedEventArgs e)
        {
            foreach (SorterControl sorterControl in sortersGrid.Children)
                sorterControl.VM.Sort();
        }

        private void OnRandomizeAllToSame(object sender, RoutedEventArgs e)
        {
            if (sortersGrid.Children.Count == 0)
                return;

            ObservableCollection<int> sortingItems = null;
            foreach (SorterControl sorterControl in sortersGrid.Children)
            {
                if (sortingItems == null)
                {
                    sorterControl.VM.Randomize();
                    sortingItems = sorterControl.VM.SortingItems;
                }
                else
                {
                    sorterControl.VM.SortingItems = new ObservableCollection<int>(sortingItems);
                }
            }
        }
    }
}
