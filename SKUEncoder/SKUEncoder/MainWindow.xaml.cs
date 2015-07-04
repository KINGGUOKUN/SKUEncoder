using SKUEncoder.View;
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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private OneManagement _oneMangement;
        private TwoManagement _twoManagement;
        private AttManagement _attManagement;
        private SKUEncodeManagement _skuEncodeManagement;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 一级目录管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbOne_Click(object sender, RoutedEventArgs e)
        {
            this.tbTwo.IsChecked = false;
            this.tbAtt.IsChecked = false;
            this.tbSKU.IsChecked = false;
            if(_oneMangement == null)
            {
                _oneMangement = new OneManagement();
            }
            this.frmSKUEncoder.Navigate(_oneMangement);
        }

        /// <summary>
        /// 二级目录管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbTwo_Click(object sender, RoutedEventArgs e)
        {
            this.tbOne.IsChecked = false;
            this.tbAtt.IsChecked = false;
            this.tbSKU.IsChecked = false;
            if(_twoManagement == null)
            {
                _twoManagement = new TwoManagement();
            }
            this.frmSKUEncoder.Navigate(_twoManagement);
        }

        /// <summary>
        /// 属性管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbAtt_Click(object sender, RoutedEventArgs e)
        {
            this.tbOne.IsChecked = false;
            this.tbTwo.IsChecked = false;
            this.tbSKU.IsChecked = false;
            if(_attManagement == null)
            {
                _attManagement = new AttManagement();
            }
            this.frmSKUEncoder.Navigate(_attManagement);
        }

        /// <summary>
        /// SKU管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbSKU_Click(object sender, RoutedEventArgs e)
        {
            this.tbOne.IsChecked = false;
            this.tbTwo.IsChecked = false;
            this.tbAtt.IsChecked = false;
            if (_skuEncodeManagement == null)
            {
                _skuEncodeManagement = new SKUEncodeManagement();
            }
            this.frmSKUEncoder.Navigate(_skuEncodeManagement);
        }
    }
}
