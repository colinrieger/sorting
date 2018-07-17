using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SortingTester.Controls
{
    /// <summary>
    /// Interaction logic for Graph.xaml
    /// </summary>
    public partial class GraphControl : UserControl
    {
        public ObservableCollection<int> Items
        {
            get { return (ObservableCollection<int>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<int>), typeof(GraphControl), new PropertyMetadata(OnItemsChanged));

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var graphControl = (GraphControl)d;
            if (e.OldValue != null)
            {
                var collection = (INotifyCollectionChanged)e.OldValue;
                collection.CollectionChanged -= graphControl.OnItemsCollectionChanged;
            }

            if (e.NewValue != null)
            {
                var collection = (ObservableCollection<int>)e.NewValue;
                collection.CollectionChanged += graphControl.OnItemsCollectionChanged;
            }

            graphControl.DrawGraph();
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Replace:
                    RemoveCircleFromGraphCanvasByIndex(e.NewStartingIndex);
                    AddCircleToGraphCanvasByIndex(e.NewStartingIndex);
                    break;
                default:
                    DrawGraph();
                    break;
            }
        }

        private double m_XScale = 1;
        private double m_YScale = 1;
        private double m_CircleDiameter = 1;

        private double m_XBorderOffset = 0;
        private double m_YBorderOffset = 0;

        private const double c_MarginOffset = 1;

        public GraphControl()
        {
            InitializeComponent();

            m_XBorderOffset = graphBorder.BorderThickness.Top + graphBorder.BorderThickness.Bottom;
            m_YBorderOffset = graphBorder.BorderThickness.Left + graphBorder.BorderThickness.Right;
        }

        private void CalculateScalesAndDiameter()
        {
            int count = Items.Count;
            int maxValue = (count > 0 ? Items.Max() : 0) + 1;

            m_XScale = (Width - m_XBorderOffset - (2 * c_MarginOffset)) / (double)count;
            m_YScale = (Height - m_YBorderOffset - (2 * c_MarginOffset)) / (double)maxValue;
            
            m_CircleDiameter = Math.Max(1, Math.Min(m_XScale, m_YScale));
        }

        private void DrawGraph()
        {
            if (Items == null)
                return;
            CalculateScalesAndDiameter();

            graphCanvas.Children.Clear();
            for (int i = 0; i < Items.Count; i++)
                AddCircleToGraphCanvasByIndex(i);
        }

        private void AddCircleToGraphCanvasByIndex(int i)
        {
            Ellipse circle = new Ellipse();
            circle.Name = string.Format("circle{0}", i);
            circle.Width = circle.Height = m_CircleDiameter;
            circle.StrokeThickness = 1;
            circle.Stroke = Brushes.Blue;
            circle.Fill = Brushes.Blue;
            Canvas.SetLeft(circle, (i * m_XScale) + c_MarginOffset);
            Canvas.SetBottom(circle, (Items[i] * m_YScale) + c_MarginOffset);
            graphCanvas.Children.Add(circle);
        }

        private void RemoveCircleFromGraphCanvasByIndex(int i)
        {
            var circle = (UIElement)LogicalTreeHelper.FindLogicalNode(graphCanvas, string.Format("circle{0}", i));
            if (circle != null)
                graphCanvas.Children.Remove(circle);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawGraph();
        }
    }
}
