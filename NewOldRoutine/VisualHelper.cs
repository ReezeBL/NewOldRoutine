using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NewOldRoutine
{
    /// <summary>
    /// more info at http://www.eidias.com/blog/2014/8/15/movable-rows-in-wpf-datagrid
    /// </summary>
    public class VisualHelper
    {
        public static readonly DependencyProperty EnableRowsMoveProperty =
            DependencyProperty.RegisterAttached("EnableRowsMove", typeof(bool), typeof(VisualHelper),
                new PropertyMetadata(false, EnableRowsMoveChanged));

        public static bool GetEnableRowsMove(DataGrid obj)
        {
            return (bool) obj.GetValue(EnableRowsMoveProperty);
        }

        public static void SetEnableRowsMove(DataGrid obj, bool value)
        {
            obj.SetValue(EnableRowsMoveProperty, value);
        }

        private static void EnableRowsMoveChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(dependencyObject is DataGrid grid)) return;

            if ((bool) e.NewValue)
            {
                grid.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
                grid.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
                grid.PreviewMouseMove += OnMouseMove;
            }
            else
            {
                grid.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
                grid.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
                grid.PreviewMouseMove -= OnMouseMove;
            }
        }

        private static readonly DependencyProperty DraggedItemProperty =
            DependencyProperty.RegisterAttached("DraggedItem", typeof(object), typeof(VisualHelper),
                new PropertyMetadata(null));

        public static object GetDraggedItem(DependencyObject obj)
        {
            return obj.GetValue(DraggedItemProperty);
        }

        public static void SetDraggedItem(DependencyObject obj, object value)
        {
            obj.SetValue(DraggedItemProperty, value);
        }


        private static void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            var draggedItem = GetDraggedItem(sender as DependencyObject);
            if (draggedItem == null) return;
            var row = TryFindFromPoint<DataGridRow>((UIElement) sender, mouseEventArgs.GetPosition(sender as DataGrid));
            if (row == null || row.IsEditing) return;
            ExchangeItems(sender, row.Item);
        }

        private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var draggedItem = GetDraggedItem(sender as DependencyObject);
            if (draggedItem == null || !(sender is DataGrid dataGrid)) return;
            ExchangeItems(sender, dataGrid.SelectedItem);

            dataGrid.SelectedItem = draggedItem;
            SetDraggedItem(dataGrid, null);
        }

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var row = TryFindFromPoint<DataGridRow>((UIElement)sender, mouseButtonEventArgs.GetPosition(sender as DataGrid));
            if (row == null || row.IsEditing) return;
            SetDraggedItem(sender as DataGrid, row.Item);
        }

        private static void ExchangeItems(object sender, object targetItem)
        {
            var draggedItem = GetDraggedItem(sender as DependencyObject);
            if(draggedItem == null) return;

            if (targetItem != null && 
                !ReferenceEquals(draggedItem, targetItem) && 
                sender is DataGrid dataGrid &&
                dataGrid.ItemsSource is IList list)
            {
                var targetIndex = list.IndexOf(targetItem);
                list.Remove(draggedItem);
                list.Insert(targetIndex, draggedItem);
            }
        }

        public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (true)
            {
                var parentObject = VisualTreeHelper.GetParent(child);
                if (parentObject == null) return null;
                if (parentObject is T parent)
                    return parent;
                child = parentObject;
            }
        }

        public static T TryFindFromPoint<T>(UIElement reference, Point point)
            where T : DependencyObject
        {
            var element = reference.InputHitTest(point) as DependencyObject;
            if (element == null) return null;
            if (element is T tElement) return tElement;
            return FindVisualParent<T>(element);
        }
    }
}
