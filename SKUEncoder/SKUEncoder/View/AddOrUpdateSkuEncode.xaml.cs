using SKUEncoder.Entities;
using SKUEncoder.Entity;
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
using System.Windows.Shapes;

namespace SKUEncoder.View
{
    /// <summary>
    /// AddOrUpdateSkuEncode.xaml 的交互逻辑
    /// </summary>
    public partial class AddOrUpdateSkuEncode : Window
    {
        public AddOrUpdateSkuEncode(SKUEncodeDetail detail)
        {
            InitializeComponent();
            this.Title = "创建SKU编码";
            this.ViewModel = new VMAddOrUpdateSkuEncode(detail);
            this.ViewModel.HandleCompleted += ViewModel_HandleCompleted;
        }

        public AddOrUpdateSkuEncode(SKUEncodeModel model)
        {
            InitializeComponent();
            this.Title = "修改SKU编码";
            this.ViewModel = new VMAddOrUpdateSkuEncode(model);
            this.ViewModel.HandleCompleted += ViewModel_HandleCompleted;
        }

        public event EventHandler<EntityEventArgs> HandleCompleted;

        public VMAddOrUpdateSkuEncode ViewModel
        {
            get
            {
                return this.DataContext as VMAddOrUpdateSkuEncode;
            }
            private set
            {
                this.DataContext = value;
            }
        }

        private void ViewModel_HandleCompleted(object sender, EntityEventArgs e)
        {
            if (this.HandleCompleted != null)
            {
                this.HandleCompleted(this, e);
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
