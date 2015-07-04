using Microsoft.Practices.Prism.Commands;
using SKUEncoder.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SKUEncoder.View
{
    /// <summary>
    /// AttManagement.xaml 的交互逻辑
    /// </summary>
    public partial class AttManagement : UserControl
    {
        public AttManagement()
        {
            InitializeComponent();
            this.ViewModel = new VMAttManagement();
            this.ViewModel.TreeViewRefreshed += (s, e) =>
            {
                TreeViewItem rootItem = this.treeSKUCGY.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;
                if(rootItem != null)
                {
                    rootItem.IsSelected = true;
                    rootItem.IsExpanded = true;
                }     
            };
            this.Loaded += (s, e) =>
                {
                    TreeViewItem rootItem = this.treeSKUCGY.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;
                    if (rootItem != null)
                    {
                        rootItem.IsSelected = true;
                        rootItem.IsExpanded = true;
                    } 
                };
        }

        public VMAttManagement ViewModel
        {
            get
            {
                return this.DataContext as VMAttManagement;
            }
            private set
            {
                this.DataContext = value;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.ViewModel.SelectedSKUCGY = this.treeSKUCGY.SelectedItem as Entity.SKUCGY;
        }

        private void dgATT_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsMouseDoubleClickDataGridRow(sender, e))
            {
                return;
            }
            this.ViewModel.CmdUpdate.Execute(null);
        }

        private bool IsMouseDoubleClickDataGridRow(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dependencyObject = e.OriginalSource as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is DataGridRow)
                {
                    return true;
                }
                else
                {
                    dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
                }
            }
            return false;
        }

        private void cbStatus_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chb = sender as CheckBox;
            if (chb != null)
            {
                string ID = chb.Tag.ToString();
                this.ViewModel.SelectedItem = this.ViewModel.ItemsSource.FirstOrDefault(x => x.ID.ToString() == ID);
                if (chb.IsChecked.HasValue)
                {
                    this.ViewModel.SelectedItem.IsChecked = chb.IsChecked.Value;
                }
            }
            (this.ViewModel.CmdDel as DelegateCommand).RaiseCanExecuteChanged();
            (this.ViewModel.CmdExport as DelegateCommand).RaiseCanExecuteChanged();
        }
    }
}
