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
        public ObservableCollection<int> SortingItems { get { return m_SortingItems; } set { m_SortingItems = value; OnPropertyChanged("SortingItems"); } }

        public bool Sorting { get { return m_Thread != null && m_Thread.IsAlive; } }
        public bool SwapTimeoutEnabled { get { return NumItems < 500; } }

        private int m_NumItems = 100;
        private int m_MaxValue = 100;
        private int m_SwapTimeout = 1;
        public int NumItems { get { return m_NumItems; } set { m_NumItems = value; OnPropertyChanged("NumItems"); OnPropertyChanged("SwapTimeout"); OnPropertyChanged("SwapTimeoutEnabled"); } }
        public int MaxValue { get { return m_MaxValue; } set { m_MaxValue = value; OnPropertyChanged("MaxValue"); } }
        public int SwapTimeout { get { return SwapTimeoutEnabled ? m_SwapTimeout : -99; } set { m_SwapTimeout = value; OnPropertyChanged("SwapTimeout"); } }

        public void Randomize()
        {
            if (Sorting)
            {
                m_Thread.Abort();
                m_Thread.Join();
                OnPropertyChanged("Sorting");
            }

            SortingItems = new ObservableCollection<int>(Utilities.GenerateRandomList(NumItems, MaxValue));
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

        private void SortThread()
        {
            BubbleSorter sorter = new BubbleSorter();
            sorter.SwapCallback = SwapCallback;
            sorter.SortedCallback = SortedCallback;
            sorter.Sort(m_SortingItems.ToList(), 3);

            OnPropertyChanged("Sorting");
        }

        private void SwapCallback(int i, int j)
        {
            if (SwapTimeout <= 0)
                return;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SortingItems.Swap(i, j);
            }));
            System.Threading.Thread.Sleep(SwapTimeout);
        }

        private void SortedCallback(List<int> list)
        {
            if (SwapTimeout > 0)
                return;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SortingItems = new ObservableCollection<int>(list);
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
