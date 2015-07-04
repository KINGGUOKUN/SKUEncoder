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

namespace SKUEncoder
{
    /// <summary>
    /// OneManagement.xaml 的交互逻辑
    /// </summary>
    public partial class OneManagement : UserControl
    {       
        public OneManagement()
        {
            InitializeComponent();
            this.ViewModel = new VMOneManagement();
        }

        /// <summary>
        /// 窗体ViewModel
        /// </summary>
        public VMOneManagement ViewModel
        {
            get
            {
                return this.DataContext as VMOneManagement;
            }
            private set
            {
                this.DataContext = value;
            }
        }

        private void dgOne_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
            if(chb != null)
            {
                string ID = chb.Tag.ToString();
                this.ViewModel.SelectedItem = this.ViewModel.ItemsSource.FirstOrDefault(x => x.ID.ToString() == ID);
                if(chb.IsChecked.HasValue)
                {
                    this.ViewModel.SelectedItem.IsChecked = chb.IsChecked.Value;
                }
            }
            (this.ViewModel.CmdDel as DelegateCommand).RaiseCanExecuteChanged();
            (this.ViewModel.CmdExport as DelegateCommand).RaiseCanExecuteChanged();
        }
    }
}
