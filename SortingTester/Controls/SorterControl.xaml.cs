using Sorting;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;

namespace SortingTester.Controls
{
    public class SorterViewModel : INotifyPropertyChanged
    {
        private Thread m_Thread;
        private ObservableCollection<int> m_SortingItems = null;
        public ObservableCollection<int> SortingItems
        {
            get { return m_SortingItems; }
            set
            {
                if (Sorting)
                {
                    m_Thread.Abort();
                    m_Thread.Join();
                    OnPropertyChanged("Sorting");
                }

                m_SortingItems = value;
                
                TimeElapsed = string.Empty;
                OnPropertyChanged("SortingItems");
                OnPropertyChanged("SwapTimeoutEnabled");
                OnPropertyChanged("SwapTimeout");
            }
        }

        public bool Sorting { get { return m_Thread != null && m_Thread.IsAlive; } }
        public bool SwapTimeoutEnabled { get { return (SortingItems != null) ? SortingItems.Count < 500 : false; } }
        
        private int m_NumItems = 100;
        private int m_SwapTimeout = 1;
        private string m_TimeElapsed;
        private string m_SelectedSorterType;
        public int NumItems { get { return m_NumItems; } set { m_NumItems = value; OnPropertyChanged("NumItems");  } }
        public int SwapTimeout { get { return SwapTimeoutEnabled ? m_SwapTimeout : -99; } set { m_SwapTimeout = value; OnPropertyChanged("SwapTimeout"); } }
        public string TimeElapsed { get { return m_TimeElapsed; } set { m_TimeElapsed = value; OnPropertyChanged("TimeElapsed"); } }
        public string SelectedSorterType { get { return m_SelectedSorterType; } set { m_SelectedSorterType = value; OnPropertyChanged("SelectedSorterType"); } }

        public class SorterType { public string Name { get; set; } }
        public ObservableCollection<SorterType> SorterTypes { get; set; }

        public SorterViewModel()
        {
            SorterTypes = new ObservableCollection<SorterType>()
            {
                new SorterType() { Name = "Bubble" },
                new SorterType() { Name = "Heap" },
                new SorterType() { Name = "Insertion" },
                new SorterType() { Name = "Merge" },
                new SorterType() { Name = "Quick" },
                new SorterType() { Name = "Selection" }
            };
            m_SelectedSorterType = SorterTypes[0].Name;
        }

        public void Randomize()
        {
            SortingItems = new ObservableCollection<int>(Utilities.GenerateRandomList(NumItems));
        }

        public void Sort()
        {
            if (SortingItems == null || Sorting)
                return;
            
            m_Thread = new Thread(new ThreadStart(SortThread));
            m_Thread.SetApartmentState(ApartmentState.STA);
            m_Thread.Start();

            OnPropertyChanged("Sorting");
        }

        private Sorter GetSorter()
        {
            switch (SelectedSorterType)
            {
                case "Bubble": return new BubbleSorter();
                case "Heap": return new HeapSorter();
                case "Insertion": return new InsertionSorter();
                case "Merge": return new MergeSorter();
                case "Quick": return new QuickSorter();
                case "Selection": return new SelectionSorter();
                default: return null;
            }
        }

        private void SortThread()
        {
            Sorter sorter = GetSorter();
            sorter.SetCallback = SetCallback;
            sorter.SwapCallback = SwapCallback;
            sorter.SortedCallback = SortedCallback;
            sorter.Sort(m_SortingItems.ToList());

            OnPropertyChanged("Sorting");
        }

        private void SetCallback(int i, int value)
        {
            if (SwapTimeout <= 0)
                return;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (SortingItems.Count > i)
                    SortingItems[i] = value;
            }));
            System.Threading.Thread.Sleep(SwapTimeout);
        }

        private void SwapCallback(int i, int j)
        {
            if (SwapTimeout <= 0)
                return;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (SortingItems.Count > i && SortingItems.Count > j)
                    SortingItems.Swap(i, j);
            }));
            System.Threading.Thread.Sleep(SwapTimeout);
        }

        private void SortedCallback(List<int> list, string timeElapsed)
        {
            if (SwapTimeout > 0)
                return;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SortingItems = new ObservableCollection<int>(list);
                TimeElapsed = timeElapsed;
            }));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Interaction logic for SorterControl.xaml
    /// </summary>
    public partial class SorterControl : UserControl
    {
        private SorterViewModel m_VM = new SorterViewModel();
        public SorterViewModel VM { get { return m_VM; } }

        public SorterControl()
        {
            InitializeComponent();

            this.DataContext = m_VM;
        }

        private void OnRandomize(object sender, RoutedEventArgs e)
        {
            m_VM.Randomize();
        }

        private void OnSort(object sender, RoutedEventArgs e)
        {
            m_VM.Sort();
        }
    }

    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
