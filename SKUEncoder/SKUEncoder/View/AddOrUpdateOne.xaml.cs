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
    /// AddOrUpdateOne.xaml 的交互逻辑
    /// </summary>
    public partial class AddOrUpdateOne : Window
    {
        public AddOrUpdateOne(SKUCGYModel model)
        {
            InitializeComponent();
            this.ViewModel = new VMAddOrUpdateOne(model);
            this.ViewModel.HandleCompleted += ViewModel_HandleCompleted;
            this.Title = model == null ? "添加一级" : "修改一级";
            this.txbCode.IsReadOnly = model != null;
        }

        public event EventHandler<EntityEventArgs> HandleCompleted;

        public VMAddOrUpdateOne ViewModel
        {
            get
            {
                return this.DataContext as VMAddOrUpdateOne;
            }
            private set
            {
                this.DataContext = value;
            }
        }


        private void ViewModel_HandleCompleted(object sender, EntityEventArgs e)
        {
            if(this.HandleCompleted != null)
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
