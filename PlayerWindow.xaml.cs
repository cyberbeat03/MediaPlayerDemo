using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WinMix;

public partial class PlayerWindow : Window
{
    public PlayerWindow(PlayerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        StatusText.Focus();

        if (viewModel?.MediaItems is INotifyCollectionChanged coll)
        {
            coll.CollectionChanged += MediaItems_CollectionChanged;
            this.Unloaded += (s, e) => coll.CollectionChanged -= MediaItems_CollectionChanged;
        }
    }

    void MediaItems_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // Run on UI thread after collection update finishes
        Dispatcher.BeginInvoke(() =>
        {
            // If nothing to select, clear selection and focus the Add button
            if (ItemList.Items.Count == 0)
            {
                ItemList.SelectedItem = null;
                Keyboard.Focus(ItemList);
                return;
            }

            // If there is a selected item, keep it selected and ensure it is in view
            if (ItemList.SelectedItem != null)
            {
                ItemList.ScrollIntoView(ItemList.SelectedItem);
                // try to focus the generated ListViewItem so arrow keys operate on it
                var container = ItemList.ItemContainerGenerator.ContainerFromItem(ItemList.SelectedItem) as ListViewItem;
                if (container != null)
                {
                    container.Focus();
                    Keyboard.Focus(container);
                }
                else
                {
                    ItemList.Focus();
                    Keyboard.Focus(ItemList);
                }
                return;
            }

            // Otherwise, try to select a reasonable item near the changed index
            int newIndex = 0;
            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldStartingIndex >= 0)
            {
                newIndex = Math.Min(e.OldStartingIndex, ItemList.Items.Count - 1);
            }
            else if (e.Action == NotifyCollectionChangedAction.Move && e.NewStartingIndex >= 0)
            {
                newIndex = e.NewStartingIndex;
            }

            if (newIndex >= 0 && newIndex < ItemList.Items.Count)
            {
                ItemList.SelectedIndex = newIndex;
                ItemList.ScrollIntoView(ItemList.SelectedItem);
                var container = ItemList.ItemContainerGenerator.ContainerFromIndex(newIndex) as ListViewItem;
                if (container != null)
                {
                    container.Focus();
                    Keyboard.Focus(container);
                }
                else
                {
                    ItemList.Focus();
                    Keyboard.Focus(ItemList);
                }
            }
        }, DispatcherPriority.Input);
    }

}
