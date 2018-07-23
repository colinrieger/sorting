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

        const int c_MaxSorterColumns = 3;
        const int c_MaxSorterRows = 2;

        private void AddSorter()
        {
            int currentChildren = sortersGrid.Children.Count;
            if (currentChildren == (c_MaxSorterColumns * c_MaxSorterRows))
                return;
            
            if (currentChildren < c_MaxSorterColumns)
                sortersGrid.Columns += 1;
            else if ((currentChildren % c_MaxSorterColumns) == 0)
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
